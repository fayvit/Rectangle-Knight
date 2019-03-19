using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PainelDeConfirmacao : MonoBehaviour
{
    public delegate void Confirmacao();
    public event Confirmacao botaoSim;
    public event Confirmacao botaoNao;

#pragma warning disable 0649
    [SerializeField] private Text textoDoBotaoSim;
    [SerializeField] private Text textoDoBotaoNao;
    [SerializeField] private Text textoDoPainel;
    [SerializeField] private Image seletorDoBotaoSim;
    [SerializeField] private Image seletorDoBotaoNao;
#pragma warning restore 0649
    private bool selectedYes = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int quanto = UiDeOpcoes.VerificaMudarOpcao(false);

        if (quanto != 0)
            ChangeSelectedOption();

        if (ActionManager.ButtonUp(0, GlobalController.g.Control))
        {
            Debug.Log("button 0");

            if (selectedYes)
                BotaoSim();
            else
                BotaoNao();
        }
        else
        if (ActionManager.ButtonUp(2, GlobalController.g.Control))
        {
            BotaoNao();
        }
    }

    void ChangeSelectedOption()
    {
        if (!selectedYes)
        {
            seletorDoBotaoNao.enabled = false;
            seletorDoBotaoSim.enabled = true;
        }
        else
        {
            seletorDoBotaoNao.enabled = true;
            seletorDoBotaoSim.enabled = false;
        }
        selectedYes = !selectedYes;
    }

    void AcaoDoBotaoSim()
    {
        //   if (ActionManager.ButtonUp(0, GlobalController.g.Control))
        {
            BotaoSim();
        }
    }

    public void AtivarPainelDeConfirmacao(Confirmacao sim, Confirmacao nao, string textoDoPainel,bool selectedYes = false)
    {
        
        gameObject.SetActive(true);
        botaoSim += sim;
        botaoNao += nao;
        seletorDoBotaoNao.enabled = !selectedYes;
        seletorDoBotaoSim.enabled = selectedYes;
        this.selectedYes = selectedYes;
        this.textoDoPainel.text = textoDoPainel;
    }

    public void AlteraTextoDoBotaoSim(string s)
    {
        textoDoBotaoSim.text = s;
    }

    public void AlteraTextoDoBotaoNao(string s)
    {
        textoDoBotaoNao.text = s;
    }

    public void AlteraTextoDoPainel(string s)
    {
        textoDoPainel.text = s;
    }

    public void AlteraTextos(string textoDoBotaoSim, string textoDoBotaoNao, string textoDoPainel)
    {
        this.textoDoPainel.text = textoDoPainel;
        this.textoDoBotaoNao.text = textoDoBotaoNao;
        this.textoDoBotaoSim.text = textoDoBotaoSim;
    }

    void LimpaBotoes()
    {
        botaoSim = null;
        botaoNao = null;
    }

    public void BotaoSim()
    {
        botaoSim();
        gameObject.SetActive(false);
        LimpaBotoes();
        EventAgregator.Publish(EventKey.positiveUiInput, null);
    }

    public void BotaoNao()
    {
        botaoNao();
        gameObject.SetActive(false);
        LimpaBotoes();
        EventAgregator.Publish(EventKey.negativeUiInput, null);
    }
}
