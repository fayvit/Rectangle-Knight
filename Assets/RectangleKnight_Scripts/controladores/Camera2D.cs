using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Transform alvo;
#pragma warning restore 0649

    [SerializeField] private float amortecimento = 0.2f;
    [SerializeField] private float fatorDeVisaoAFrente = 4;
    [SerializeField] private float velocidadeParaCameraFrente = 0.5f;
    [SerializeField] private float limiteDeVariacaoParaCameraFrente = 0.1f;

    private bool useLimitsCam = false;
    private float distanciaZ;
    private float fatorCima;
    private float limiteCimaBaixo = 4;
    private DadosDeCena.LimitantesDaCena limitantes;
    private Vector3 ultimaPosicaoDoAlvo;
    private Vector3 velocidadeDeReferencia;
    private Vector3 posicaoDeOlharFrente;
    private float wordWidthOfScreen;
    private float wordheightOfScreen;

    // Use this for initialization
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        Vector3 V1 = cam.ViewportToWorldPoint(Vector3.zero);
        Vector3 V2 = cam.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height/2,0));
        wordWidthOfScreen = V2.x - V1.x;
        wordheightOfScreen = V2.y - V1.y;

        fatorCima = 0;// Vector3.Distance(cam.ScreenPointToRay(Vector3.zero).origin, cam.ScreenPointToRay(new Vector3(0, cam.pixelHeight, 0)).origin)/4;
        
        ultimaPosicaoDoAlvo = alvo.position;
        distanciaZ = (transform.position - alvo.position).z;
        transform.parent = null;

        

        EventAgregator.AddListener(EventKey.requestToFillDates, OnRequestFillDates);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.requestToFillDates, OnRequestFillDates);
    }

    private void OnRequestFillDates(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        SaveDates S = (SaveDates)ssge.MyObject[0];

        if (S == null)
        {
            transform.position = new Vector3(-8, -2, -10);
        }
        else
        {
            transform.position = S.Posicao + new Vector3(0,0,-10);
        }

        SetarLimitantes();
    }

    public void AposMudarDeCena(Vector3 pos)
    {
        transform.position = pos;
        SetarLimitantes();
    }

    DadosDeCena.LimitantesDaCena CalcularLimitantes(DadosDeCena c)
    {
        return new DadosDeCena.LimitantesDaCena()
        {
            xMin = c.limitantes.xMin + (int)wordWidthOfScreen,
            xMax = c.limitantes.xMax - (int)wordWidthOfScreen,
            yMin = c.limitantes.yMin + (int)wordheightOfScreen,
            yMax = c.limitantes.yMax - (int)wordheightOfScreen
        };
    }

    void SetarLimitantes()
    {
        DadosDeCena c = GlobalController.g.SceneDates.GetCurrentSceneDates();

        limitantes = c!=null?CalcularLimitantes(c):null;

        if (limitantes != null)
            useLimitsCam = true;
        else
            useLimitsCam = false;
    }

    // Update is called once per frame
    void Update()
    {
        float variacaoDaPosicaoX = (alvo.position - ultimaPosicaoDoAlvo).x;

        bool atualizarCamerafrente = Mathf.Abs(variacaoDaPosicaoX) > limiteDeVariacaoParaCameraFrente;

        if (atualizarCamerafrente)
        {
            posicaoDeOlharFrente = fatorDeVisaoAFrente * Vector3.right * Mathf.Sign(variacaoDaPosicaoX);
        }
        else
        {
            posicaoDeOlharFrente = Vector3.MoveTowards(posicaoDeOlharFrente, Vector3.zero, Time.deltaTime * velocidadeParaCameraFrente);
        }

        transform.position = new Vector3(transform.position.x, alvo.position.y, transform.position.z);

        Vector3 posDoAlvoFrente = alvo.position + posicaoDeOlharFrente + Vector3.forward * distanciaZ + fatorCima * Vector3.up;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, posDoAlvoFrente , ref velocidadeDeReferencia, amortecimento);

        if (Mathf.Abs(transform.position.y - posDoAlvoFrente.y) > limiteCimaBaixo/3)
        {
            newPos = Vector3.SmoothDamp(transform.position, posDoAlvoFrente, ref velocidadeDeReferencia, amortecimento/2);
        }

        if (useLimitsCam)
        {
            newPos = new Vector3(Mathf.Clamp(newPos.x, limitantes.xMin, limitantes.xMax),
                Mathf.Clamp(newPos.y, limitantes.yMin, limitantes.yMax),
                newPos.z
                );
        }

        if (Mathf.Abs(transform.position.y - posDoAlvoFrente.y) > limiteCimaBaixo)
            newPos = Vector3.SmoothDamp(transform.position, posDoAlvoFrente, ref velocidadeDeReferencia, amortecimento / 4);



        transform.position = newPos;

        ultimaPosicaoDoAlvo = alvo.position;
    }
}
