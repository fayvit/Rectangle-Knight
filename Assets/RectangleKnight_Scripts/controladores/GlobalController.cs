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

    public PainelDeConfirmacao Confirmacao { get => confirmacao;}

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
    }
}
