using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecuperadorDeMana : MonoBehaviour
{
    private float tempoDecorrido = 0;
    private bool ativo = false;
    [SerializeField] private float intervaloDeRecuperacao = 1;
    [SerializeField] private int taxaDeRecuperacao = 1;

    #region inspector
    [SerializeField] private GameObject particulaDaAcao = default;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                Debug.Log("ativou");
                ativo = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                ativo = false;
            }
        }
    }

    private void Update()
    {
        Debug.Log("updateando: " + ativo);
        if(ativo)
        { 
            tempoDecorrido += Time.deltaTime;
            if (tempoDecorrido > intervaloDeRecuperacao)
            {

                Recupera();
            }
                
            
        }
    }

    void Recupera()
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
