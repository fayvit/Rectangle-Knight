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
    public SwordColor CorDeEspadaSelecionada { get; set; } = SwordColor.grey;

    public List<Emblema> MeusEmblemas{get;set;} = new List<Emblema>() { new Emblema(NomesEmblemas.dinheiroMagnetico,1),
        new Emblema(NomesEmblemas.dinheiroMagnetico,1),
        new Emblema(NomesEmblemas.dinheiroMagnetico,1)
    };

    public DinheiroCaido DinheiroCaido { get; set; } = new DinheiroCaido();

    public UltimoCheckPoint ultimoCheckPoint = new UltimoCheckPoint()
    {
        nomesDasCenas = new NomesCenas[1] { NomesCenas.TutoScene},
        Pos = new Vector3(-8, -2, 0)
    };

    public void SomaHexagono()
    {
        PartesDeHexagonoObtidas++;
    }

    public void SomaPentagono()
    {
        PartesDePentagonosObtidas++;
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
