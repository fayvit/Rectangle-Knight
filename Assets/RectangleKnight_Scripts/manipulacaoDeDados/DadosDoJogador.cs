using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DadosDoJogador : DadosDoPersonagem
{
    [SerializeField] private bool _temDash = false;
    [SerializeField] private bool _temDoubleJump = false;
    [SerializeField] private bool _temDownArrowJump = false;
    [SerializeField] private bool _temMagicAttack = false;
    [SerializeField] private bool _espadaAzul = false;
    [SerializeField] private bool _espadaVerde = false;
    [SerializeField] private bool _espadaDourada = false;
    [SerializeField] private bool _espadaVermelha = false;

    public bool TemDash { get => _temDash; set => _temDash = value; }
    public bool TemDoubleJump { get => _temDoubleJump; set => _temDoubleJump = value; }
    public bool TemDownArrowJump { get => _temDownArrowJump; set => _temDownArrowJump = value; }
    public bool TemMagicAttack { get => _temMagicAttack; set => _temMagicAttack = value; }
    public bool EspadaAzul { get => _espadaAzul; set => _espadaAzul = value; }
    public bool EspadaVerde { get => _espadaVerde; set => _espadaVerde = value; }
    public bool EspadaDourada { get => _espadaDourada; set => _espadaDourada = value; }
    public bool EspadaVermelha { get => _espadaVermelha; set => _espadaVermelha = value; }
    public int Dinheiro { get; set; } = 0;
    public int EspacosDeEmblemas { get; set; } = 2;
    public int PartesDeHexagonoObtidas { get; set; } = 0;
    public int PartesDePentagonosObtidas { get; set; } = 0;
    public int HexagonosCompletados { get; set; } = 0;
    public int PentagonosCompletados { get; set; } = 0;
    public int SeloDoProgresso { get; set; } = 0;
    public int SeloDoAmor { get; set; } = 0;
    public int SeloDoOrdem { get; set; } = 0;
    public SwordColor CorDeEspadaSelecionada { get; set; } = SwordColor.grey;

    public override int AtaqueBasico {
        get {
            float modificador = 1;
            float adicional = 0;

            if (GameController.g.MyKeys.VerificaAutoShift("equiped_" + NomesEmblemas.ataqueAprimorado.ToString()))
            {
                adicional = 10;
            }

            return Mathf.RoundToInt(modificador*base.AtaqueBasico+adicional);

        }
        set => base.AtaqueBasico = value;
    }

    public List<Emblema> MeusEmblemas{get;set;} = new List<Emblema>() { new Emblema(NomesEmblemas.dinheiroMagnetico,1),
        new Emblema(NomesEmblemas.dinheiroMagnetico,1),
        new Emblema(NomesEmblemas.ataqueAprimorado,1)
    };

    public DinheiroCaido DinheiroCaido { get; set; } = new DinheiroCaido();

    public UltimoCheckPoint ultimoCheckPoint = new UltimoCheckPoint()
    {
        nomesDasCenas = new NomesCenas[1] { NomesCenas.TutoScene},
        Pos = new Vector3(-8, -2, 0)
    };

    public void PegouSelo(SeloPositivista.TipoSelo tipo,int quantidade = 1)
    {
        switch (tipo)
        {
            case SeloPositivista.TipoSelo.progresso:
                SeloDoProgresso += quantidade;
            break;
            case SeloPositivista.TipoSelo.amor:
                SeloDoAmor += quantidade;
            break;
            case SeloPositivista.TipoSelo.ordem:
                SeloDoOrdem += quantidade;
            break;
        }
    }

    public void SomaHexagono()
    {
        PartesDeHexagonoObtidas++;
    }

    public void SomaPentagono()
    {
        PartesDePentagonosObtidas++;
    }

    public void GetSword(SwordColor cor)
    {
        switch (cor)
        {
            case SwordColor.blue:
                EspadaAzul = true;
            break;
            case SwordColor.green:
                EspadaVerde = true;
            break;
            case SwordColor.gold:
                EspadaDourada = true;
            break;
            case SwordColor.red:
                EspadaVermelha = true;
            break;
        }
    }

    public bool SwordAvailable(SwordColor cor)
    {
        bool retorno = false;
        switch (cor)
        {
            case SwordColor.blue:
                retorno = EspadaAzul;
            break;
            case SwordColor.green:
                retorno = EspadaVerde;
            break;
            case SwordColor.gold:
                retorno = EspadaDourada;
            break;
            case SwordColor.red:
                retorno = EspadaVermelha;
            break;
        }

        return retorno;
    }

}

[System.Serializable]
public class UltimoCheckPoint
{
    private float[] pos=new float[3] {0,0,0};

    public Vector3 Pos {
        get => new Vector3(pos[0], pos[1], pos[2]);
        set => pos = new float[3]{value.x, value.y, value.z};
    }
    public NomesCenas[] nomesDasCenas;
}

[System.Serializable]
public class DinheiroCaido
{
    private float[] pos = new float[3] { 0, 0, 0 };

    public Vector3 Pos
    {
        get => new Vector3(pos[0], pos[1], pos[2]);
        set => pos = new float[3] { value.x, value.y, value.z };
    }

    public NomesCenas cenaOndeCaiu;
    public int valor = 0;
    public bool estaCaido = false;
}
