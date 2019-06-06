using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreederEnemy : StrategyMovementEnemy
{
    #region inspector
    [SerializeField] private GameObject breed = default;
    [SerializeField] private GameObject particulaDoSpawn = default;
    #endregion

    

    protected override void Start()
    {
        _Animator = GetComponent<Animator>();
        base.Start();
    }

    private List<GameObject> spawnados = new List<GameObject>();

    void VerifiqueVivos()
    {
        spawnados.RemoveAll(u=>u==null); 
        /*
        List<int> indicesMortos = new List<int>();

        for (int i = 0; i < spawnados.Count; i++)
        {
            if (spawnados[i] == null)
                indicesMortos.Add(i);
        }

        for (int i = indicesMortos.Count - 1; i >= 0; i++)
        {
            Debug.Log(indicesMortos.Count+": "+spawnados.Count+" : "+ i + " : " + indicesMortos[i]+" indices");
            spawnados. RemoveAt(indicesMortos[i]);
        }*/
    }

    protected override void RequestAction(Vector3 charPos)
    {

        FlipDirection.Flip(transform, charPos.x - transform.position.x);
        InstanciaLigando.Instantiate(particulaDoSpawn, transform.position, 5);
        GameObject G = InstanciaLigando.Instantiate(breed, transform.position);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.lancaProjetilInimigo));
        spawnados.Add(G);        
        RetornarParaEsperaZerandoTempo();
    }


    bool DecidaSpawnar()
    {
        float f = Random.Range(0, 1f);
        
        int num = spawnados.Count;

        Debug.Log(f + " : " + (0.75f - (num - 3f) / num) + " : " + num);

        if (f < 0.75f - (num - 3f) / num && num >= 3)
            return true;
        else
            return false;
    }

    protected override void Telegrafar(Vector3 charPos)
    {
        VerifiqueVivos();
        Debug.Log("spawnados: "+spawnados.Count);

        if (spawnados.Count < 3)
        {
            SimTelegrafar(charPos);
        }
        else
        {
            if (DecidaSpawnar())
            {
                Debug.Log("sim");
                SimTelegrafar(charPos);
            }
            else
            {
                RetornarParaEsperaZerandoTempo();
                Debug.Log("nao");
            }


        }

        

    }

    void SimTelegrafar(Vector3 charPos)
    {
        FlipDirection.Flip(transform, charPos.x - transform.position.x);
        _Animator.SetTrigger("action");
       // RetornarParaEsperaZerandoTempo();
    }
}
