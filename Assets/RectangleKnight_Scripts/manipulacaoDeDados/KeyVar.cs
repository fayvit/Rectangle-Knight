using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public class KeyVar
{
    private Dictionary<KeyCont, int> contadorChave = new Dictionary<KeyCont, int>();
    private Dictionary<string, int> contadorAuto = new Dictionary<string, int>();
    private Dictionary<KeyShift, bool> shift = new Dictionary<KeyShift, bool>();
    private Dictionary<string, bool> autoShift = new Dictionary<string, bool>();
    private Dictionary<string, bool> enemyShift = new Dictionary<string, bool>();
    private Dictionary<MyVector2Int, MyColor> mapDates = new Dictionary<MyVector2Int, MyColor>();

    public Dictionary<MyVector2Int, MyColor> MapDates
    {
        get
        {

            if (mapDates != null)
            {
                return mapDates;
            }
            else
            {
                mapDates = new Dictionary<MyVector2Int, MyColor>();
                return mapDates;
            }
        }

        set => mapDates = value;
    }

    public  CapsuleList ListaDeCapsulas { get; private set; } = new CapsuleList();    

    public NomesCenas CenaAtiva { get; private set; } = NomesCenas.TutoScene;

    public List<NomesCenas> CenasAtivas { get; private set; } = new List<NomesCenas>();

    public void SetarCenasAtivas(NomesCenas[] cenasAtivas)
    {
        this.CenasAtivas = new List<NomesCenas>();

        this.CenasAtivas.AddRange(cenasAtivas);

        CenaAtiva = cenasAtivas[0];
    }

    public void SetarCenasAtivas()
    {
        NomesCenas[] nomesDeCenas = (NomesCenas[])(System.Enum.GetValues(typeof(NomesCenas)));
        CenasAtivas = new List<NomesCenas>();

        for (int i = 0; i < nomesDeCenas.Length; i++)
        {
            if (SceneManager.GetSceneByName(nomesDeCenas[i].ToString()).isLoaded)
            {
                CenasAtivas.Add(nomesDeCenas[i]);
            }
        }

        CenaAtiva = (NomesCenas)System.Enum.Parse(typeof(NomesCenas), SceneManager.GetActiveScene().name);
    }

    void MudaDic<T1, T2>(Dictionary<T1, T2> dic, T1 key, T2 val)
    {
        if (!dic.ContainsKey(key))
        {
            dic.Add(key, val);
        }
        else
            dic[key] = val;
    }

    public void MudaShift(KeyShift key, bool val = false)
    {
        MudaDic(shift, key, val);
    }

    public void MudaAutoShift(string key, bool val = false)
    {
        MudaDic(autoShift, key, val);
    }

    public void MudaCont(KeyCont key, int val = 0)
    {
        MudaDic(contadorChave, key, val);
    }

    public void MudaAutoCont(string key, int val = 0)
    {
        MudaDic(contadorAuto, key, val);
    }

    public void MudaEnemyShift(string key, bool val = false)
    {
        MudaDic(enemyShift, key, val);
    }

    public void SomaAutoCont(string key, int soma = 0)
    {
        if (contadorAuto.ContainsKey(key))
        {
            contadorAuto[key] += soma;
        }
        else
            contadorAuto.Add(key, soma);
    }

    public void SomaCont(KeyCont key, int soma = 0)
    {
        if (contadorChave.ContainsKey(key))
        {
            contadorChave[key] += soma;
        }
        else
            contadorChave.Add(key, soma);
    }

    public bool VerificaAutoShift(string key)
    {
        //Debug.Log(autoShift.ContainsKey(key));
        if (!autoShift.ContainsKey(key))
        {
            autoShift.Add(key, false);
            return false;
        }
        else
        { //Debug.Log(autoShift[key]); 
            return autoShift[key];
        }
    }

    public bool VerificaEnemyShift(string key)
    {
        //Debug.Log(autoShift.ContainsKey(key));
        if (!enemyShift.ContainsKey(key))
        {
            enemyShift.Add(key, false);
            return false;
        }
        else
        { //Debug.Log(autoShift[key]); 
            return enemyShift[key];
        }
    }


    public bool VerificaAutoShift(KeyShift key)
    {
        if (!shift.ContainsKey(key))
        {
            shift.Add(key, false);
            return false;
        }
        else return shift[key];
    }

    public int VerificaCont(KeyCont key)
    {
        if (!contadorChave.ContainsKey(key))
        {
            contadorChave.Add(key, 0);
            return 0;
        }
        else return contadorChave[key];
    }

    public int VerificaAutoCont(string key)
    {
        if (!contadorAuto.ContainsKey(key))
        {
            contadorAuto.Add(key, 0);
            return 0;
        }
        else return contadorAuto[key];
    }

    public void ReviverInimigos()
    {
        Dictionary<string,bool>.KeyCollection s = enemyShift.Keys;
        string[] s2 = new string[s.Count];
        s.CopyTo(s2, 0);

        foreach (string key in s2)
        {
            enemyShift[key] = false;
        }

    }
}

public enum KeyShift
{
    sempretrue = -3,
    sempreFalse = -2,
    nula = -1,
    pegouPrimeiroEmblema,
    conhecendoTyron,
    tyronUmCaminhoEmFrente,
    conhecendoHerika,
    algoEmComumComTyron,
    conhecendoOtto,
    fascinadoPelasCapsulas,
    prazerEmConhecerCapsulas,
    venceuCirculoImperfeito,
    escadaDasProfundezas,
    conhecendoTales,
    talescetico,
    talesOuveChamado,
    ottoSobreAquifero,
    conheceuOtto,
    venceuSetaSombria
}

public enum KeyCont
{
    nula = -1,
    losangulosPegos,
    losangulosConfirmados
}
