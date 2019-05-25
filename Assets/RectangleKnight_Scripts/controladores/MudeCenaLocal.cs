using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MudeCenaLocal : MonoBehaviour
{
    [SerializeField] private NomesCenas[] cenasAlvo = default;
    [SerializeField] private RestringirCamLimits[] camLimits;

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

        public QualRestricao onde;
        public float val;

    }

    void VerifiqueLimitantes()
    {
        if (camLimits.Length > 0)
        {
            limitantes = GlobalController.g.SceneDates.GetSceneDates(cenasAlvo[0]).limitantes;

            for (int i = 0; i < camLimits.Length;i++)
            {
                AltereLimitante(camLimits[i]);
            }

            SceneManager.activeSceneChanged += ModifiqueLimitantesAct;
            SceneManager.sceneLoaded += ModifiqueLimitantes;
        }
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
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeCamLimits, limitantes, 0));
    }

    void AltereLimitante(RestringirCamLimits camLimits)
    {
        switch (camLimits.onde)
        {
            case RestringirCamLimits.QualRestricao.xMax:
                limitantes.xMax = camLimits.val;
            break;
            case RestringirCamLimits.QualRestricao.xMin:
                limitantes.xMin = camLimits.val;
            break;
            case RestringirCamLimits.QualRestricao.yMax:
                limitantes.yMax = camLimits.val;
            break;
            case RestringirCamLimits.QualRestricao.yMin:
                limitantes.yMin = camLimits.val;
            break;
        }
    }


    void OnFadeOutComplete()
    {
        StaticMudeCena.OnFadeOutComplete(cenasAlvo, cenasAlvo[0], transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                GlobalController.g.FadeV.IniciarFadeOutComAction(OnFadeOutComplete, 0.5f);

                VerifiqueLimitantes();
            }

        }
    }
}
