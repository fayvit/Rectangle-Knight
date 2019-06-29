using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDeOpcoes : MonoBehaviour
{
#pragma warning disable 0649
    
    [SerializeField] private Slider sMusica;
    [SerializeField] private Slider sEfeitos;
    [SerializeField] private Text txtMusica;
    [SerializeField] private Text txtEfeitos;
    [SerializeField] private Image[] destaques;
#pragma warning restore 0649

    private int opcaoSelecionada = 0;
    private const float velDaModificacao = 0.15f;
    private bool podeMudar = true;

    // Start is called before the first frame update
    void OnEnable()
    {
        opcaoSelecionada = 0;

        sMusica.value = GlobalController.g.VolumeDaMusica;
        sEfeitos.value = GlobalController.g.VolumeDosEfeitos;

        txtMusica.text = Mathf.RoundToInt(sMusica.value * 100) + "%";
        txtEfeitos.text = Mathf.RoundToInt(sEfeitos.value * 100) + "%";

        ColocarDestaqueSelecionado();

        podeMudar = true;
    }

    void ColocarDestaqueSelecionado()
    {
        for (int i = 0; i < destaques.Length; i++)
        {
            destaques[i].enabled = false;
        }

        if(GlobalController.g.Control!=Controlador.Android)
            destaques[opcaoSelecionada].enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (podeMudar)
        {
            float quanto = CommandReader.GetAxis("horizontal", GlobalController.g.Control)
                + CommandReader.GetAxis("HDpad", GlobalController.g.Control);

            if (quanto > 0)
            {
                VerificaModificacao(true);
            }
            else if (quanto < 0)
            {
                VerificaModificacao(false);
            }
            else
            {

                quanto = UiDeOpcoes.VerificaMudarOpcao(true);

                if (quanto > 0)
                    opcaoSelecionada = ContadorCiclico.AlteraContador(1, opcaoSelecionada, destaques.Length);
                else if (quanto < 0)
                    opcaoSelecionada = ContadorCiclico.AlteraContador(-1, opcaoSelecionada, destaques.Length);

                if (quanto != 0)
                    ColocarDestaqueSelecionado();
            }

            bool foi = ActionManager.ButtonUp(0, GlobalController.g.Control);

            if (CommandReader.ButtonDown(2, GlobalController.g.Control)
                ||
                ( foi && opcaoSelecionada == destaques.Length - 1))
            {
                BotaoVoltar();
            }else

            if (foi && opcaoSelecionada == 2)
            {
                BotaoTodasHabilidades();
            }
        }


    }

    void VerificaModificacao(bool positivo)
    {
        switch (opcaoSelecionada)
        {
            case 0:
                sMusica.value += velDaModificacao * (positivo ? 1 : -1) * Time.fixedDeltaTime;
            break;
            case 1:
                sEfeitos.value += velDaModificacao * (positivo ? 1 : -1) * Time.fixedDeltaTime;
            break;
        }
    }

    public void OnChangeMusicSlider()
    {
        txtMusica.text = Mathf.RoundToInt(sMusica.value * 100) + "%";
        GlobalController.g.VolumeDaMusica = sMusica.value;
    }

    public void OnChangeEfectsSlider()
    {
        txtEfeitos.text = Mathf.RoundToInt(sEfeitos.value * 100) + "%";
        GlobalController.g.VolumeDosEfeitos = sEfeitos.value;
    }

    public void BotaoVoltar()
    {
        EventAgregator.Publish(EventKey.returnToMainMenu, null);
        gameObject.SetActive(false);
    }

    public void BotaoTodasHabilidades()
    {
        Debug.Log("habilidade");
        podeMudar = false;
        GlobalController.g.Confirmacao.AtivarPainelDeConfirmacao(LigarTodasHabilidades, NaoLigarTodasHabilidades,
            "Gostaria de ligar todas as habilidades? Isso desabilitará o salvamento do progresso");
    }

    void LigarTodasHabilidades()
    {
        podeMudar = true;
        EventAgregator.Publish(EventKey.allAbilityOn);
    }

    void NaoLigarTodasHabilidades()
    {
        podeMudar = true;

    }
}
