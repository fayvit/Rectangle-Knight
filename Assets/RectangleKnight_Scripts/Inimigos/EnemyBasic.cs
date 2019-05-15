using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : TargedEnemy
{
    [SerializeField] private float tempoEsperando = .75f;
    [SerializeField] private float velocidadeDoDeslocamento = 1.25f;

    private float distanciaDeDeslocamento = 1;
    private float tempoDecorrido = 0;
    private FasesDaMovimentacao fase = FasesDaMovimentacao.esperandoMove;

    private enum FasesDaMovimentacao
    {
        esperandoMove,
        move
    }

    protected override void Start()
    {
        PreviousMoveTarget = transform.position;
        base.Start();
        
    }

    protected virtual void Update()
    {
        tempoDecorrido += Time.deltaTime;
        switch (fase)
        {
            case FasesDaMovimentacao.esperandoMove:
                if (tempoDecorrido > tempoEsperando)
                {
                    distanciaDeDeslocamento = Vector3.Distance(PreviousMoveTarget, MovePoints[MoveTarget].position);
                    fase = FasesDaMovimentacao.move;

                    tempoDecorrido = 0;
                }
            break;
            case FasesDaMovimentacao.move:
                if(MovePoints.Length>0)
                    transform.position = Vector3.Lerp(PreviousMoveTarget,MovePoints[MoveTarget].position,
                        tempoDecorrido * velocidadeDoDeslocamento / distanciaDeDeslocamento);

                if (tempoDecorrido * velocidadeDoDeslocamento > distanciaDeDeslocamento)
                {
                    fase = FasesDaMovimentacao.esperandoMove;
                    tempoDecorrido = 0;
                    TrocaMoveTarget();
                }
            break;
        }

        
    }
}
