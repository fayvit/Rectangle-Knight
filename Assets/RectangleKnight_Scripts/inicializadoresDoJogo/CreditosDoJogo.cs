using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class CreditosDoJogo : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Scrollbar sb;
#pragma warning restore 0649
    private float velDoScroll = 1f;
    public void Start()
    {
        gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        float quanto = CommandReader.GetAxis("vertical",GlobalController.g.Control);

        if (quanto > 0)
            sb.value += velDoScroll*Time.deltaTime;
        else if (quanto < 0)
            sb.value -= velDoScroll * Time.deltaTime;

        if (CommandReader.ButtonDown(2, GlobalController.g.Control))
            SairDosCreditos();
    }

    public void SairDosCreditos()
    {
        gameObject.SetActive(false);
        EventAgregator.Publish(EventKey.returnToMainMenu, null);
    }

    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }
    
}
