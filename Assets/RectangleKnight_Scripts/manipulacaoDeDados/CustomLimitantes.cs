using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomLimitantes : MonoBehaviour
{
    [SerializeField] private DadosDeCena.LimitantesDaCena limitantes;
    [SerializeField] private NomesCenas cenaDosLimitantes = default; 
    [SerializeField] private float time = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {

                if (cenaDosLimitantes != NomesCenas.nula)
                    limitantes = GlobalController.g.SceneDates.GetSceneDates(cenaDosLimitantes).limitantes;

                SceneManager.activeSceneChanged += ModifiqueLimitantesAct;
                SceneManager.sceneLoaded += ModifiqueLimitantes;
            }
        }
    }

    private void ModifiqueLimitantesAct(Scene arg0,Scene arg1)
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
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeCamLimits, limitantes, time));
    }
}
