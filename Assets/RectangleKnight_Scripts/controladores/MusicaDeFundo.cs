using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MusicaDeFundo
{
#pragma warning disable 0649
    [SerializeField] private AudioSource[] audios;
#pragma warning restore 0649
    private int inicia = -1;
    private int termina = -1;

    private string cenaIniciada = "";
    private bool parando;
    private float volumeAlvo = 0.5f;
    private float volumeBase = 0.75f;
    private const float VELOCIDADE_DE_MUDANCA = 0.25f;

    public MusicaComVolumeConfig MusicaGuardada { get; private set; }

    public MusicaComVolumeConfig MusicaAtualAtiva { get; private set; }

    public float VelocidadeAtiva { get; set; } = 0.25f;

    public float VolumeBase {
        get => volumeBase;
        set {
            float voltransitorio = volumeAlvo / volumeBase;
            volumeBase = value;
            volumeAlvo = volumeBase * voltransitorio;
            for (int i = 0; i<audios.Length; i++)
            {
                if(MusicaAtualAtiva!=null)
                    audios[i].volume = MusicaAtualAtiva.Volume * volumeBase;
            }
        }
    }

    public void ResetaVelAtiva()
    {
        VelocidadeAtiva = VELOCIDADE_DE_MUDANCA;
    }

    public void IniciarMusicaDaCena(DadosDeCena d)
    {
        //DadosDeCena d = SceneDates.GetCurrentSceneDates();

        if (d != null)
        {
            IniciarMusica(d.musicName);
        }
    }

    public void IniciarMusicaGuardada(float vel = -1)
    {
        if (vel <= 0)
            vel = VELOCIDADE_DE_MUDANCA;

        VelocidadeAtiva = vel;

        if (MusicaGuardada != null)
        {
            IniciarMusica(MusicaGuardada.Musica, MusicaGuardada.Volume);
        }
        else
            ReiniciarMusicas();
    }

    public void IniciarMusicaGuardandoAtual(AudioClip esseClip, float volumeAlvo = 1,float vel = -1)
    {
        MusicaGuardada = MusicaAtualAtiva;
        IniciarMusica(esseClip, volumeAlvo,vel);
    }

    public void IniciarMusicaGuardandoAtual(NameMusicaComVolumeConfig n,float vel = -1)
    {
        IniciarMusicaGuardandoAtual(n.Musica, n.Volume,vel);
    }

    public void IniciarMusicaGuardandoAtual(MusicaComVolumeConfig n, float vel = -1)
    {
        IniciarMusicaGuardandoAtual(n.Musica, n.Volume);
    }

    public void IniciarMusicaGuardandoAtual(NameMusic esseClip, float volumeAlvo = 1, float vel = -1)
    {
        IniciarMusicaGuardandoAtual(esseClip.ToString(), volumeAlvo); ;
    }

    public void IniciarMusicaGuardandoAtual(string esseClip, float volumeAlvo = 1, float vel = -1)
    {
        IniciarMusicaGuardandoAtual((AudioClip)Resources.Load(esseClip), volumeAlvo);
    }

    public void IniciarMusica(NameMusicaComVolumeConfig esseClip,  float vel = -1)
    {
        IniciarMusica((AudioClip)Resources.Load(esseClip.Musica.ToString()), esseClip.Volume, vel);
    }

    public void IniciarMusica(NameMusic esseClip, float volumeAlvo = 1,float vel = -1)
    {
        IniciarMusica((AudioClip)Resources.Load(esseClip.ToString()), volumeAlvo,vel);
    }

    public void IniciarMusica(AudioClip esseClip, float volumeAlvo = 1, float vel = -1)
    {
        if (vel <= 0)
        {
            vel = VELOCIDADE_DE_MUDANCA;
        }

        VelocidadeAtiva = vel;

        MusicaAtualAtiva = new MusicaComVolumeConfig()
        {
            Musica = esseClip,
            Volume = volumeAlvo
        };

        parando = false;
        this.volumeAlvo = volumeAlvo*VolumeBase;
        AudioSource au = audios[0];

        if (au.isPlaying)
        {
            termina = 0;
            inicia = 1;
        }
        else
        {
            termina = 1;
            inicia = 0;
        }

        if (audios[termina].clip == esseClip)
        {
            int temp = inicia;
            inicia = termina;
            termina = temp;
        }
        else
        {
            audios[inicia].volume = 0;
            audios[inicia].clip = esseClip;
            audios[inicia].Play();
        }
        
    }

    public void PararMusicas(float vel = -1)
    {
        if (vel <= 0)
            vel = VELOCIDADE_DE_MUDANCA;

        VelocidadeAtiva = vel;
        parando = true;
    }

    /*
    public void PararMusicas()
    {
        parando = true;
    }*/

    public void ReiniciarMusicas(bool doZero = false)
    {
        parando = false;

        if (doZero)
        {
            audios[inicia].Stop();
            audios[inicia].Play();
        }
    }

    public void Update()
    {
        //Debug.Log(audios.Length + " : " + parando);
        if (audios.Length > 0)
        {
            if (!parando)
            {
                if (inicia != -1 && termina != -1)
                {


                    if (audios[inicia].volume < 0.9f * volumeAlvo)
                        audios[inicia].volume = Mathf.Lerp(audios[inicia].volume, volumeAlvo, Time.fixedDeltaTime * VelocidadeAtiva);
                    else
                        audios[inicia].volume = volumeAlvo;

                    if (audios[termina].volume < 0.2f)
                    {
                        audios[termina].volume = 0;
                        audios[termina].Stop();
                    }
                    else
                        audios[termina].volume = Mathf.Lerp(audios[termina].volume, 0, Time.fixedDeltaTime * 3 * VelocidadeAtiva);

                }
            }
            else
            {
                if (termina != -1)
                    audios[termina].volume = Mathf.Lerp(audios[termina].volume, 0, Time.fixedDeltaTime * 2 * VelocidadeAtiva);

                if (inicia != -1)
                    audios[inicia].volume = Mathf.Lerp(audios[inicia].volume, 0, Time.fixedDeltaTime * 2 * VelocidadeAtiva);
            }


        }
    }

    void MudaPara(string clip, float volume = 1)
    {
        volumeAlvo = volume;
        IniciarMusica((AudioClip)Resources.Load(clip));
        cenaIniciada = SceneManager.GetActiveScene().name;
    }

    public void Start()
    {
        Debug.Log("Adicionar musicas iniciais");
        /*
        if (SceneManager.GetActiveScene().name == "Inicial 1")
            IniciarMusica((AudioClip)Resources.Load(NameMusic.Field2.ToString()));*/
    }
}

public enum NameMusic
{
    nula = -1,
    IntroTheme,//Theme2
    initialAdventureTheme,//DUngeon3
    acampamentoTheme,//Town4,
    TyronTheme,//Theme
    inCheckPointSound,
    trapMusic,//2003_Battle1
    miniBoss,//XP006Boss02
    XPboss3,
    HerikaTheme,//New_Year_s_Anthem_QuincasMoreira_youtubeAudioLIbarry
    TemaDoAquifero,//B->Marigold__QuincasMoreira_youtubeAudioLibrary
    XPBoss4
}

public enum SoundEffectID
{
    nulo = -1,
    Damage3,
    Decision1,
    Book1,
    Moeda,
    VariasMoedas,
    fakeWall,//XP050-Explosion03
    CheckPointSound,//137-Light03
    painelAbrindo,//Bell3
    addUpdateGeometry,//Push
    exitCheckPoint,//XP108-Heal04
    pedrasQuebrando,//XP129-Earth01
    rockFalseAttack,//XP097-Attack09
    somParaGetLosangulo,//pacote de audios positivos
    somParaEruptLosangulo,//pacote de audios positivos
    vinhetinhaDoSite,
    Wind1,
    lancaProjetilInimigo,//Absorb1
    EnemySlash,//100-Attack12
    ItemImportante, //ROGM2003_JEnd of Battle 3
    meuArbusto,
    Darkness4,//RPGMAKER2003
    Fire1,
    Break,
    avancoDoInimigo,//VXACE_Shot3
    aparicaoSurpresaDeInimigo,//VXACE_Wind6
    mordida,//XP089-Attack01 -> XP065-Swing04 -> XP052-Cannon01
    naCachoeirinha,//022-Dive02
    Fire3,// usado para update geometry complete
    XP010_System10,// usado para update geometry complete
    XP049_Explosion02,// usado para update geometry complete
    Shop//VXACE
}

[System.Serializable]
public class MusicaComVolumeConfig
{
    [SerializeField] private AudioClip musica;
    [SerializeField] private float volume = 1;

    public AudioClip Musica
    {
        get { return musica; }
        set { musica = value; }
    }

    public float Volume
    {
        get { return volume; }
        set { volume = value; }
    }
}

[System.Serializable]
public class NameMusicaComVolumeConfig
{
    [SerializeField] private NameMusic musica;
    [SerializeField] private float volume = 1;

    public NameMusic Musica
    {
        get { return musica; }
        set { musica = value; }
    }

    public float Volume
    {
        get { return volume; }
        set { volume = value; }
    }
}

