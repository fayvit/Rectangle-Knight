using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuPentagonosHexagonos
{
    #region inspector
    [SerializeField] private Image partesDeHexagonoObtidas = default;
    [SerializeField] private Image partesDePentagonosObtidas = default;
    [SerializeField] private Text numHexagonosCompletados = default;
    [SerializeField] private Text numPentagonosCompletados = default;
    [SerializeField] private Text totalDeHp = default;
    [SerializeField] private Text totalDeMp = default;
    [SerializeField] private Sprite[] hexagonoSprites = default;
    [SerializeField] private Sprite[] pentagonoSprites = default;
    #endregion

    public void IniciarHud()
    {
        partesDeHexagonoObtidas.transform.parent.gameObject.SetActive(true);

        DadosDoJogador dj = GameController.g.Manager.Dados;
        partesDeHexagonoObtidas.sprite = hexagonoSprites[dj.PartesDeHexagonoObtidas];
        partesDePentagonosObtidas.sprite = pentagonoSprites[dj.PartesDePentagonosObtidas];
        numHexagonosCompletados.text = dj.HexagonosCompletados.ToString();
        numPentagonosCompletados.text = dj.PentagonosCompletados.ToString();
        totalDeHp.text = dj.PontosDeVida + " / " + dj.MaxVida;
        totalDeMp.text = dj.PontosDeMana + " / " + dj.MaxMana;
    }

    public void FinalizarHud()
    {
        partesDeHexagonoObtidas.transform.parent.gameObject.SetActive(false);
    }
}