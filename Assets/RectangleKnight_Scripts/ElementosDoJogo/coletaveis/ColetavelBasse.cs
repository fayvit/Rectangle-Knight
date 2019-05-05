using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColetavelBasse : AtivadorDeBotao
{
    #region inspector
    [SerializeField] private PainelPentagonoHexagono painel = default;
    //[SerializeField] private GameObject particulaDaAcao;
    [SerializeField] private string ID;
    #endregion

    protected abstract void AcaoEspecificaDaColeta();

    private void Start()
    {
        SempreEstaNoTrigger();

        if (ExistenciaDoController.AgendaExiste(Start, this))
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.destroyShiftCheck, ID, gameObject));
        }
    }

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    void Coletou()
    {
        Time.timeScale = 0;
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, ID));
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(GetComponent<Collider2D>());

        AcaoEspecificaDaColeta();
        
        painel.ConstroiPainelDosPentagonosOuHexagonos(OnClosePanel,
            PainelPentagonoHexagono.Forma.naoForma);

        EventAgregator.Publish(EventKey.abriuPainelSuspenso, null);
        

        
    }

    void OnClosePanel()
    {
        Time.timeScale = 1;
        EventAgregator.Publish(EventKey.fechouPainelSuspenso);
        Destroy(gameObject);
    }

    public override void FuncaoDoBotao()
    {
        btn.SetActive(false);
        Coletou();
    }



}
