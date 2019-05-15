using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour {

    [SerializeField] private DadosDoPersonagem dados = new DadosDoPersonagem();
    //private EstouEmDano emDano;
    private Animator animator;
    
    public DadosDoPersonagem Dados
    {
        get { return dados; }
        set { dados = value; }
    }

    public Animator _Animator
    {
        get { if (animator != null)
                return animator;
            else
            {
                animator = GetComponent<Animator>();
                return animator;
            }
        }
        set { animator = value; }
    }

    public bool VerificaDerrota()
    {
        if (Dados.PontosDeVida <= 0)
            return true;
        else
            return false;
    }
}
