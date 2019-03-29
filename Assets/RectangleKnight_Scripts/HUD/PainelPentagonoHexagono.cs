using UnityEngine;
using UnityEngine.UI;

public class PainelPentagonoHexagono : PainelUmaMensagem
{
    #region inspector
    [SerializeField] private Text textoDaDescricao = default;
    [SerializeField] private Image imgDaqui = default;
    [SerializeField] private Sprite[] labelImages = default;
    #endregion

    public enum Forma
    {
        pentagono,
        hexagono,
        naoForma
    }

    public void ConstroiPainelDosPentagonosOuHexagonos(System.Action r,Forma f)
    {
        string[] s = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.hexagonPentagonTips).ToArray();
        if (f == Forma.hexagono)
        {
            ConstroiPainelUmaMensagem(r, s[0]);
            textoDaDescricao.text = s[1];
            imgDaqui.sprite = labelImages[GameController.g.Manager.Dados.PartesDeHexagonoObtidas];
        }
        else if (f == Forma.pentagono)
        {
            ConstroiPainelUmaMensagem(r, s[2]);
            textoDaDescricao.text = s[3];
            imgDaqui.sprite = labelImages[GameController.g.Manager.Dados.PartesDePentagonosObtidas];
        }
        else {
            ConstroiPainelUmaMensagem(r);
        }
    }
}
