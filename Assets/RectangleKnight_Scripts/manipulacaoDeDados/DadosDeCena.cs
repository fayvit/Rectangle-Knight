using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DadosDeCena {

    public NomesCenas nomeDaCena;
    public LimitantesDaCena limitantes;
    public Color bkColor;
    public NameMusicaComVolumeConfig musicName = new NameMusicaComVolumeConfig()
    {
        Musica = NameMusic.initialAdventureTheme,
        Volume = 1
    };

    [System.Serializable]
    public class LimitantesDaCena : System.ICloneable,System.IComparable
    {
        public float xMin;
        public float xMax;
        public float yMin;
        public float yMax;

        public object Clone()
        {
            return new LimitantesDaCena()
            {
                xMax = xMax,
                xMin = xMin,
                yMin = yMin,
                yMax = yMax
            };
        }

        public int CompareTo(LimitantesDaCena obj)
        {
            return CompareTo((object)obj);
        }

        public int CompareTo(object obj)
        {
            LimitantesDaCena l = (LimitantesDaCena)obj;
            int retorno = Mathf.RoundToInt(Mathf.Abs(l.xMax - xMax) + Mathf.Abs(l.xMin - xMin) + Mathf.Abs(l.yMax - yMax) + Mathf.Abs(l.yMin - yMin));
            Debug.Log("comparação de limitantes: " + retorno + " : " + Mathf.Abs(l.xMax - xMax) + " : " + Mathf.Abs(l.xMin - xMin) + " : " + Mathf.Abs(l.yMax - yMax) + " : " + Mathf.Abs(l.yMin - yMin));
            return retorno;
        }
    }
}

[System.Serializable]
public class ContainerDeDadosDeCena
{
    public List<DadosDeCena> meusDados;

    public DadosDeCena GetCurrentSceneDates()
    {
        
        string s = SceneManager.GetActiveScene().name;

        Debug.Log(s);
        return GetSceneDates(s);
    }

    public DadosDeCena GetSceneDates(string nome)
    {
        NomesCenas s = StringParaEnum.ObterEnum<NomesCenas>(nome,true);

        //Debug.Log(s+" : "+default(NomesCenas));

        if (s != default)
        {
            return GetSceneDates(s);
        }

        return null;
    }

    public DadosDeCena GetSceneDates(NomesCenas nome)
    {
        for (int i = 0; i < meusDados.Count; i++)
            if (meusDados[i].nomeDaCena == nome)
                return meusDados[i];

        Debug.Log("Parece que não foi encontrada a cena no dicionario");

        return null;
    }
}