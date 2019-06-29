using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class UiDeOpcoes
{
    [SerializeField] protected GameObject itemDoContainer;
    [SerializeField] protected RectTransform painelDeTamanhoVariavel;
    [SerializeField] protected ScrollRect sr;

    public abstract void SetarComponenteAdaptavel(GameObject G, int indice);
    protected abstract void FinalizarEspecifico();

    private float contadorDeTempo = 0;
    private const float TEMPO_DE_SCROLL = .15F;

    public int OpcaoEscolhida { get; private set; } = 0;

    public bool EstaAtivo
    {
        get { return painelDeTamanhoVariavel.parent.parent.gameObject.activeSelf; }
    }

    protected void IniciarHUD(int quantidade,TipoDeRedimensionamento tipo = TipoDeRedimensionamento.vertical)
    {
        OpcaoEscolhida = 0;
        painelDeTamanhoVariavel.parent.parent.gameObject.SetActive(true);

        itemDoContainer.SetActive(true);

        if(tipo==TipoDeRedimensionamento.vertical)
            RedimensionarUI.NaVertical(painelDeTamanhoVariavel, itemDoContainer, quantidade);
        else if(tipo==TipoDeRedimensionamento.emGrade)
            RedimensionarUI.EmGrade(painelDeTamanhoVariavel, itemDoContainer, quantidade);
        else if(tipo==TipoDeRedimensionamento.horizontal)
            RedimensionarUI.NaHorizontal(painelDeTamanhoVariavel, itemDoContainer, quantidade);

        for (int i = 0; i < quantidade; i++)
        {
            GameObject G = ParentearNaHUD.Parentear(itemDoContainer, painelDeTamanhoVariavel);
            SetarComponenteAdaptavel(G,i);

            G.name += i.ToString();

            if (i == OpcaoEscolhida)
            {
                ColocarDestaqueNoSelecionado(G.GetComponent<UmaOpcao>());
                /*if (GameController.g != null)
                  //  G.GetComponent<UmaOpcao>().SpriteDoItem.sprite = GameController.g.El.UiDestaque;
                //else
                {
                    Color C;
                    ColorUtility.TryParseHtmlString("#FFFFFFFF", out C);
                    G.GetComponent<UmaOpcao>().SpriteDoItem.color = C;
                }*/
                        

            }
        }

        itemDoContainer.SetActive(false);

        if (sr != null)
            if (sr.verticalScrollbar)
                sr.verticalScrollbar.value = 1;

        if (sr != null)
            if (sr.horizontalScrollbar)
                sr.horizontalScrollbar.value = 0;
        AgendaScrollPos();

    }

    void AgendaScrollPos()
    {
        if (GlobalController.g)
            GlobalController.g.StartCoroutine(ScrollPos());
        
    }

    void VerificarDestaque(UmaOpcao[] umaS)
    {
        for (int i = 0; i < umaS.Length; i++)
        {
            if (i == OpcaoEscolhida)
            {
                ColocarDestaqueNoSelecionado(umaS[i]);
            }
            else
            {
                RetirarDestaqueDoSelecionado(umaS[i]);
            }
        }
    }

    public void MudarSelecaoParaEspecifico(int qual)
    {
        UmaOpcao[] umaS = painelDeTamanhoVariavel.GetComponentsInChildren<UmaOpcao>();
        RetirarTodosOsDestaques(umaS);
        SelecionarOpcaoEspecifica(qual);
    }

    public void RetirarTodosOsDestaques(UmaOpcao[] umaS)
    {
        for (int i = 0; i < umaS.Length; i++)
        {
            RetirarDestaqueDoSelecionado(umaS[i]);
        }
    }

    public virtual void RetirarDestaqueDoSelecionado(UmaOpcao uma)
    {
        uma.SpriteDoItem.color = new Color(1, 1, 1, 0);
    }

    public virtual void ColocarDestaqueNoSelecionado(UmaOpcao uma)
    {
        Color C;
        ColorUtility.TryParseHtmlString("#ffffffFF", out C);
        uma.SpriteDoItem.color = C;
    }

    public void MudarOpcaoComVal(int quanto,int rowCellCount = -1)
    {
        if (quanto != 0)
        {
            UmaOpcao[] umaS = painelDeTamanhoVariavel.GetComponentsInChildren<UmaOpcao>();

            if (quanto > 0)
            {
                if (OpcaoEscolhida + quanto < umaS.Length)
                    OpcaoEscolhida += quanto;
                else
                    OpcaoEscolhida = 0;
            }
            else if (quanto < 0)
            {
                if (OpcaoEscolhida + quanto >= 0)
                    OpcaoEscolhida += quanto;
                else
                    OpcaoEscolhida = umaS.Length - 1;
            }

            VerificarDestaque(umaS);

            if (sr != null)
                if (sr.verticalScrollbar || sr.horizontalScrollbar)
                {
                    
                    AjeitaScroll(umaS,rowCellCount);
                }
                else
                {
                    Debug.Log("erro scroll 2");
                }

            else
                Debug.Log("erro no scrool");

            EventAgregator.Publish(EventKey.UiDeOpcoesChange,null);
        }
    }

    public static int VerificaMudarOpcao(bool vertical = true)
    {
        int quanto = 0;
        if (vertical)
        {
            quanto = -CommandReader.ValorDeGatilhos("VDpad", 1);

            if (quanto == 0)
                quanto = -CommandReader.ValorDeGatilhos("vertical", -1);

        }
        else
        {
            quanto = CommandReader.ValorDeGatilhos("HDpad", 1);

            if (quanto == 0)
                quanto = -CommandReader.ValorDeGatilhos("horizontal", -1);

        }

        return quanto;
    }

    public virtual void MudarOpcao()
    {
        MudarOpcaoComVal(VerificaMudarOpcao());
    }

    public virtual void MudarOpcao_H(bool negativar = false)
    {

        MudarOpcaoComVal((negativar?-1:1)*VerificaMudarOpcao(false));
    }

    void AjeitaScroll(UmaOpcao[] umaS,int rowCellCount)
    {
        contadorDeTempo = 0;
        if(GlobalController.g)
            GlobalController.g.StartCoroutine(MovendoScroll(umaS,rowCellCount));
        
    }

    public void SelecionarOpcaoEspecifica(int qual)
    {
        if (painelDeTamanhoVariavel.childCount > qual + 1)
        {
            OpcaoEscolhida = qual;
            UmaOpcao uma = painelDeTamanhoVariavel.GetChild(qual + 1).GetComponent<UmaOpcao>();
            ColocarDestaqueNoSelecionado(uma);
        }
    }

    protected virtual IEnumerator MovendoScroll(UmaOpcao[] umaS, int rowCellCount)
    {
      
        yield return new WaitForSecondsRealtime(0.01f);
        yield return new WaitForEndOfFrame();

        int val = (rowCellCount==-1)?umaS.Length: Mathf.CeilToInt((float)umaS.Length / rowCellCount);
        int opc = OpcaoEscolhida /( (rowCellCount==-1)?1:rowCellCount);
        
        contadorDeTempo += 0.01f;
        float destiny = Mathf.Clamp((float)(val - opc-1) / Mathf.Max(val-1, 1), 0, 1);

        Scrollbar s = null;
        if (sr != null)
        {
            if (sr.verticalScrollbar != null)
                s = sr.verticalScrollbar;
            else if (sr.horizontalScrollbar != null)
                s = sr.horizontalScrollbar;
        }

        if (s != null)
        {
            s.value = Mathf.Lerp(s.value,
                destiny, contadorDeTempo / TEMPO_DE_SCROLL);

            if (s.value != destiny)
                if (GlobalController.g)
                {
                    GlobalController.g.StartCoroutine(MovendoScroll(umaS, rowCellCount));
                }
        }

        //GlobalController.g.StartCoroutine(MovendoScroll(umaS, rowCellCount));
    }

    protected virtual IEnumerator MovendoScroll_H(UmaOpcao[] umaS, int rowCellCount)
    {
        yield return new WaitForSecondsRealtime(0.01f);
        int val = (rowCellCount == -1) ? umaS.Length : Mathf.CeilToInt((float)umaS.Length / rowCellCount);
        int opc = OpcaoEscolhida / ((rowCellCount == -1) ? 1 : rowCellCount);

        contadorDeTempo += 0.01f;
        float destiny = 1-Mathf.Clamp((float)(val - opc - 1) / Mathf.Max(val - 1, 1), 0, 1);
        
        sr.horizontalScrollbar.value = Mathf.Lerp(sr.horizontalScrollbar.value,
            destiny, contadorDeTempo / TEMPO_DE_SCROLL);

        
        if (sr.horizontalScrollbar.value != destiny)
            if (GlobalController.g)
            {
                GlobalController.g.StartCoroutine(MovendoScroll_H(umaS, rowCellCount));
            }

       // GlobalController.g.StartCoroutine(MovendoScroll(umaS, rowCellCount));
    }



    IEnumerator ScrollPos()
    {
        yield return new WaitForSecondsRealtime(0.01f);

        if (sr != null)
            if (sr.verticalScrollbar)
            {
                sr.verticalScrollbar.value = 1;
            }


        if (sr != null)
            if (sr.horizontalScrollbar)
                sr.horizontalScrollbar.value = 0;

        yield return new WaitForEndOfFrame();

        if (sr != null)
            if (sr.verticalScrollbar)
            {
                
                if (sr.verticalScrollbar.value != 1)
                    AgendaScrollPos();
                    
            }

        
        if (sr != null)
            if (sr.horizontalScrollbar)
                if (sr.horizontalScrollbar.value != 0)
                    AgendaScrollPos();
                    

    }

    public void FinalizarHud(int starter = 1)
    {
        for (int i = starter; i < painelDeTamanhoVariavel.transform.childCount; i++)
        {
            MonoBehaviour.Destroy(painelDeTamanhoVariavel.GetChild(i).gameObject);
            painelDeTamanhoVariavel.parent.parent.gameObject.SetActive(false);
        }

        FinalizarEspecifico();
    }

    
}
