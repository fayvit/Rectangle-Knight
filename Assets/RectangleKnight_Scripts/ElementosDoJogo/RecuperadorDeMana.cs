using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecuperadorDeMana : MonoBehaviour
{
    private float tempoDecorrido = 0;
    [SerializeField] private float intervaloDeRecuperacao = 1;
    [SerializeField] private int taxaDeRecuperacao = 1;
    [SerializeField] private GameObject particulaDaAcao;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                tempoDecorrido += Time.deltaTime;
                if (tempoDecorrido > intervaloDeRecuperacao)
                { 

                    DadosDoJogador dj = GameController.g.Manager.Dados;

                    tempoDecorrido = 0;

                    if (dj.PontosDeMana < dj.MaxMana)
                    {
                        dj.AdicionarMana(taxaDeRecuperacao);
                        EventAgregator.Publish(new StandardSendGameEvent(EventKey.changeMagicPoints, dj.PontosDeMana, dj.MaxMana));

                        InstanciaLigando.Instantiate(particulaDaAcao, GameController.g.Manager.transform.position);
                    }
                }
            }
        }
    }
}
