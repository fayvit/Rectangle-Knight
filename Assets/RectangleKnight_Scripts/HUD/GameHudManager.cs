using UnityEngine;
using UnityEngine.UI;

public class GameHudManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Image lifeBar;
    [SerializeField] private Image magicBar;
    [SerializeField] private Text txtDinheiro;
#pragma warning restore 0649
    // Start is called before the first frame update
    void Start()
    {
        EventAgregator.AddListener(EventKey.changeLifePoints, OnChangeLifePoints);
        EventAgregator.AddListener(EventKey.changeMagicPoints, OnChangemagicPoints);
        EventAgregator.AddListener(EventKey.changeMoneyAmount, OnChangeMoneyAmount);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.changeLifePoints, OnChangeLifePoints);
        EventAgregator.RemoveListener(EventKey.changeMagicPoints, OnChangemagicPoints);
        EventAgregator.RemoveListener(EventKey.changeMoneyAmount, OnChangeMoneyAmount);
    }

    private void OnChangemagicPoints(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;

        magicBar.fillAmount = ((float)(int)ssge.MyObject[0]) / (int)ssge.MyObject[1];
    }

    private void OnChangeLifePoints(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;

        lifeBar.fillAmount = ((float)(int)ssge.MyObject[0]) / (int)ssge.MyObject[1];
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
