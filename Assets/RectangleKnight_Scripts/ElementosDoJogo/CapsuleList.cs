using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CapsuleList
{
    public List<CapsuleInfo> lista = new List<CapsuleInfo>()
    {
        new CapsuleInfo()
        {
            Cena = NomesCenas.capsulaDaGargantaDasProfundezas,
            ID = CapsuleID.gargantaDasProfundezas,
            Pos = new Vector3(28,-130,0),
            Ativada = true
        },
        new CapsuleInfo()
        {
            Cena = NomesCenas.AcampamentoDosRejeitados,
            ID = CapsuleID.acampamentoDosRejeitados,
            Pos = new Vector3(340,-36,0),
            Ativada = true
        },
        new CapsuleInfo()
        {
            Cena = NomesCenas.AbsolutamenteCapsulaDoAquifero,
            ID = CapsuleID.aquiferoDoBuscador,
            Pos = new Vector3(-1164,-14,0),
            Ativada = false
        },
    };

    private CapsuleInfo GetInnerCapsuleInfo(CapsuleID ID)
    {
        CapsuleInfo retorno = null;

        for (int i = 0; i < lista.Count; i++)
            if (lista[i].ID == ID)
                retorno = lista[i];

        return retorno;
    }

    public CapsuleInfo GetCapsuleInfo(CapsuleID ID)
    {
        CapsuleInfo retorno = GetInnerCapsuleInfo(ID);
        

        if (retorno == null)
        {
            retorno = new CapsuleList().GetInnerCapsuleInfo(ID);

            if (retorno != null)
                lista.Add(retorno);
        }

        return retorno;
    }

    public List<CapsuleInfo> GetActiveCapsules()
    {
        List<CapsuleInfo> retorno = new List<CapsuleInfo>();

        for (int i = 0; i < lista.Count; i++)
        {
            if (lista[i].Ativada)
                retorno.Add(lista[i]);
        }

        return retorno;
    }

    public void ChangeForActive(CapsuleID minhaID)
    {
        for (int i = 0; i < lista.Count; i++)
            if (lista[i].ID == minhaID)
                lista[i].Ativada = true;
    }

    public string[] GetActiveCapsuleNames()
    {
        List<CapsuleInfo> capsuleInfos = GetActiveCapsules();
        List<string> nomes = new List<string>();

        for (int i = 0; i < capsuleInfos.Count; i++)
            nomes.Add(capsuleInfos[i].NomeEmLinguas);

        return nomes.ToArray();
    }
}

[System.Serializable]
public class CapsuleInfo
{
    private float[] pos { get; set; }

    public Vector3 Pos
    {
        get { return new Vector3(pos[0], pos[1], pos[2]); }
        set { pos = new float[3] { value.x, value.y, value.z }; }
    }
    public NomesCenas Cena { get; set; }
    public CapsuleID ID { get; set; }
    public bool Ativada { get; set; }

    public string NomeEmLinguas {
        get {
            return BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.nomesParaViagensDeCapsula)[(int)ID];
        }
    }
}

public enum CapsuleID//lembrar de colocar o texto nos textos chave
{
    gargantaDasProfundezas,
    acampamentoDosRejeitados,
    aquiferoDoBuscador
}