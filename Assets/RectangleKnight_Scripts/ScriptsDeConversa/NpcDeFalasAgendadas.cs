using UnityEngine;
using System.Collections;

[System.Serializable]
public class NpcDeFalasAgendadas : NPCdeConversa
{
    #region inspector
    [SerializeField] private FalasAgendaveis[] falas = default;
    #endregion

    private int ultimoIndice = -1;

    [System.Serializable]
    private class FalasAgendaveis
    {
        [SerializeField] private KeyShift chaveCondicionalDaConversa;
        [SerializeField] private ChaveDeTexto chaveDeTextoDaConversa;
        [SerializeField] private int repetir = 0;

        public KeyShift ChaveCondicionalDaConversa
        {
            get { return chaveCondicionalDaConversa; }
            set { chaveCondicionalDaConversa = value; }
        }

        public ChaveDeTexto ChaveDeTextoDaConversa
        {
            get { return chaveDeTextoDaConversa; }
            set { chaveDeTextoDaConversa = value; }
        }

        //public bool EstaAgendado { get => estaAgendado; set => estaAgendado = value; }
        public int Repetir { get => repetir; set => repetir = value; }
    }

    void VerificaQualFala()
    {
        KeyVar myKeys = GameController.g.MyKeys;

        Debug.Log("ultimo indice no inicio: " + ultimoIndice);

        //int indiceInicial = ultimoIndice < falas.Length ? Mathf.Max(ultimoIndice,0) : 0;
        int indiceFinal = ultimoIndice >0 ? Mathf.Min(ultimoIndice,falas.Length) : falas.Length;
        

        for (int i=0;i<indiceFinal;i++)
        //for (int i = falas.Length - 1; i >= indiceInicial; i--)
        {
            if (myKeys.VerificaAutoShift(falas[i].ChaveCondicionalDaConversa))
            {

                conversa = BancoDeTextos.RetornaListaDeTextoDoIdioma(falas[i].ChaveDeTextoDaConversa).ToArray();
                ultimoIndice = i;
            }
        }

        Debug.Log(indiceFinal+" : "+ultimoIndice);

        if (falas[ultimoIndice].Repetir >= 0)
        {
            string kCont = falas[ultimoIndice].ChaveCondicionalDaConversa.ToString();
            
            myKeys.SomaAutoCont(kCont, 1);
            if (falas[ultimoIndice].Repetir < myKeys.VerificaAutoCont(kCont))
                myKeys.MudaShift(falas[ultimoIndice].ChaveCondicionalDaConversa, false);

        }

        //ultimoIndice--;

        /*
        if (!GameController.g.MyKeys.VerificaAutoShift(falas[i].ChaveCondicionalDaConversa))
            conversa = BancoDeTextos.RetornaListaDeTextoDoIdioma(falas[i].ChaveDeTextoDaConversa).ToArray();
    // conversa é uma variavel protected da classe pai*/

    }

    override public void IniciaConversa()
    {
        VerificaQualFala();
        base.IniciaConversa();
    }
}