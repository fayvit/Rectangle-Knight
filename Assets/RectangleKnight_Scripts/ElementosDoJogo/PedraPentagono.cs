using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedraPentagono : MonoBehaviour
{
    [SerializeField] private string ID;
    [SerializeField] private GameObject particulaDeCarregado = default;
    [SerializeField] private GameObject particulaDeAcao = default;
    [SerializeField] private SpriteRenderer meuSprite=default;
    [SerializeField] private int contCargasTotais = 5;
    [SerializeField] private int taxaDeRecuperacao = 10;
    [SerializeField] private AudioClip som = default;

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    // Start is called before the first frame update
    void Start()
    {
        KeyVar kv = GameController.g.MyKeys;
        if (ExistenciaDoController.AgendaExiste(Start, this))
        {
            if (kv.VerificaEnemyShift(ID))
            {
                Desativar();
            }
            else
            {
                if (!kv.VerificaEnemyShift("limparContPentagono" + ID))
                    kv.MudaAutoCont(ID, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Desativar()
    {
        particulaDeCarregado.SetActive(false);
        meuSprite.color = new Color(.75f, .75f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KeyVar kv = GameController.g.MyKeys;
        if (collision.tag == "attackCollisor" && !kv.VerificaEnemyShift(ID))
        {
            //kv.SomaAutoCont(ID, 1);
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, som));
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.sumContShift, ID, 1));
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeEnemyKey,"limparContPentagono" + ID));

            if (kv.VerificaAutoCont(ID) >= contCargasTotais)
            {
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeEnemyKey, ID));
                Desativar();
                new MyInvokeMethod().InvokeNoTempoDeJogo(()=> {
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.exitCheckPoint));
                },.5f);
            }

            InstanciaLigando.Instantiate(particulaDeAcao, 0.5f * (collision.transform.position + transform.position),5);
            DadosDoJogador dj = GameController.g.Manager.Dados;

            if (dj.PontosDeMana < dj.MaxMana)
            {
                dj.AdicionarMana(taxaDeRecuperacao);
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.changeMagicPoints, dj.PontosDeMana, dj.MaxMana));
            }
        }
    }
}
