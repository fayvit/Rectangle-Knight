using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class LanguageSwitcher 
{
#pragma warning disable 0649
    [SerializeField] private LanguageMenu languageMenu;
#pragma warning restore 0649
    private EstadosDoSwitch estado = EstadosDoSwitch.emEspera;

    private enum EstadosDoSwitch
    {
        emEspera,
        menuSuspenso
    }

    // Use this for initialization
    public void Start()
    {
        BancoDeTextos.VerificaChavesFortes(idioma.pt_br, idioma.en_google);
        FuncaoDoBotao();
        //bandeirinha.sprite = languageMenu.BandeirinhaAtualSelecionada();
    }

    // Update is called once per frame
    public void Update()
    {
        switch (estado)
        {
            case EstadosDoSwitch.menuSuspenso:
                languageMenu.MudarOpcao();

                if (ActionManager.ButtonUp(0, GlobalController.g.Control))
                {
                    EventAgregator.Publish(EventKey.positiveUiInput, null);
                    
                    OpcaoEscolhida(languageMenu.OpcaoEscolhida);
                    estado = EstadosDoSwitch.emEspera;
                }
            break;
        }
    }

    void OpcaoEscolhida(int indice)
    {
        BancoDeTextos.linguaChave = languageMenu.IdiomaNoIndice(indice);
        //bandeirinha.sprite = languageMenu.BandeirinhaNoIndice(indice);
        languageMenu.FinalizarHud();
        //BtnsManager.ReligarBotoes(gameObject);

        InterfaceLanguageConverter[] ilc = MonoBehaviour.FindObjectsOfType<InterfaceLanguageConverter>();

        foreach (InterfaceLanguageConverter I in ilc)
        {
            I.MudaTexto();
        }

        SaveDatesManager.s.ChosenLanguage = BancoDeTextos.linguaChave;
        SaveDatesManager.Save();

        EventAgregator.Publish(EventKey.returnToMainMenu, null);
        /*
        InitialSceneManager.i.EstadoDeEscolhaInicial();
        InitialSceneManager.i.AtualizaLista();*/


        estado = EstadosDoSwitch.emEspera;
    }

    public void FuncaoDoBotao()
    {
        EventAgregator.Publish(EventKey.positiveUiInput, null);
        estado = EstadosDoSwitch.menuSuspenso;
        languageMenu.IniciarHud(OpcaoEscolhida);

        //InitialSceneManager.i.EstadoDePainelSuspenso();
        //BtnsManager.DesligarBotoes(gameObject);
    }
}
