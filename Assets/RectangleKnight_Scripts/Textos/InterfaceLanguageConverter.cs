using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InterfaceLanguageConverter : MonoBehaviour
{
    private Text textoConvertivel;
#pragma warning disable 0649
    [SerializeField]private InterfaceTextKey key;
#pragma warning restore 0649
    public void MudaTexto()
    {
        if (textoConvertivel != null)
        {
            textoConvertivel.text = BancoDeTextos.RetornaTextoDeInterface(key);
        }
        else
        {
            Invoke("MudaTexto", 0.15f);
            Debug.Log("Fiz um Invoke de texto");
        }
    }

    void OnEnable()
    {
        textoConvertivel = GetComponent<Text>();
        MudaTexto();
    }
}