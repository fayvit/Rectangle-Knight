using UnityEngine;
using System.Collections;

public class StringParaEnum
{
    public static string[] SetarConversaOriginal(string chaveDaConversaGambiarraString,ref ChaveDeTexto chaveDaConversa)
    {
        if (chaveDaConversaGambiarraString != "")
        {
            try
            {
                chaveDaConversa = (ChaveDeTexto)System.Enum.Parse(typeof(ChaveDeTexto), chaveDaConversaGambiarraString);
            }
            catch (System.ArgumentException e)
            {
                Debug.LogError("string para texto invalida no enum \n" + e.StackTrace);
            }
        }

        return BancoDeTextos.RetornaListaDeTextoDoIdioma(chaveDaConversa).ToArray();
    }

    public static T ObterEnum<T>(string chaveDaConversaGambiarraString, T original)
    {
        T chave = original;
        if (chaveDaConversaGambiarraString != "")
        {
            try
            {
                chave = (T)System.Enum.Parse(typeof(T), chaveDaConversaGambiarraString);
            }
            catch (System.ArgumentException e)
            {
                Debug.LogError("string para enum: " + typeof(T) + " invalida no enum \n" + e.StackTrace);
            }
        }

        return chave;
    }

    public static T ObterEnum<T>(string chaveDaConversaGambiarraString, bool standard = false)
    {
        T chave = default(T);
        if (chaveDaConversaGambiarraString != "")
        {
            try
            {
                chave = (T)System.Enum.Parse(typeof(T), chaveDaConversaGambiarraString);
            }
            catch (System.ArgumentException e)
            {
                if (standard)
                    return default;
                else
                    Debug.LogError("string "+chaveDaConversaGambiarraString+" para enum: "+typeof(T)+" invalida no enum \n" + e.StackTrace);
            }
        }

        return chave;
    }
}