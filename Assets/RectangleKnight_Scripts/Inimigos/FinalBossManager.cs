using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossManager: MonoBehaviour
{
    [SerializeField] private GameObject spriteFinalizador = default;
    [SerializeField] private GameObject particulasPreparaAnimaMorte = default;
    [SerializeField] private GameObject particulasAnimaMorte = default;
    [SerializeField] private GameObject particulaFinalizaAnimaMorte = default;
    [SerializeField] private GameObject premio = default;
    [SerializeField] private float TEMPO_ESCALONANDO_SPRITE = 2.5F;
    [SerializeField] private float TEMPO_AGUARDANDO_PARTICULAS = 1.5F;
    [SerializeField] private float TEMPO_DAS_PARTICULAS = 2.5F;

    private Transform boss;
    private float tempoDecorrido = 0;
    private GameObject interestObject;
    private Vector3 interestVector3;
    private SpriteRenderer meuSprite;
    private EstadoDaqui estado = EstadoDaqui.emEspera;

    private enum EstadoDaqui
    {
        emEspera,
        spriteFinalizador,
        aguardandoParticulas,
        visualizandoParticulas,
        aguardandoPremio,
        animaPremio
    }

    public void IniciarFinalizacao(Transform boss)
    {
        this.boss = boss;
        interestObject =  InstanciaLigando.Instantiate(spriteFinalizador, boss.position, 5);
        interestVector3 = interestObject.transform.localScale;
        meuSprite = interestObject.GetComponent<SpriteRenderer>();
        estado = EstadoDaqui.spriteFinalizador;
    }
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if (estado != EstadoDaqui.emEspera)
            tempoDecorrido += Time.deltaTime;

        switch (estado)
        {
            case EstadoDaqui.spriteFinalizador:
                if (tempoDecorrido > TEMPO_ESCALONANDO_SPRITE)
                {
                    interestObject = InstanciaLigando.Instantiate(particulasPreparaAnimaMorte, boss.position,5);
                    estado = EstadoDaqui.aguardandoParticulas;
                    tempoDecorrido = 0;
                }
                else
                {
                    EscalonaSprite();
                }
            break;
            case EstadoDaqui.aguardandoParticulas:
                if (tempoDecorrido > TEMPO_AGUARDANDO_PARTICULAS)
                {
                    interestObject = InstanciaLigando.Instantiate(particulasAnimaMorte, boss.position);
                    estado = EstadoDaqui.visualizandoParticulas;
                    tempoDecorrido = 0;
                }
            break;
            case EstadoDaqui.visualizandoParticulas:
                if (tempoDecorrido > TEMPO_DAS_PARTICULAS)
                {
                    MonoBehaviour.Destroy(interestObject);
                    
                    InstanciaLigando.Instantiate(particulaFinalizaAnimaMorte, boss.position,5);
                    interestObject = InstanciaLigando.Instantiate(spriteFinalizador, boss.position);
                    interestVector3 = interestObject.transform.localScale;
                    meuSprite = interestObject.GetComponent<SpriteRenderer>();
                    premio.transform.position = boss.position;

                    MonoBehaviour.Destroy(boss.gameObject);
                    estado = EstadoDaqui.aguardandoPremio;
                    tempoDecorrido = 0;
                }
            break;
            case EstadoDaqui.aguardandoPremio:
                if (tempoDecorrido < TEMPO_ESCALONANDO_SPRITE)
                {
                    EscalonaSprite();
                }
                else
                {
                    EventAgregator.Publish(EventKey.requestSceneCamLimits, null);
                    EventAgregator.Publish(EventKey.fechouPainelSuspenso, null);
                    premio.SetActive(true);
                    Destroy(interestObject);
                    estado = EstadoDaqui.animaPremio;
                }
            break;
        }
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
