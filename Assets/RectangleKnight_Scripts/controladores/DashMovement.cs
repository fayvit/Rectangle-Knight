using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DashMovement
{
#pragma warning disable 0649
    [SerializeField] private ParticleSystem particulaDoInicio;
    [SerializeField] private ParticleSystem particulaDoMovomento;
    [SerializeField] private AudioClip somDoDash;
#pragma warning restore 0649
    [SerializeField] private float vel = 24;
    [SerializeField] private float tempoNoDash = 0.75f;
    [SerializeField] private float intervaloDeRecuperacao = 2;
    

    private bool esteveNochao = false;
    private float tempoDecorrido = 0;
    private Rigidbody2D m_Rigidbody2D;

    public void IniciarCampos(Transform T)
    {
        tempoDecorrido = 1.1f * intervaloDeRecuperacao;
        m_Rigidbody2D = T.GetComponent<Rigidbody2D>();
    }

    public void RetornarAoEstadoDeEspera()
    {
        tempoDecorrido = 0;
        particulaDoInicio.gameObject.SetActive(false);
        particulaDoMovomento.gameObject.SetActive(false);
    }

    public bool PodeDarDash(bool noChao)
    {
        esteveNochao |= noChao;
        tempoDecorrido += Time.deltaTime;

        return tempoDecorrido > intervaloDeRecuperacao&&esteveNochao;
    }

    // Start is called before the first frame update
    public void Start(float move,bool chao)
    {
        Vector3 S;
        if (move < 0)
        {
            S = particulaDoInicio.transform.localScale;
            S.x = -Mathf.Abs(S.x);
            particulaDoInicio.transform.localScale = S;

            S = particulaDoMovomento.transform.localScale;
            S.x = -Mathf.Abs(S.x);
            particulaDoMovomento.transform.localScale = S;
        }
        else
        {
            S = particulaDoInicio.transform.localScale;
            S.x = Mathf.Abs(S.x);
            particulaDoInicio.transform.localScale = S;

            S = particulaDoMovomento.transform.localScale;
            S.x = Mathf.Abs(S.x);
            particulaDoMovomento.transform.localScale = S;
        }

        particulaDoInicio.gameObject.SetActive(true);
        particulaDoInicio.Play();
        particulaDoMovomento.gameObject.SetActive(true);
        tempoDecorrido = 0;
        esteveNochao = chao;

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, somDoDash));
    }

    // Update is called once per frame
    public bool Update(float move,float comando)
    {
        tempoDecorrido += Time.deltaTime;

        if (tempoDecorrido > tempoNoDash || comando!=move)
        {
            RetornarAoEstadoDeEspera();
            return true;
        }

        float localVel = Mathf.Lerp(vel, 0, 2 * tempoDecorrido / tempoNoDash-1);
        m_Rigidbody2D.velocity = new Vector2(move * localVel,0);

        return false;
    }
}
