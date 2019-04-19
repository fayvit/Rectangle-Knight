using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreederEnemy : StrategyMovementEnemy
{
    #region inspector
    [SerializeField] private GameObject breed = default;
    [SerializeField] private GameObject particulaDoSpawn = default;
    #endregion

    [SerializeField] private Animator animador;

    protected override void Start()
    {
        animador = GetComponent<Animator>();
        base.Start();
    }

    private List<GameObject> spawnados = new List<GameObject>();

    void VerifiqueVivos()
    {
        List<int> indicesMortos = new List<int>();

        for (int i = 0; i < spawnados.Count; i++)
        {
            if (spawnados[i] == null)
                indicesMortos.Add(i);
        }

        for (int i = indicesMortos.Count - 1; i >= 0; i++)
        {
            spawnados.RemoveAt(i);
        }
    }

    protected override void RequestAction(Vector3 charPos)
    {

        FlipDirection.Flip(transform, charPos.x - transform.position.x);
        GameObject G = InstanciaLigando.Instantiate(particulaDoSpawn, transform.position, 5);
        InstanciaLigando.Instantiate(breed, transform.position);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.lancaProjetilInimigo));
        spawnados.Add(G);        
        RetornarParaEsperaZerandoTempo();
    }


    bool DecidaSpawnar()
    {
        float f = Random.Range(0, 1);
        int num = spawnados.Count;
        if (f < 0.75f - (num - 3f) / num && num > 3)
            return true;
        else
            return false;
    }

    protected override void Telegrafar(Vector3 charPos)
    {
        Debug.Log("spawnados: "+spawnados.Count);

        if (spawnados.Count < 3)
        {
            SimTelegrafar(charPos);
        }
        else
        {
            if (DecidaSpawnar())
            {
                SimTelegrafar(charPos);
            }else
                RetornarParaEsperaZerandoTempo();

        }

        

    }

    void SimTelegrafar(Vector3 charPos)
    {
        FlipDirection.Flip(transform, charPos.x - transform.position.x);
        animador.SetTrigger("action");
       // RetornarParaEsperaZerandoTempo();
    }
}
