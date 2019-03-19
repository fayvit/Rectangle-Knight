using System;
using UnityEngine;

[Serializable]
public class LanguageMenu : UiDeOpcoes
{
    private Action<int> acaoDeOpcao;
#pragma warning disable 0649
    [SerializeField] private OpcaoDeLinguagem[] minhasOpcoes;
#pragma warning restore 0649
    [Serializable]
    private struct OpcaoDeLinguagem
    {
        [SerializeField]private idioma key;
        [SerializeField]private string labelDoIdioma;
        [SerializeField]private Sprite imgDoIdioma;

        public idioma Key { get => key; set => key = value; }
        public string LabelDoIdioma { get => labelDoIdioma; set => labelDoIdioma = value; }
        public Sprite ImgDoIdioma { get => imgDoIdioma; set => imgDoIdioma = value; }
    }

    public Sprite BandeirinhaAtualSelecionada()
    {
        for (int i = 0; i < minhasOpcoes.Length; i++)
        {
            if (minhasOpcoes[i].Key == BancoDeTextos.linguaChave)
                return minhasOpcoes[i].ImgDoIdioma;
        }

        return null;
    }

    public idioma IdiomaNoIndice(int indice)
    {
        return minhasOpcoes[indice].Key;
    }

    public Sprite BandeirinhaNoIndice(int indice)
    {
        return minhasOpcoes[indice].ImgDoIdioma;
    }

    public void IniciarHud(Action<int> acao)
    {
        acaoDeOpcao += acao;
        IniciarHUD(minhasOpcoes.Length);

    }

    public override void SetarComponenteAdaptavel(GameObject G, int indice)
    {
        G.GetComponent<UmaOpcaoDeLinguagem>().SetarOpcao(acaoDeOpcao,
            minhasOpcoes[indice].LabelDoIdioma,
            minhasOpcoes[indice].ImgDoIdioma);
    }

    protected override void FinalizarEspecifico()
    {
        acaoDeOpcao = null;
    }
}

