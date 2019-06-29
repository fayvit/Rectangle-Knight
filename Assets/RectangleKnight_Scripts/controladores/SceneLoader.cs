using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class SceneLoader : MonoBehaviour
{

    [SerializeField] private LoadBar loadBar;

    private SaveDates S;
    private AsyncOperation[] a2;
    private System.Action acaoFinalizadora;
    private FasesDoLoad fase = FasesDoLoad.emEspera;
    private bool podeIr = false;
    private float tempo = 0;
    private int indiceDoJogo = 0;    

    private int aSerCarregado = 0;
    private int numCarregador = 0;

    private const float tempoMin = 0.25f;

    private enum FasesDoLoad
    {
        emEspera,
        carregando,
        //escurecendo,
        //clareando,
        eventInProgress
    }

    public static void CarergarMenuPrincipal()
    {
        
        GlobalController.g.FadeV.IniciarFadeIn();
        SceneManager.LoadScene(NomesCenasEspeciais.menuInicial.ToString());
        GlobalController.g.EmTeste = false;
        Destroy(GameController.g.gameObject);
    }

    public static void IniciarCarregamento(int slote,System.Action acaoFinalizadora = null)
    {
        Physics2D.gravity = Vector2.zero;
        GameObject G = new GameObject();
        SceneLoader loadScene = G.AddComponent<SceneLoader>();


        loadScene.CenaDoCarregamento(slote,acaoFinalizadora);

    }

    public void CenaDoCarregamento(int indice, System.Action acaoFinalizadora = null)
    {

        DontDestroyOnLoad(gameObject);
        indiceDoJogo = indice;
        this.acaoFinalizadora = acaoFinalizadora;

        if (SceneManager.GetSceneByName(NomesCenasEspeciais.ComunsDeFase.ToString()).isLoaded)
        {
            AditiveLoadScene(NomesCenasEspeciais.CenaDeCarregamento);
            SceneManager.sceneLoaded += IniciarCarregamentoComComuns;
            //Camera.main.farClipPlane = 0.4f;
            Time.timeScale = 0;
        }
        else
        {

            AditiveLoadScene(NomesCenasEspeciais.CenaDeCarregamento);
            SceneManager.sceneLoaded += IniciarCarregamento;
        }
    }

    private void IniciarCarregamento(Scene arg0, LoadSceneMode arg1)
    {
        if (SceneManager.GetSceneByName(NomesCenasEspeciais.menuInicial.ToString()).isLoaded)
        {
            SceneManager.UnloadSceneAsync(NomesCenasEspeciais.menuInicial.ToString());
        }

        loadBar = FindObjectOfType<LoadBar>();

        AditiveLoadScene(NomesCenasEspeciais.ComunsDeFase);
        SceneManager.sceneLoaded -= IniciarCarregamento;
        SceneManager.sceneLoaded += CarregouComuns;
    }

    private void CarregouComuns(Scene arg0, LoadSceneMode arg1)
    {
        ComunsCarregado();
    }

    private void IniciarCarregamentoComComuns(Scene arg0, LoadSceneMode arg1)
    {

        SceneManager.sceneLoaded -= IniciarCarregamentoComComuns;
        loadBar = FindObjectOfType<LoadBar>();
        ComunsCarregado();
    }

    void ComunsCarregado()
    {
        if (ExistenciaDoController.AgendaExiste(ComunsCarregado, this))
        {
            SceneManager.sceneLoaded -= CarregouComuns;
            SceneManager.sceneLoaded += SetarCenaPrincipal;
            SceneManager.sceneLoaded += TatudoCarregado;

            if (SaveDatesManager.s.SavedGames.Count > indiceDoJogo)
                S = SaveDatesManager.s.SavedGames[indiceDoJogo];
            else
                S = null;

            if (S == null)
            {
                fase = FasesDoLoad.carregando;
                aSerCarregado = 1;
                a2 = new AsyncOperation[1];
                a2[0] = AditiveLoadScene(NomesCenas.TutoScene);
                
            }
            else
            {
                Debug.Log("contador do mapa: "+S.VariaveisChave.MapDates.Count);

                NomesCenas[] N2 = DescarregarCenasDesnecessarias(S.VariaveisChave.CenasAtivas.ToArray());

                numCarregador = 0;
                aSerCarregado = N2.Length;
                

                for (int i = 0; i < N2.Length; i++)
                {
                    SceneManager.UnloadSceneAsync(N2[i].ToString());
                }

                FuncaoCarregadora();
            }
        }
    }

    public static NomesCenas[] PegueAsCenasPorCarregar(NomesCenas[] N)
    {

        System.Collections.Generic.List<NomesCenas> retorno = new System.Collections.Generic.List<NomesCenas>();
        for (int i = 0; i < N.Length; i++)
        {
            //Debug.Log("nomes cenas por carregar: "+N[i]);
            if (!SceneManager.GetSceneByName(N[i].ToString()).isLoaded)
            {
                retorno.Add(N[i]);
            }
        }

        return retorno.ToArray();
    }

    void FuncaoCarregadora()
    {
        fase = FasesDoLoad.carregando;
        NomesCenas[] N = PegueAsCenasPorCarregar(S.VariaveisChave.CenasAtivas.ToArray());


        aSerCarregado = N.Length;

        a2 = new AsyncOperation[N.Length];
        for (int i = 0; i < N.Length; i++)
        {
            a2[i] = SceneManager.LoadSceneAsync(N[i].ToString(), LoadSceneMode.Additive);
            
        }



        if (N.Length == 0 || SceneManager.GetSceneByName(GameController.g.MyKeys.CenaAtiva.ToString()).isLoaded)
        {
            SetarCenaPrincipal(SceneManager.GetSceneByName(GameController.g.MyKeys.CenaAtiva.ToString()), LoadSceneMode.Single);
            TatudoCarregado();
        }
        Time.timeScale = 0;
    }

    void SetarCenaPrincipal(Scene scene, LoadSceneMode mode)
    {
        if (S != null)
        {
            Debug.Log(S.VariaveisChave.CenaAtiva.ToString() + " : " + scene.name);
            if (scene.name == S.VariaveisChave.CenaAtiva.ToString())
            {
                PedirAtualizacaoDeDados(scene);
            }
        }
        else if (scene.name == NomesCenas.TutoScene.ToString())
        {
            PedirAtualizacaoDeDados(scene);
        }
    }

    void PedirAtualizacaoDeDados(Scene scene)
    {
        InvocarSetScene(scene,S);
        SceneManager.sceneLoaded -= SetarCenaPrincipal;
       
        
        SaveDatesManager.s.IndiceDoJogoAtualSelecionado = indiceDoJogo;
    }
    

    static IEnumerator setarScene(Scene scene,SaveDates S)
    {
        yield return new WaitForSeconds(0.5f);
        InvocarSetScene(scene,S);
    }

    public static void InvocarSetScene(Scene scene,SaveDates S)
    {
        
        SceneManager.SetActiveScene(scene);


        if (SceneManager.GetActiveScene() != scene)
            GlobalController.g.StartCoroutine(setarScene(scene,S));
        else
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestToFillDates, S));//ComoPode();

    }

    private void TatudoCarregado(Scene arg0, LoadSceneMode arg1)
    {
        TatudoCarregado();
    }

    private void TatudoCarregado()
    {
        numCarregador++;
        if (aSerCarregado <= numCarregador)
        {
            podeIr = true;
            SceneManager.sceneLoaded -= TatudoCarregado;
        }
    }

    public static NomesCenas[] DescarregarCenasDesnecessarias(NomesCenas[] N)
    {
        System.Collections.Generic.List<NomesCenas> retorno = new System.Collections.Generic.List<NomesCenas>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene S = SceneManager.GetSceneAt(i);

            //Debug.Log("tentativa de string para enum: " + StringParaEnum.ObterEnum<NomesCenasEspeciais>(S.name));

            if (S.isLoaded && S.name != NomesCenasEspeciais.ComunsDeFase.ToString() 
                && S.name != NomesCenasEspeciais.CenaDeCarregamento.ToString())
            {
                if (S.isLoaded)
                {
                    bool foi = false;
                    for (int j = 0; j < N.Length; j++)
                    {
                        if (S.name == N[j].ToString())
                            foi = true;
                    }

                    if (!foi)
                        retorno.Add(StringParaEnum.ObterEnum<NomesCenas>(S.name));
                }
            }
        }

        return retorno.ToArray();
    }

    AsyncOperation AditiveLoadScene(NomesCenasEspeciais n)
    {
        return SceneManager.LoadSceneAsync(n.ToString(), LoadSceneMode.Additive);
    }

    AsyncOperation AditiveLoadScene(NomesCenas n)
    {
        return SceneManager.LoadSceneAsync(n.ToString(), LoadSceneMode.Additive);
    }

    public void Update()
    {
        switch (fase)
        {
            case FasesDoLoad.carregando:

                tempo += Time.fixedDeltaTime;

                float progresso = 0;
                if (a2.Length > 0)
                {
                    for (int i = 0; i < a2.Length; i++)
                    {
                        progresso += a2[i].progress;
                    }

                    progresso /= a2.Length;

                }
                else
                    progresso = 1;

                loadBar.ValorParaBarra(Mathf.Min(progresso, tempo / tempoMin, 1));

                if (podeIr && tempo >= tempoMin)
                {
                    GameObject go = GameObject.Find("EventSystem");
                    if (go)
                        SceneManager.MoveGameObjectToScene(go, SceneManager.GetSceneByName(NomesCenasEspeciais.ComunsDeFase.ToString()));

                    GlobalController.g.FadeV.IniciarFadeOut();
                    EventAgregator.AddListener(EventKey.fadeOutComplete, OnFadeOutComplete);
                    fase = FasesDoLoad.eventInProgress;

                }

            break;
        }
    }

    private void OnFadeOutComplete(IGameEvent obj)
    {
        GlobalController.g.FadeV.IniciarFadeIn();
        EventAgregator.AddListener(EventKey.fadeInComplete, OnFadeInComplete);

        SceneManager.SetActiveScene(
           SceneManager.GetSceneByName(GameController.g.MyKeys.CenaAtiva.ToString()));

        SaveDatesManager.SalvarAtualizandoDados();

        fase = FasesDoLoad.eventInProgress;

        SceneManager.UnloadSceneAsync(NomesCenasEspeciais.CenaDeCarregamento.ToString());

        //GameController.g.ModificacoesDaCena();
        Time.timeScale = 1;
        Physics2D.gravity = new Vector2(0, -0.8f);

    }

    private void OnFadeInComplete(IGameEvent obj)
    {
        Physics2D.gravity = new Vector2(0, -9.8f);
        Debug.Log("cena ativa para musica: " + SceneManager.GetActiveScene().name);
        
        Destroy(gameObject);

        InvokeAcaoFinalizadora();
    }

    void InvokeAcaoFinalizadora()
    {
        if (acaoFinalizadora != null)
        {
            acaoFinalizadora();
            acaoFinalizadora = null;
        }
    }
     

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.fadeInComplete, OnFadeInComplete);
        EventAgregator.RemoveListener(EventKey.fadeOutComplete, OnFadeOutComplete);
    }
}

public enum NomesCenas
{
    nula = 0,
    TutoScene,
    GreyScaleScene,
    FirstCheckPointZone,
    AcampamentoDosRejeitados,
    primeiroSubsolo,
    descidaDasProfundezas,
    zonaDosLosangulos,
    hexagonoDabarreiraAzul,
    area1ParaArea9,
    PassagemDosRefugiados,
    CheckPointNaGargantaDasProfundezas,
    obstaculosParaOsRefugiados,
    entroncamentoDosObstaculos,
    preparacaoParaSubida,
    grandeSubida,
    ExilioDasProfundezas,
    barracaDeHerikaNoExilio,
    capsulaDaGargantaDasProfundezas,
    preparacaoParaChefe,
    zonaDoChefe,
    armadilhasAzuis,
    barracaDeHerikaNoAcampamento,
    IntroAquifero,
    ApresentacaoInimigosAquifero,
    PreparacaoParaSubirAquifero,
    SubidaDoAquifero,
    checkPointDoAquifero,
    DesafioX_Shooter,
    DesafioDoInvestidorOculto,
    DesafioDoMordedorOculto,
    PrimeiraDescidaDosEspinhos,
    SegundoCheckPointDoAquifero,
    DesafioDoDefensorDoAquifero,
    SegundaDescidaComEspinhos,
    PreludioDaIntervencao,
    EntrocamentosDoAquifero,
    CapsulaDoAquifero,
    AbsolutamenteCapsulaDoAquifero,
    CorredeirasParaOChefe,
    ZonaDoChefeDoAquifero,
    posChefeSetaSombria,
    ConexaoDeCaminhosnoAquifero,
    DesafioDoCarapassaNadadora,
    DesafioDoCarapassaVoador,
    ItensPosDesafioDosCarapassa,
    EntroncamentoComAsGrandesCorredeiras,
    GrandesCorredeiras,
    GrandesCorredeiras_d,
    GrandesCorredeiras_b,
    GrandesCorredeiras_c,
    CaminhoDoAquiferoParaPontal,
    CaminhoDaGargantaParaPOntal
}

public enum NomesCenasEspeciais
{
    tentaivaDeMenos1 = -1,
    valorDefaultNoZero,
    ComunsDeFase,
    CenaDeCarregamento,
    menuInicial    
}