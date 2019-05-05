using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspadaColetavel : AtivadorDeBotao
{
    [SerializeField] private PainelUmaMensagem painel = default;
    [SerializeField] private PainelUmaMensagem extraInfo = default;
    [SerializeField] private SwordColor corDaEspada = SwordColor.blue;


    private void Start()
    {
        SempreEstaNoTrigger();
    }

    public override void FuncaoDoBotao()
    {
        btn.SetActive(false);
        Coletou();
    }

    void Coletou()
    {
        Time.timeScale = 0;

        painel.ConstroiPainelUmaMensagem(OnCloseFirstPanel);


        EventAgregator.Publish(new StandardSendGameEvent(EventKey.getColorSword, SwordColor.blue));
        EventAgregator.Publish(EventKey.abriuPainelSuspenso, null);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, KeyShift.venceuCirculoImperfeito));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.painelAbrindo));
        //  EventAgregator.Publish(ePentagono ? EventKey.getPentagon : EventKey.getHexagon, null);

        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(GetComponent<Collider2D>());
    }

    void OnCloseFirstPanel()
    {

        if (corDaEspada==SwordColor.blue)
        {
            StartCoroutine(PainelAoFimDoQuadro());
        }
        else
        {
            OnCloseSecondPanel();
            //Destroy(gameObject);
        }

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.ItemImportante));
    }

    IEnumerator PainelAoFimDoQuadro()
    {
        yield return new WaitForEndOfFrame();
        extraInfo.ConstroiPainelUmaMensagem(OnCloseSecondPanel);
    }

    void OnCloseSecondPanel()
    {
        Time.timeScale = 1;
        EventAgregator.Publish(EventKey.fechouPainelSuspenso);
        Destroy(gameObject);
    }
    
}
