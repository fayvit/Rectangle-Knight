using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AtivadorDoCheckPoint : AtivadorDeBotao
{
#pragma warning disable 0649
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject standparticles;
    [SerializeField] private NomesCenas[] cenasparaLoad = new NomesCenas[1] { NomesCenas.TutoScene};
    [SerializeField] private NomesCenas cenaAtivaNoLoad = NomesCenas.nula;
#pragma warning restore 0649

    public override void FuncaoDoBotao()
    {
        if (GameController.g.Manager.Estado != EstadoDePersonagem.inCheckPoint)
        {
            GameObject G = Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(G, 5);
            DontDestroyOnLoad(G);

            GameController.g.MyKeys.ReviverInimigos();

            SaveDatesManager.SalvarAtualizandoDados();

            EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.startCheckPoint,cenasparaLoad));
            SceneManager.LoadScene(NomesCenasEspeciais.ComunsDeFase.ToString());

            for (int i = 0; i < cenasparaLoad.Length; i++)
            {
                SceneManager.LoadSceneAsync(cenasparaLoad[i].ToString(), LoadSceneMode.Additive);
            }
            SceneManager.sceneLoaded += OnLoadedScene;
        }
    }

    private void OnLoadedScene(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name != NomesCenasEspeciais.ComunsDeFase.ToString())
        {
            if (cenaAtivaNoLoad == NomesCenas.nula || arg0.name == cenaAtivaNoLoad.ToString())
            {
                SceneManager.SetActiveScene(arg0);
                SceneManager.sceneLoaded -= OnLoadedScene;

                //Debug.Log(SaveDatesManager.s.CurrentSaveDate);

                GlobalController.g.StartCoroutine(AtivadorDoCheckPoint.FillDates());

                
            }

            FindObjectOfType<CharacterManager>().transform.position = SaveDatesManager.s.CurrentSaveDate.Posicao;
            FindObjectOfType<Camera2D>().transform.position = SaveDatesManager.s.CurrentSaveDate.Posicao + new Vector3(0, 0, -10);
        }
    }

    static IEnumerator FillDates()
    {
        yield return new WaitForEndOfFrame();
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestToFillDates, SaveDatesManager.s.CurrentSaveDate));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.checkPointLoad));
    }

    // Start is called before the first frame update
    void Start()
    {
        SempreEstaNoTrigger();
    }
}
