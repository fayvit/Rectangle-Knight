using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MensagemComPainelArrayText : MonoBehaviour
{
    [SerializeField] private MessageVsUiText[] textForExibition = default; 

    [System.Serializable]
    private struct MessageVsUiText
    {
        public UnityEngine.UI.Text uiText;
        public ChaveDeTexto messageKey;
        public int indiceDoMessage;

        public MessageVsUiText(UnityEngine.UI.Text tt, ChaveDeTexto c, int indice)
        {
            uiText = tt;
            messageKey = c;
            indiceDoMessage = indice;
        }

    }

    private void OnEnable()
    {
        for (int i = 0; i < textForExibition.Length; i++)
        {
            MessageVsUiText m = textForExibition[i];
            m.uiText.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(m.messageKey)[m.indiceDoMessage];
        }
    }
    
}
