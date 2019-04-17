using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Transform alvo;
    [SerializeField] private ShakeCam shake = default;
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
    private DadosDeCena.LimitantesDaCena lerpLimitantes;
    private DadosDeCena.LimitantesDaCena lerpLimitantesTransitorio;
    private Vector3 ultimaPosicaoDoAlvo;
    private Vector3 velocidadeDeReferencia;
    private Vector3 posicaoDeOlharFrente;
    private float wordWidthOfScreen;
    private float wordheightOfScreen;

    private float tempoDecorrido = 0;
    [SerializeField]private float tempoDeLerpLimits = 4;
    private bool pedindoLimiteLerp = false;

    // Use this for initialization
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        Vector3 V1 = cam.ViewportToWorldPoint(Vector3.zero);
        Vector3 V2 = cam.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height/2,0));
        wordWidthOfScreen = V2.x - V1.x;
        wordheightOfScreen = V2.y - V1.y;

        shake.Construir(transform);
        fatorCima = 0;// Vector3.Distance(cam.ScreenPointToRay(Vector3.zero).origin, cam.ScreenPointToRay(new Vector3(0, cam.pixelHeight, 0)).origin)/4;
        
        ultimaPosicaoDoAlvo = alvo.position;
        distanciaZ = (transform.position - alvo.position).z;
        transform.parent = null;

        

        EventAgregator.AddListener(EventKey.requestToFillDates, OnRequestFillDates);
        EventAgregator.AddListener(EventKey.requestChangeCamLimits, OnRequestChangeCamLimits);
        EventAgregator.AddListener(EventKey.requestSceneCamLimits, OnRequestStandardLimits);
        EventAgregator.AddListener(EventKey.requestShakeCam, OnRequestShakeCam);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.requestToFillDates, OnRequestFillDates);
        EventAgregator.RemoveListener(EventKey.requestChangeCamLimits, OnRequestChangeCamLimits);
        EventAgregator.RemoveListener(EventKey.requestSceneCamLimits, OnRequestStandardLimits);
        EventAgregator.RemoveListener(EventKey.requestShakeCam, OnRequestShakeCam);
    }

    private void OnRequestShakeCam(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        int totalShake = 5;
        float angle = 1;
        ShakeAxis axis = ShakeAxis.x;

        if (ssge.MyObject.Length > 0)
        {
            axis = (ShakeAxis)ssge.MyObject[0];
            if (ssge.MyObject.Length > 1)
            {
                totalShake = (int)ssge.MyObject[1];
                angle = (float)ssge.MyObject[2];
            }
        }

        shake.IniciarShake(axis, totalShake, angle);

    }

    private void OnRequestStandardLimits(IGameEvent e)
    {
        SetarLimitantesTransitorio();

        if (lerpLimitantes != null)
        {
            tempoDecorrido = 0;
            pedindoLimiteLerp = true;
            
        }else
            limitantes = lerpLimitantesTransitorio;
    }

    private void OnRequestChangeCamLimits(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;

        lerpLimitantes = CalcularLimitantes((DadosDeCena.LimitantesDaCena)ssge.MyObject[0]);

        Vector3 pos = transform.position;
        if (!useLimitsCam)
            limitantes = new DadosDeCena.LimitantesDaCena()
            {
                xMax = (pos.x + wordWidthOfScreen),
                xMin = (pos.x - wordWidthOfScreen),
                yMin = (pos.y - wordheightOfScreen),
                yMax = (pos.y + wordheightOfScreen)
            };

        tempoDeLerpLimits = (float)ssge.MyObject[1];

        lerpLimitantesTransitorio = (DadosDeCena.LimitantesDaCena)limitantes.Clone();

        tempoDecorrido = 0;
        useLimitsCam = true;
        pedindoLimiteLerp = true;
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

        SetarLimitantesTransitorio();
        limitantes = lerpLimitantes;
    }

    public void AposMudarDeCena(Vector3 pos)
    {
        transform.position = pos;
        SetarLimitantesTransitorio();
        limitantes = lerpLimitantes;
    }

    DadosDeCena.LimitantesDaCena CalcularLimitantes(DadosDeCena.LimitantesDaCena c)
    {
        return new DadosDeCena.LimitantesDaCena()
        {
            xMin = (c.xMax - c.xMin) > 2*(int)wordWidthOfScreen? c.xMin + (int)wordWidthOfScreen:ValorDeAjuste(c.xMin,c.xMax)-1,
            xMax = (c.xMax - c.xMin) > 2*(int)wordWidthOfScreen ? c.xMax - (int)wordWidthOfScreen: ValorDeAjuste(c.xMin, c.xMax)+1,
            yMin = (c.yMax-c.yMin)>2*(int)wordheightOfScreen? c.yMin + (int)wordheightOfScreen:ValorDeAjuste(c.yMin,c.yMax)-1,
            yMax = (c.yMax - c.yMin) > 2*(int)wordheightOfScreen ? c.yMax - (int)wordheightOfScreen: ValorDeAjuste(c.yMin, c.yMax)+1
        };
    }

    float ValorDeAjuste(float xI,float xF)
    {
        return (xI + xF) / 2f;
    }

    void SetarLimitantesTransitorio()
    {
        DadosDeCena c = GlobalController.g.SceneDates.GetCurrentSceneDates();

        if(c!=null)
            GetComponent<Camera>().backgroundColor = c.bkColor;

        /*
        DadosDeCena.LimitantesDaCena dl = c.limitantes;
        Debug.Log(dl.xMin+" : "+dl.xMax+" : "+dl.yMin+" : "+dl.yMax);
        */

        lerpLimitantes = c!=null?CalcularLimitantes(c.limitantes):null;


        if (lerpLimitantes != null)
            useLimitsCam = true;
        else
            useLimitsCam = false;
    }

    void VerifiqueLimiteLerp()
    {
        if (pedindoLimiteLerp)
        {
            tempoDecorrido += Time.deltaTime;

            limitantes.xMax = Mathf.Lerp(lerpLimitantesTransitorio.xMax, lerpLimitantes.xMax, tempoDecorrido / tempoDeLerpLimits);
            limitantes.xMin = Mathf.Lerp(lerpLimitantesTransitorio.xMin, lerpLimitantes.xMin, tempoDecorrido / tempoDeLerpLimits);
            limitantes.yMax = Mathf.Lerp(lerpLimitantesTransitorio.yMax, lerpLimitantes.yMax, tempoDecorrido / tempoDeLerpLimits);
            limitantes.yMin = Mathf.Lerp(lerpLimitantesTransitorio.yMin, lerpLimitantes.yMin, tempoDecorrido / tempoDeLerpLimits);

            if (tempoDecorrido > tempoDeLerpLimits)
            {
                pedindoLimiteLerp = false;
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.limitCamOk, limitantes));
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        shake.Update();

        VerifiqueLimiteLerp();

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
