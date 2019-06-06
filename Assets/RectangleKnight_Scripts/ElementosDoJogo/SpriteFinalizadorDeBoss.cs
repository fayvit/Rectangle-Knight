using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteFinalizadorDeBoss
{
    private GameObject interestObject;
    private Vector3 interestVector3;
    private SpriteRenderer meuSprite;
    private float tempoDecorrido = 0;

    [SerializeField] private GameObject spriteFinalizador = default;
    [SerializeField] private float TEMPO_ESCALONANDO_SPRITE = 2.5f;

    public void InstanciarSpriteFinalizador(Vector3 startPosition)
    {
        tempoDecorrido = 0;
        interestObject = InstanciaLigando.Instantiate(spriteFinalizador, startPosition, 5);
        interestVector3 = interestObject.transform.localScale;
        meuSprite = interestObject.GetComponent<SpriteRenderer>();
    }

    public bool Update()
    {
        tempoDecorrido += Time.deltaTime;
        if (tempoDecorrido > TEMPO_ESCALONANDO_SPRITE)
        {
            return true;
        }
        else
        {
            EscalonaSprite();
        }

        return false;
    }


    void EscalonaSprite()
    {
        interestObject.transform.localScale =
                        Vector3.Lerp(interestVector3, new Vector3(1000, 1000, 100), tempoDecorrido / TEMPO_ESCALONANDO_SPRITE);

        Color C = meuSprite.color;
        meuSprite.color = new Color(C.r, C.g, C.b, Mathf.Lerp(1, 0,
            ZeroOneInterpolation.PolinomialInterpolation(
            tempoDecorrido / TEMPO_ESCALONANDO_SPRITE, 4)));
    }
}
