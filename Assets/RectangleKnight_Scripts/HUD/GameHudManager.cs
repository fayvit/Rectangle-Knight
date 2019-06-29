using System;
using UnityEngine;
using UnityEngine.UI;

public class GameHudManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Image lifeBar;
    [SerializeField] private Image magicBar;
    [SerializeField] private Image[] lifeBarUpdates;
    [SerializeField] private Image[] lifeBarAddlife;
    [SerializeField] private Image[] magicBarUpdates;
    [SerializeField] private Text txtDinheiro;

    private int maxBasicLifeBar = 100;
    private int maxBasicMagicBar = 50;
    private int addLifeBarAmount = 25;
    private int addMagicBarAmount = 10; 
#pragma warning restore 0649

    // Start is called before the first frame update
    void Start()
    {
        EventAgregator.AddListener(EventKey.changeLifePoints, OnChangeLifePoints);
        EventAgregator.AddListener(EventKey.changeMagicPoints, OnChangemagicPoints);
        EventAgregator.AddListener(EventKey.changeMoneyAmount, OnChangeMoneyAmount);
        EventAgregator.AddListener(EventKey.requestToFillDates, OnRequestFillDates);
        EventAgregator.AddListener(EventKey.starterHudForTest, OnStarterHudForTest);

    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.changeLifePoints, OnChangeLifePoints);
        EventAgregator.RemoveListener(EventKey.changeMagicPoints, OnChangemagicPoints);
        EventAgregator.RemoveListener(EventKey.changeMoneyAmount, OnChangeMoneyAmount);
        EventAgregator.RemoveListener(EventKey.requestToFillDates, OnRequestFillDates);
        EventAgregator.RemoveListener(EventKey.starterHudForTest, OnStarterHudForTest);
    }

    private void OnStarterHudForTest(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        SetarDadosIniciaisDaHud((DadosDoJogador)ssge.MyObject[0]);


        
    }

    void SetarDadosIniciaisDaHud(DadosDoJogador dados)
    {
        maxBasicLifeBar = dados.BaseMaxLife;
        maxBasicMagicBar = dados.BaseMaxMana;
        addLifeBarAmount = dados.AddLifeBarAmount;
        addMagicBarAmount = dados.AddMagicBarAmount;

        OnChangeVisibleAddLifeBar(new StandardSendGameEvent(EventKey.nulo, dados.HexagonosCompletados));//addlife precisa ser feito
        OnChangeVisibleUpdateLifeBar(new StandardSendGameEvent(EventKey.nulo, dados.HexagonosCompletados));
        OnChangeVisibleUpdateMagicBar(new StandardSendGameEvent(EventKey.nulo, dados.PentagonosCompletados));

        OnChangeMoneyAmount(new StandardSendGameEvent(EventKey.changeMoneyAmount, dados.Dinheiro));
        OnChangeLifePoints(new StandardSendGameEvent(EventKey.changeLifePoints, dados.PontosDeVida, dados.MaxVida));
        OnChangemagicPoints(new StandardSendGameEvent(EventKey.changeMagicPoints, dados.PontosDeMana, dados.MaxMana));
    }

    private void OnRequestFillDates(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        SaveDates S = (SaveDates)ssge.MyObject[0];
        DadosDoJogador dados;

        if (S == null)
        {
            dados = new DadosDoJogador();
            
        }
        else
        {
            dados = S.Dados;
        }

        SetarDadosIniciaisDaHud(dados);

    }

    private void OnChangeVisibleUpdateMagicBar(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;
        for (int i = 0; i < magicBarUpdates.Length; i++)
            if (i < (int)ssge.MyObject[0])
            {
                magicBarUpdates[i].transform.parent.gameObject.SetActive(true);
            }
            else
                magicBarUpdates[i].transform.parent.gameObject.SetActive(false);
    }

    private void OnChangeVisibleUpdateLifeBar(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;
        for (int i = 0; i < lifeBarUpdates.Length; i++)
            if (i < (int)ssge.MyObject[0])
            {
                lifeBarUpdates[i].transform.parent.gameObject.SetActive(true);
            }
            else
                lifeBarUpdates[i].transform.parent.gameObject.SetActive(false);
    }

    private void OnChangeVisibleAddLifeBar(IGameEvent obj)
    {
        for (int i = 0; i < lifeBarAddlife.Length; i++)
            lifeBarAddlife[i].transform.parent.gameObject.SetActive(false);
    }

    private void OnChangemagicPoints(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;

        
        magicBar.fillAmount = ((float)(int)ssge.MyObject[0]) / maxBasicMagicBar;
        
        PreencherBarrasDeUpdate(magicBarUpdates, addMagicBarAmount, maxBasicMagicBar, (int)ssge.MyObject[0], (int)ssge.MyObject[1]);

       
    }

    void PreencherBarrasDeUpdate(Image[] A,float val, float baseVal, float amount, float total)
    {
        for (int i = 0; i < A.Length; i++)
        {
            float max = baseVal+(i+1)*val;
            float min = baseVal + i * val;

           // if (total >= max + val * i)
                A[i].fillAmount = Mathf.Clamp((amount - min) / (max - min), 0, 1);
            //else
              //  A[i].gameObject.SetActive(false);
        }
    }

    private void OnChangeLifePoints(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;

        lifeBar.fillAmount = ((float)(int)ssge.MyObject[0]) / maxBasicLifeBar;

        
        PreencherBarrasDeUpdate(lifeBarUpdates, addLifeBarAmount, maxBasicLifeBar, (int)ssge.MyObject[0], (int)ssge.MyObject[1]);
        
    }

    private void OnChangeMoneyAmount(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;

        txtDinheiro.text = "x" + ((int)ssge.MyObject[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
