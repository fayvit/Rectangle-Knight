using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjetilParabolico : ProjetilInimigo
{
    private float tempoDecorrido = 0;
    [SerializeField]private Vector2 posInicial;
    [SerializeField]private Vector2 vertice;// { get; set; }

    public void Iniciar(Vector2 posAlvo,Vector2 vertice,GameObject particle,float vel)
    {
        this.vertice = vertice;
        IniciarProjetilInimigo(posAlvo, particle, vel,SoundEffectID.lancaProjetilInimigo);
        Dir = posAlvo;
    }

    private void Start()
    {
        posInicial = transform.position;// new Vector2(transform.position.x, transform.position.y);
    }

    protected override void Update()
    {
        tempoDecorrido += Time.deltaTime;
        transform.position = ZeroOneInterpolation.ParabolaDeDeslocamento(
            posInicial,
            Dir,
            vertice,
            tempoDecorrido/(Mathf.Abs(posInicial.x-Dir.x)) * Velocidade
            );
    }
}
