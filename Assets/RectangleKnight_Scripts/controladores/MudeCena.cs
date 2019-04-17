using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MudeCena : MonoBehaviour
{
    [SerializeField] private Vector3 posAlvo=default;
    [SerializeField] private NomesCenas[] cenasAlvo = default;
    [SerializeField] private NomesCenas cenaAtiva = NomesCenas.nula;
    

    //private int numCenasParaCarregar;
    //private int contCenasCaregadas = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    void OnFadeOutComplete()
    {
        Debug.Log("terminou fade out");

        Time.timeScale = 0;
        contCenasCaregadas = 0;

        NomesCenas[] N = SceneLoader.DescarregarCenasDesnecessarias(cenasAlvo);

        for (int i = 0; i < N.Length; i++)
        {
            SceneManager.UnloadSceneAsync(N[i].ToString());
        }

        N = SceneLoader.PegueAsCenasPorCarregar(cenasAlvo);

        numCenasParaCarregar = N.Length;

        for (int i = 0; i < N.Length; i++)
        {
            SceneManager.LoadScene(N[i].ToString(),LoadSceneMode.Additive);
        }

        Debug.Log("num: "+numCenasParaCarregar);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        contCenasCaregadas++;
        Debug.Log(contCenasCaregadas);

        if (contCenasCaregadas >= numCenasParaCarregar)
        {

            SceneManager.sceneLoaded -= OnSceneLoaded;

            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            if (cenaAtiva != NomesCenas.nula)
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(cenaAtiva.ToString()));
            else
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(cenasAlvo[0].ToString()));

            
        }
    }

    private void OnActiveSceneChanged(Scene arg0, Scene arg1)
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        Time.timeScale = 1;

        GameController.g.Manager.transform.position = posAlvo;
        GameController.g.VerifiqueDinheiroCaido(GameController.g.Manager.Dados.DinheiroCaido);
        GlobalController.g.FadeV.IniciarFadeInComAction(OnFadeInComplete);
        FindObjectOfType<Camera2D>().AposMudarDeCena(posAlvo + new Vector3(0, 0, -10));
    }

    void OnFadeInComplete()
    {

    }*/

    void OnFadeOutComplete()
    {
        StaticMudeCena.OnFadeOutComplete(cenasAlvo,cenaAtiva,posAlvo);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
                GlobalController.g.FadeV.IniciarFadeOutComAction(OnFadeOutComplete, 0.5f);               
            
        }
    }
}

public static class UnicidadeDoPlayer
{
    public static bool Verifique(Collider2D collision)
    {
        CapsuleCollider2D cc2d = null;

        try { cc2d = (CapsuleCollider2D)collision; }
        catch { }

        if (cc2d != null)
        {
            return true;
        }

        return false;
    }
}

public static class StaticMudeCena
{
    private static int contCenasCaregadas= 0;
    private static int numCenasParaCarregar = 0;
    private static NomesCenas cenaAtiva = NomesCenas.nula;
    private static Vector3 posAlvo;

    public static bool EstaCenaEstaCarregada(NomesCenas nome)
    {
        return SceneManager.GetSceneByName(nome.ToString()).isLoaded;
    }

    public static void OnFadeOutComplete(NomesCenas[] cenasAlvo,NomesCenas estaCenaAtiva,Vector3 pos)
    {
        Debug.Log("iniciaou");

        posAlvo = pos;

        if (estaCenaAtiva != NomesCenas.nula)
            cenaAtiva = estaCenaAtiva;
        else
            cenaAtiva = cenasAlvo[0];

        Time.timeScale = 0;
        contCenasCaregadas = 0;

        NomesCenas[] N = SceneLoader.DescarregarCenasDesnecessarias(cenasAlvo);

        for (int i = 0; i < N.Length; i++)
        {
            SceneManager.UnloadSceneAsync(N[i].ToString());
        }

        N = SceneLoader.PegueAsCenasPorCarregar(cenasAlvo);

        numCenasParaCarregar = N.Length;

        for (int i = 0; i < N.Length; i++)
        {
            SceneManager.LoadScene(N[i].ToString(), LoadSceneMode.Additive);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        contCenasCaregadas++;
        //Debug.Log(contCenasCaregadas+" : "+numCenasParaCarregar);

        if (contCenasCaregadas >= numCenasParaCarregar)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(cenaAtiva.ToString()));

                /*
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(cenaAtiva.ToString()));
            else
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(cenasAlvo[0].ToString()));
                */

        }
    }

    private static void OnActiveSceneChanged(Scene arg0, Scene arg1)
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        Time.timeScale = 1;

        GameController.g.Manager.transform.position = posAlvo;
        GameController.g.VerifiqueDinheiroCaido(GameController.g.Manager.Dados.DinheiroCaido);
        GlobalController.g.FadeV.IniciarFadeInComAction(OnFadeInComplete);
        MonoBehaviour.FindObjectOfType<Camera2D>().AposMudarDeCena(posAlvo + new Vector3(0, 0, -10));
        EventAgregator.Publish(EventKey.changeActiveScene, null);
    }

    static void OnFadeInComplete()
    {

    }
}
