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
    private const float VELOCIDADE_DE_MUDANCA = 0.25f;

    public MusicaComVolumeConfig MusicaGuardada { get; private set; }

    public MusicaComVolumeConfig MusicaAtualAtiva { get; private set; }

    public float VolumeBase { get; set; } = 0.95f;

    public float VelocidadeAtiva { get; set; } = 0.25f;

    public void ResetaVelAtiva()
    {
        VelocidadeAtiva = VELOCIDADE_DE_MUDANCA;
    }



    public void IniciarMusicaGuardada()
    {
        if (MusicaGuardada != null)
        {
            IniciarMusica(MusicaGuardada.Musica, MusicaGuardada.Volume);
        }
    }

    public void IniciarMusicaGuardandoAtual(AudioClip esseClip, float volumeAlvo = 1)
    {
        MusicaGuardada = MusicaAtualAtiva;
        IniciarMusica(esseClip, volumeAlvo);
    }

    public void IniciarMusicaGuardandoAtual(NameMusic esseClip, float volumeAlvo = 1)
    {
        IniciarMusicaGuardandoAtual(esseClip.ToString(), volumeAlvo); ;
    }

    public void IniciarMusicaGuardandoAtual(string esseClip, float volumeAlvo = 1)
    {
        IniciarMusicaGuardandoAtual((AudioClip)Resources.Load(esseClip), volumeAlvo);
    }

    public void IniciarMusica(NameMusic esseClip, float volumeAlvo = 1)
    {
        IniciarMusica((AudioClip)Resources.Load(esseClip.ToString()), volumeAlvo);
    }

    public void IniciarMusica(AudioClip esseClip, float volumeAlvo = 1)
    {

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

    public void PararMusicas(float vel)
    {
        parando = true;
    }

    public void PararMusicas()
    {
        parando = true;
    }

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
                        audios[inicia].volume = Mathf.Lerp(audios[inicia].volume, volumeAlvo, Time.deltaTime * VelocidadeAtiva);
                    else
                        audios[inicia].volume = volumeAlvo;

                    if (audios[termina].volume < 0.2f)
                    {
                        audios[termina].volume = 0;
                        audios[termina].Stop();
                    }
                    else
                        audios[termina].volume = Mathf.Lerp(audios[termina].volume, 0, Time.deltaTime * 3 * VelocidadeAtiva);

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
    Field2,
    Mushrooms,
    Battle8,
    Theme5
}

public enum SoundEffectID
{
    tuin_1ponto3,
    tuimParaNivel,
    XP_Heal01,
    coisaBoaRebot,
    XP_Swing03,
    rajadaDeAgua,
    Book1,
    paraBau,
    Collapse1,
    chamadaParaAcao,
    XP_Knock04,
    XP_Knock01,
    bemFeito,
    Decision1,
    XP_Heal02,
    Item,
    encontro,
    VinhetaDoEncontro,
    Evasion1,
    Slash2,
    Shot3,
    Slash1,
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

