using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class RestritorDeCamLimits {
    [SerializeField] private RestringirCamLimits[] camLimits = default;

    private DadosDeCena.LimitantesDaCena limitantes;

    [System.Serializable]
    private struct RestringirCamLimits
    {
        public enum QualRestricao
        {
            xMin,
            xMax,
            yMin,
            yMax
        }

        [SerializeField] private QualRestricao onde;
        [SerializeField] private float val;

        public float Val { get => val; set => val = value; }
        public QualRestricao Onde { get => onde; set => onde = value; }
    }

    public void VerifiqueLimitantesParaMudeCena()
    {
        VerifiqueLimitantesParaMudeCena(StringParaEnum.ObterEnum<NomesCenas>(SceneManager.GetActiveScene().name));
    }

    public void VerifiqueLimitantesParaMudeCena(NomesCenas cenaAlvo)
    {
        if(camLimits!=null)
        if (camLimits.Length > 0)
        {
            limitantes = (DadosDeCena.LimitantesDaCena)GlobalController.g.SceneDates.GetSceneDates(cenaAlvo).limitantes.Clone();

            for (int i = 0; i < camLimits.Length; i++)
            {
                AltereLimitante(camLimits[i]);
            }

            SceneManager.activeSceneChanged += ModifiqueLimitantesAct;
            SceneManager.sceneLoaded += ModifiqueLimitantes;
        }
    }

    public void MudeLimitantesParaTrigger(float tempoDeLerpLimits)
    {
        MudeLimitantesParaTrigger(StringParaEnum.ObterEnum<NomesCenas>(SceneManager.GetActiveScene().name),tempoDeLerpLimits);
    }

    public void MudeLimitantesParaTrigger(NomesCenas cenaAlvo,float tempoDeLerpLimits)
    {
       
        limitantes = (DadosDeCena.LimitantesDaCena)GlobalController.g.SceneDates.GetSceneDates(cenaAlvo).limitantes.Clone();

        if (camLimits != null)
            if (camLimits.Length > 0)
            {
                //Debug.Log("send event A" + limitantes.xMax + " : " + tempoDeLerpLimits);

                for (int i = 0; i < camLimits.Length; i++)
                {
                    AltereLimitante(camLimits[i]);
                }

            }

        
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeCamLimits, limitantes, tempoDeLerpLimits));
    }

    private void ModifiqueLimitantesAct(Scene arg0, Scene arg1)
    {
        GameController.g.StartCoroutine(AtrasoDeMudaCam());
        SceneManager.activeSceneChanged -= ModifiqueLimitantesAct;
    }

    private void ModifiqueLimitantes(Scene arg0, LoadSceneMode arg1)
    {
        GameController.g.StartCoroutine(AtrasoDeMudaCam());
        SceneManager.sceneLoaded -= ModifiqueLimitantes;
    }

    IEnumerator AtrasoDeMudaCam()
    {
        yield return new WaitForEndOfFrame();
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeCamLimits, limitantes, 0f));
    }

    void AltereLimitante(RestringirCamLimits camLimits)
    {
        switch (camLimits.Onde)
        {
            case RestringirCamLimits.QualRestricao.xMax:
                limitantes.xMax = camLimits.Val;
            break;
            case RestringirCamLimits.QualRestricao.xMin:
                limitantes.xMin = camLimits.Val;
            break;
            case RestringirCamLimits.QualRestricao.yMax:
                limitantes.yMax = camLimits.Val;
            break;
            case RestringirCamLimits.QualRestricao.yMin:
                limitantes.yMin = camLimits.Val;
            break;
        }
    }
}