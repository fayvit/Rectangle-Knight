using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDamage
{
    public bool agendado = false;
    public Vector3 pos;

    public void Iniciar()
    {
        Time.timeScale = 0;
        GlobalController.g.FadeV.IniciarFadeOutComAction(OnFadeOutComplet);
    }

    void OnFadeOutComplet()
    {
        Time.timeScale = 1;
        GameController.g.Manager.transform.position = pos;
        GameController.g.StartCoroutine(IniciarFadeIn());
        
    }

    IEnumerator IniciarFadeIn()
    {
        yield return new WaitForEndOfFrame();
        GlobalController.g.FadeV.IniciarFadeInComAction(OnFadeInComplete);

    }

    void OnFadeInComplete()
    {
        
        agendado = false;
        EventAgregator.Publish(EventKey.fechouPainelSuspenso);
    }
}
