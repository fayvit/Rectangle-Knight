using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DerrotaDoJogador
{
#pragma warning disable 0649
    [SerializeField]private GameObject losanguloFinalizador;
    [SerializeField] private Vector3 escalaFinal;
    [SerializeField] private AudioClip somDoLosangulo;
#pragma warning restore 0649

    private float tempoDecorrido = 0;
    private float tempoParaIniciarEscalonamento = 1;
    private float tempoDoEscalonamentoDoRetangulo = 1;
    private EstadoDaqui estado = EstadoDaqui.iniciando;

    private enum EstadoDaqui
    {
        iniciando,
        escalonandoLosangulo,
    }

    public void DesligarLosangulo()
    {
        tempoDecorrido = 0;
        losanguloFinalizador.SetActive(false);
    }

    public bool Update()
    {
        tempoDecorrido += Time.deltaTime;
        switch (estado)
        {
            case EstadoDaqui.iniciando:
                
                if (tempoDecorrido > tempoParaIniciarEscalonamento)
                {
                    tempoDecorrido = 0;
                    estado = EstadoDaqui.escalonandoLosangulo;
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, somDoLosangulo));
                }
            break;
            case EstadoDaqui.escalonandoLosangulo:
                losanguloFinalizador.SetActive(true);
                losanguloFinalizador.transform.localScale =
                    Vector3.Lerp(Vector3.zero, escalaFinal, tempoDecorrido / tempoDoEscalonamentoDoRetangulo);

                if (tempoDecorrido > tempoDoEscalonamentoDoRetangulo)
                {
                    tempoDecorrido = 0;
                    estado = EstadoDaqui.iniciando;
                    return true;
                }
            break;

                
        }
        return false;
    }

}