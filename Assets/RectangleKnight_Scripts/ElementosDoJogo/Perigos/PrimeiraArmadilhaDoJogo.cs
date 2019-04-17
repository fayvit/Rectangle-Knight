using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimeiraArmadilhaDoJogo : CheckConclusionOfID
{
    #region inspector
    [SerializeField] private GameObject[] barreiras = default;
    [SerializeField] private GameObject particulaDoInicio = default;
    [SerializeField] private GameObject[] monitorado = default;
    [SerializeField] private GameObject[] spawnaveis = default;
    [SerializeField] private DadosDeCena.LimitantesDaCena limitantes = default;
    #endregion

    private string[] IdsMonitorados;
    private bool jaIniciaou = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        EventAgregator.AddListener(EventKey.requestChangeEnemyKey, OnEnemyDefeated);
        EventAgregator.AddListener(EventKey.requestChangeEnemyKey, OnFinalizeDefeated);

        /*
        IdsMonitorados = new string[monitorado.Length];

        for(int i=0;i<monitorado.Length;i++)
            IdsMonitorados[i] = monitorado[i].GetComponent<NonRespawnOnLoadEnemy>().GetID;
*/
        base.Start();
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.requestChangeEnemyKey, OnEnemyDefeated);
        EventAgregator.RemoveListener(EventKey.requestChangeEnemyKey, OnFinalizeDefeated);
    }

    private void OnFinalizeDefeated(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;
        GameObject[] quem = new GameObject[spawnaveis.Length];

        for (int i = 0; i < spawnaveis.Length; i++)
        {
            
            EnemyBase eny = spawnaveis[i].GetComponentInChildren<EnemyBase>();

            if (eny != null)
                quem[i] = eny.gameObject;
            else
                quem[i] = null;

            if (ssge.Sender == quem[i])
            {
                StartCoroutine(VerificaTodosSpawnadosDerrotados(quem));

                new MyInvokeMethod().InvokeAoFimDoQuadro(() =>
                {
                    if (TodosDerrotados(quem))
                        EventAgregator.Publish(EventKey.returnRememberedMusic, null);
                });


            }
        }
    }

    private void OnEnemyDefeated(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;

        for (int i = 0; i < monitorado.Length;i++)
        {
            if (ssge.Sender == monitorado[i])
            {
                StartCoroutine(VerificaTodosDerrotados());
                
            }
        }
    }

    void FinalizaArmadilha()
    {
        for (int i = 0; i < barreiras.Length; i++)
        {
            barreiras[i].GetComponent<DestructibleWithAttack>().Destruicao();
        }

        RequestChangeKey();
        EventAgregator.Publish(EventKey.requestSceneCamLimits, null);
        Destroy(gameObject);
    }

    IEnumerator VerificaTodosSpawnadosDerrotados(GameObject[] quem)
    {
        yield return new WaitForSeconds(2);

        if (TodosDerrotados(quem))
        {
            FinalizaArmadilha();
        }
    }

    IEnumerator VerificaTodosDerrotados()
    {
        yield return new WaitForSeconds(2);

        if (TodosDerrotados(monitorado))
        {
            AcionarSpawnaveis();
        }

    }

    void AcionarSpawnaveis()
    {
        for (int i = 0; i < spawnaveis.Length; i++)
        {
            if (Vector3.Distance(GameController.g.Manager.transform.position, spawnaveis[i].transform.position) < 1)
                spawnaveis[i].transform.position += 2 * Vector3.right;

            InstanciaLigando.Instantiate(particulaDoInicio, spawnaveis[i].transform.position);

            spawnaveis[i].SetActive(true);
        }

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, "Fire1"));
    }

    private bool TodosDerrotados(GameObject[] quais)
    {
        for (int i = 0; i < quais.Length; i++)
        {
            Debug.Log("quem sou eu: "+quais[i]);

            if (quais[i] != null)
                return false;
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player" &&!jaIniciaou)
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                jaIniciaou = true;

                for (int i = 0; i < barreiras.Length; i++)
                {
                    InstanciaLigando.Instantiate(particulaDoInicio, barreiras[i].transform.position,5);
                    barreiras[i].SetActive(true);
                }

                EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeCamLimits, limitantes,3f));
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.pedrasQuebrando));
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.changeMusicWithRecovery,
                    new NameMusicaComVolumeConfig()
                    {
                    Musica= NameMusic.trapMusic,
                    Volume = 1
                    }));
            }
        }
        
    }
}
