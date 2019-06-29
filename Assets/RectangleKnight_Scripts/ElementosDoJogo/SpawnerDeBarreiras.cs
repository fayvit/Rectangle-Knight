using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerDeBarreiras : MonoBehaviour
{
    [SerializeField] private GameObject[] barreiraSpawnavel=default;
    [SerializeField] private Transform xDaDestruicao=default;
    [SerializeField] private float intervalo = 3;

    [SerializeField]private bool ligado;
    [SerializeField]private float tempoDecorrido = 0;
//    [SerializeField] private float resquestColorInterval = 1;
    [SerializeField] private int requestColor = 0;

    [SerializeField]private List<GameObject> spawnados = new List<GameObject>();

    //private float requestColorTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        tempoDecorrido = intervalo;
        EventAgregator.AddListener(EventKey.triggerInfo, OnReceivedTriggerInfo);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.triggerInfo, OnReceivedTriggerInfo);
    }

    void OnReceivedTriggerInfo(IGameEvent e)
    {
        if (e.Sender.transform.IsChildOf(transform))
        {
            Collider2D c = (Collider2D)(((StandardSendGameEvent)e).MyObject[0]);

            if (c.tag == "Player")
            {
                if (e.Sender.name == "ligador")
                {
                    ligado = true;
                }
                else if (e.Sender.name == "desligador")
                {
                    ligado = false;
                }
                else if (e.Sender.name == "requestColor")
                {

                    VerifiqueSpawnarCorChave();

                }
            }
        }
    }

    void VerifiqueSpawnarCorChave()
    {
        ligado = true;
      //  if (requestColorTime == 0 || Time.time - requestColorTime > resquestColorInterval)
        {
            bool foi = false;
            if (spawnados.Count > 0)
                if (spawnados[spawnados.Count - 1] != null)
                {
                    spawnados[spawnados.Count - 1].SetActive(false);
                    GameObject G = InstanciaLigando.Instantiate(barreiraSpawnavel[requestColor], spawnados[spawnados.Count - 1].transform.position);
                    Destroy(spawnados[spawnados.Count - 1]);
                    spawnados.Add(G);
                    foi = true;
                }

            if (!foi)
            {
                GameObject G = InstanciaLigando.Instantiate(barreiraSpawnavel[requestColor], transform.position);
                spawnados.Add(G);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ligado)
        {
            tempoDecorrido += Time.deltaTime;
            if (tempoDecorrido > intervalo)
            {
                tempoDecorrido = 0;
                int sorteio = Random.Range(0, barreiraSpawnavel.Length);
                
                GameObject G = InstanciaLigando.Instantiate(barreiraSpawnavel[sorteio],transform.position);
                
                spawnados.Add(G);
            }
        }


        
        List<int> indicesParaRemover = new List<int>();
        for (int i = 0; i < spawnados.Count; i++)
        {
            float sinal = Mathf.Sign(transform.position.x - xDaDestruicao.position.x);
            if (spawnados[i] != null)
            {
                if (sinal*(spawnados[i].transform.position.x - xDaDestruicao.position.x) < 0.01f * sinal)
                {
                    spawnados[i].GetComponent<ColorBarrageSemID>().DestruicaoSemID();                    
                    indicesParaRemover.Add(i);
                }
            }
            else
            {
                indicesParaRemover.Add(i);
            }
        }

        for (int i = 0; i < indicesParaRemover.Count; i++)
            spawnados.RemoveAt(indicesParaRemover[indicesParaRemover.Count - 1 - i]);
    }
}
