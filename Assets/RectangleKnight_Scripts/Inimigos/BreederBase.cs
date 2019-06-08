using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreederBase : EnemyBase
{
    #region inspector
    [SerializeField] private GameObject breed = default;
    [SerializeField] private GameObject particulaDoSpawn = default;
    [SerializeField] private int numBaseDeSpawnaveis = 7;
    [SerializeField] private int numMaxSpawnaveis = 13;
    [SerializeField] private float tempoTelegrafando = 0.5f;
    #endregion

    private float tempoDecorrido;
    
    protected void RequestAction(Vector3 charPos)
    {

        FlipDirection.Flip(transform, charPos.x - transform.position.x);
        GameObject G = InstanciaLigando.Instantiate(particulaDoSpawn, transform.position, 5);
        InstanciaLigando.Instantiate(breed, transform.position);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.lancaProjetilInimigo));
        spawnados.Add(G);
        //RetornarParaEsperaZerandoTempo();
    }

    bool DecidaSpawnar()
    {
        float f = Random.Range(0, 1);
        int num = spawnados.Count;
        if (f < 0.75f - (num - numBaseDeSpawnaveis) / num && num < numMaxSpawnaveis)
            return true;
        else
            return false;
    }

    protected void Telegrafar(Vector3 charPos)
    {
        if (_Animator == null)
            _Animator = GetComponent<Animator>();

        VerifiqueVivos();
        Debug.Log("spawnados: " + spawnados.Count);

        if (spawnados.Count < numBaseDeSpawnaveis)
        {
            SimTelegrafar(charPos);
        }
        else
        {
            if (DecidaSpawnar())
            {
                SimTelegrafar(charPos);
            }
            //else
              //  RetornarParaEsperaZerandoTempo();

        }
    }

    /*
    protected void RetornarParaEsperaZerandoTempo()
    {
        //estado = EstadoDaqui.emEspera;
        tempoDecorrido = 0;
        //VerifiquePosicionamento();
    }*/

    void SimTelegrafar(Vector3 charPos)
    {
        new MyInvokeMethod().InvokeNoTempoDeJogo(gameObject,
            () => {
                RequestAction(charPos);
                }, tempoTelegrafando);
        FlipDirection.Flip(transform, charPos.x - transform.position.x);
        _Animator.SetTrigger("action");
        // RetornarParaEsperaZerandoTempo();
    }

    private List<GameObject> spawnados = new List<GameObject>();

    void VerifiqueVivos()
    {
        spawnados.RemoveAll(u => u == null);
    }
}
