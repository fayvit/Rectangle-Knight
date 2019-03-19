using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : EnemyBase
{
    [SerializeField] private float tempoEsperando = .75f;
    [SerializeField] private float tempoDeDeslocamento = 1.25f;

#pragma warning disable 0649
    [SerializeField] private Transform[] movePoints;
#pragma warning restore 0649

    private int moveTarget = 0;
    private Vector3 previousMoveTarget;
    private float tempoDecorrido = 0;
    private FasesDaMovimentacao fase = FasesDaMovimentacao.esperandoMove;

    private enum FasesDaMovimentacao
    {
        esperandoMove,
        move
    }

    protected override void Start()
    {
        previousMoveTarget = transform.position;
        base.Start();
        
    }

    private void Update()
    {
        tempoDecorrido += Time.deltaTime;
        switch (fase)
        {
            case FasesDaMovimentacao.esperandoMove:
                if (tempoDecorrido > tempoEsperando)
                {
                    fase = FasesDaMovimentacao.move;
                    tempoDecorrido = 0;
                }
            break;
            case FasesDaMovimentacao.move:
                if(movePoints.Length>0)
                    transform.position = Vector3.Lerp(previousMoveTarget,movePoints[moveTarget].position,tempoDecorrido/tempoDeDeslocamento);

                if (tempoDecorrido > tempoDeDeslocamento)
                {
                    fase = FasesDaMovimentacao.esperandoMove;
                    tempoDecorrido = 0;
                    TrocaMoveTarget();
                }
            break;
        }

        void TrocaMoveTarget()
        {
            if (movePoints.Length > 0)
                previousMoveTarget = movePoints[moveTarget].position;

            if (movePoints.Length > moveTarget + 1)
                moveTarget++;
            else
                moveTarget = 0;
        }
    }
}
