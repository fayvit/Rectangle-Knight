using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public static GlobalController g;

#pragma warning disable 0649

    [SerializeField] private SoundEffectsManager sfx;
    [SerializeField] private MusicaDeFundo musica;
    [SerializeField] private MyFadeView fadeV;
    [SerializeField] private PainelDeConfirmacao confirmacao;
    [SerializeField] private PainelUmaMensagem umaMensagem;
    [SerializeField] private Controlador control = Controlador.Android;
    [SerializeField] private ContainerDeDadosDeCena sceneDates;
    [SerializeField] private bool emTeste = true;

#pragma warning restore 0649

    private List<AudioSource> ativos = new List<AudioSource>();

    public Controlador Control { get => control; }
    public PainelDeConfirmacao Confirmacao { get => confirmacao; }
    public PainelUmaMensagem UmaMensagem { get => umaMensagem; }
    public MyFadeView FadeV { get => fadeV; }
    public float VolumeDaMusica { get => musica.VolumeBase; set { musica.VolumeBase = value; } }
    public float VolumeDosEfeitos { get => sfx.VolumeBase; set { sfx.VolumeBase = value; } }
    public bool EmTeste { get => emTeste; set => emTeste = value; }
    public ContainerDeDadosDeCena SceneDates { get => sceneDates; private set => sceneDates = value; }

    void Awake()
    {
        GlobalController[] g = FindObjectsOfType<GlobalController>();

        if (g.Length > 1)
            Destroy(gameObject);

        transform.parent = null;
        DontDestroyOnLoad(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        g = this;

        if (emTeste)
            Debug.Log("está em teste");

        musica.IniciarMusicaDaCena(sceneDates.GetCurrentSceneDates());

        EventAgregator.AddListener(EventKey.disparaSom, OnRequestSoundEffects);
        EventAgregator.AddListener(EventKey.startMusic, OnRequestStartMusic);
        EventAgregator.AddListener(EventKey.stopMusic, OnRequestStopMusic);
        EventAgregator.AddListener(EventKey.restartMusic, OnRequestRestartMusic);
        EventAgregator.AddListener(EventKey.changeActiveScene, OnChangeActiveScene);
        EventAgregator.AddListener(EventKey.changeMusicWithRecovery, OnRequestMusicWithRecovery);
        EventAgregator.AddListener(EventKey.returnRememberedMusic, OnRequestRememberedMusic);
        EventAgregator.AddListener(EventKey.startCheckPoint, OnStartCheckPoint);
        EventAgregator.AddListener(EventKey.requestToFillDates, OnChangeActiveScene);
        EventAgregator.AddListener(EventKey.checkPointLoad, OnCheckPointLoad);
        EventAgregator.AddListener(EventKey.checkPointExit, OnCheckPointExit);
        EventAgregator.AddListener(EventKey.getUpdateGeometry, OnGetUpdateGeometry);
        EventAgregator.AddListener(EventKey.request3dSound, OnRequest3dSound);

    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.disparaSom, OnRequestSoundEffects);
        EventAgregator.RemoveListener(EventKey.startMusic, OnRequestStartMusic);
        EventAgregator.RemoveListener(EventKey.stopMusic, OnRequestStopMusic);
        EventAgregator.RemoveListener(EventKey.restartMusic, OnRequestRestartMusic);
        EventAgregator.RemoveListener(EventKey.changeActiveScene, OnChangeActiveScene);
        EventAgregator.RemoveListener(EventKey.changeMusicWithRecovery, OnRequestMusicWithRecovery);
        EventAgregator.RemoveListener(EventKey.returnRememberedMusic, OnRequestRememberedMusic);
        EventAgregator.RemoveListener(EventKey.startCheckPoint, OnStartCheckPoint);
        EventAgregator.RemoveListener(EventKey.requestToFillDates, OnChangeActiveScene);
        EventAgregator.RemoveListener(EventKey.checkPointLoad, OnCheckPointLoad);
        EventAgregator.RemoveListener(EventKey.checkPointExit, OnChangeActiveScene);
        EventAgregator.RemoveListener(EventKey.getUpdateGeometry, OnGetUpdateGeometry);
        EventAgregator.RemoveListener(EventKey.checkPointExit, OnCheckPointExit);
        EventAgregator.RemoveListener(EventKey.request3dSound, OnRequest3dSound);
    }

    private void OnRequest3dSound(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        sfx.Instantiate3dSound(e.Sender.transform.position,(SoundEffectID)ssge.MyObject[0],(float)ssge.MyObject[1]);
    }

    private void OnCheckPointExit(IGameEvent e)
    {
        OnChangeActiveScene(e);

        sfx.DisparaAudio(SoundEffectID.exitCheckPoint);
    }

    private void OnGetUpdateGeometry(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;
        HexagonoColetavel.DadosDaGeometriaColetavel d = (HexagonoColetavel.DadosDaGeometriaColetavel)ssge.MyObject[0];

        musica.PararMusicas(d.velocidadeNaQuedaDaMusica);
        sfx.DisparaAudio(SoundEffectID.painelAbrindo);

    }

    void OnCheckPointLoad(IGameEvent e)
    {
        musica.IniciarMusica(NameMusic.inCheckPointSound);
    }

    void OnStartCheckPoint(IGameEvent e)
    {
        musica.PararMusicas(3);
        sfx.DisparaAudio(SoundEffectID.CheckPointSound.ToString());

    }

    void OnRequestRememberedMusic(IGameEvent e)
    {
        musica.IniciarMusicaGuardada();
    }

    void OnRequestMusicWithRecovery(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        if(ssge.MyObject.Length>1)
            musica.IniciarMusicaGuardandoAtual((NameMusicaComVolumeConfig)ssge.MyObject[0],(float)ssge.MyObject[1]);
        else
            musica.IniciarMusicaGuardandoAtual((NameMusicaComVolumeConfig)ssge.MyObject[0]);
    }

    void OnChangeActiveScene(IGameEvent e)
    {
        DadosDeCena d = sceneDates.GetCurrentSceneDates();
        bool foi = true;

        //Debug.Log(d + " : " + musica + " : " + musica.MusicaAtualAtiva + " : " + d.musicName+" : "+d.musicName.Musica);
        if (musica.MusicaAtualAtiva != null)
            if (musica.MusicaAtualAtiva.Musica.name == d.musicName.Musica.ToString())
            {
                foi = false;
            }

        if (foi)
            musica.IniciarMusica(d.musicName.Musica, d.musicName.Volume);
    }

    void OnRequestRestartMusic(IGameEvent e)
    {
        musica.ReiniciarMusicas();
    }

    void OnRequestStopMusic(IGameEvent e)
    {
        float vel = 0.25f;

        if (e is StandardSendGameEvent)
        {
            StandardSendGameEvent ssge = (StandardSendGameEvent)e;

            if (ssge.MyObject.Length > 0)
                vel = (float)ssge.MyObject[0];
        }
        /*
        try
        {
            StandardSendGameEvent ssge = (StandardSendGameEvent)e;

            if (ssge.MyObject.Length > 0)
                vel = (float)ssge.MyObject[0];
        }
        catch
        {

        }*/
        musica.PararMusicas(vel);
    }

    void OnRequestStartMusic(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        float vol = 1;

        if (ssge.MyObject.Length >= 2)
            vol = (float)ssge.MyObject[1];

        try
        {
            NameMusic n = (NameMusic)ssge.MyObject[0];
            musica.IniciarMusica(n, vol);
        }
        catch {
            AudioClip a = (AudioClip)ssge.MyObject[0];
            musica.IniciarMusica(a, vol);
        }
    }

    void OnRequestSoundEffects(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        if (ssge.MyObject[0] is AudioClip)
        {
            AudioClip a = (AudioClip)ssge.MyObject[0];
            sfx.DisparaAudio(a);
        }
        else if (ssge.MyObject[0] is string)
        {
            string s = (string)ssge.MyObject[0];
            sfx.DisparaAudio(s);
        }
        else
        if (ssge.MyObject[0] is SoundEffectID)
        {
            string s = ((SoundEffectID)ssge.MyObject[0]).ToString();
            sfx.DisparaAudio(s);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        #region changeController
        if (Input.touchCount > 0)
            control = Controlador.Android;
        if (Input.GetKeyDown(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.Space))
            control = Controlador.teclado;
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
            control = Controlador.joystick1;
        #endregion

        musica.Update();
    }
}
