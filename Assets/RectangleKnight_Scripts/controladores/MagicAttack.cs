using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MagicAttack
{
    [SerializeField] private int custoParaRecarga = 10;
    [SerializeField] private int pontosQueSeraoCarregados = 25;
    [SerializeField] private int pontosParaMagia = 10;
    [SerializeField] private int custoParaDownArrow = 10;
    [SerializeField] private float tempoParaRecarga = 1.75f;
    [SerializeField] private float coolDownRecharge = .2f;
    [SerializeField] private float coolDownMagic = .75f;
    [SerializeField] private float tempoGroundDownArrow = 0.75f;

#pragma warning disable 0649

    [SerializeField] private GameObject projetil;
    [SerializeField] private GameObject particulaDaCura;
    [SerializeField] private ParticleSystem particulaDoCurou;
    [SerializeField] private GameObject downArrowJumpInGround;
    [SerializeField] private GameObject downArrowJumpCollider;
    [SerializeField] private AudioClip somDisparaMagia;
    [SerializeField] private AudioClip somDoCurou;
    [SerializeField] private AudioClip somDoIniciaCura;
    [SerializeField] private AudioClip somDisparaDownArrow;
    [SerializeField] private AudioClip somDownArrowChao;

#pragma warning restore 0649

    private float tempoEmRecuperacao = 0;
    private float tempoDaParticula = .15f;
    private bool jaTocou = false;

    private float TempoPressionado { get; set; } = 0;

    public bool EmTempoDeRecarga
    {
        get
        {
            return TempoPressionado > tempoDaParticula && tempoEmRecuperacao < 0;
        }
    }

    public int CustoParaRecarga { get => custoParaRecarga; }

    public void RetornarAoModoDeEspera()
    {
        particulaDaCura.SetActive(false);
        TempoPressionado = 0;
        tempoEmRecuperacao = 0;
        downArrowJumpCollider.SetActive(false);
        downArrowJumpInGround.SetActive(false);
    }

    // Update is called once per frame
    public void Update(Controlador controlador,int pontosDeMana,bool noChao,DadosDoJogador dados)
    {

        if (CommandReader.PressionadoBotao(2, controlador) && noChao)
        {
            if(!particulaDaCura.activeSelf)
                tempoEmRecuperacao -= Time.deltaTime;

            if (tempoEmRecuperacao < 0)
            {
                TempoPressionado += Time.deltaTime;

                if (EmTempoDeRecarga && pontosDeMana >= custoParaRecarga)
                {
                    if(!particulaDaCura.activeSelf)
                        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, somDoIniciaCura));

                    particulaDaCura.SetActive(true);
                }
                else if (pontosDeMana < custoParaRecarga)
                {
                    tempoEmRecuperacao = coolDownRecharge;
                    //som de não tem magia
                }

                if (TempoPressionado > tempoParaRecarga)
                {
                    TempoPressionado = 0;
                    tempoEmRecuperacao = coolDownRecharge;
                    particulaDaCura.SetActive(false);
                    VerifiqueRecarga(pontosDeMana);
                }
            }
        }
        else if (!CommandReader.PressionadoBotao(2, controlador))
        {
            tempoEmRecuperacao -= Time.deltaTime;
        }
        

        if (CommandReader.ButtonUp(2, controlador))
        {
            if (dados.TemDownArrowJump && CommandReader.VetorDirecao(controlador).z < -0.25f && !noChao)
            {
                VerifiqueDownArrowJump(pontosDeMana);
            }else if (TempoPressionado > tempoDaParticula)
            {
                tempoEmRecuperacao = coolDownRecharge;
                EventAgregator.Publish(new StandardSendGameEvent(null, EventKey.curaCancelada));
            }
            else
            {
                if (tempoEmRecuperacao <= -1 && dados.TemMagicAttack)
                {
                    VerifiqueMagicAttack(pontosDeMana);
                }
            }

            TempoPressionado = 0;
            particulaDaCura.SetActive(false);
        }
    }

    void VerifiqueRecarga(int pontosDeMana)
    {
        if (pontosDeMana >= custoParaRecarga)
        {
            tempoEmRecuperacao = coolDownRecharge;
            particulaDoCurou.gameObject.SetActive(true);
            particulaDoCurou.Play();
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.curaDisparada, custoParaRecarga,pontosQueSeraoCarregados));
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, somDoCurou));
        }
    }

    void VerifiqueMagicAttack(int pontosDeMana)
    {
        if (pontosDeMana >= pontosParaMagia)
        {
            particulaDoCurou.gameObject.SetActive(true);
            particulaDoCurou.Play();
            EventAgregator.Publish(new StandardSendGameEvent(null, EventKey.requestMagicAttack,pontosParaMagia));
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, somDisparaMagia));
        }
        else
        {
            //som do não tem mana
        }
    }

    void VerifiqueDownArrowJump(int pontosDeMana)
    {
        if (pontosDeMana >= pontosParaMagia)
        {
            particulaDoCurou.gameObject.SetActive(true);
            particulaDoCurou.Play();
            EventAgregator.Publish(new StandardSendGameEvent(null, EventKey.requestDownArrowMagic, custoParaDownArrow));
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, somDisparaDownArrow));
        }
        else
        {
            //som do não tem mana
        }
    }

    

    public bool FinalizaDownArrow(bool chegouNoChao)
    {
        if (chegouNoChao && !jaTocou)
        {
            jaTocou = true;
            downArrowJumpCollider.SetActive(false);
            downArrowJumpInGround.SetActive(true);
            tempoEmRecuperacao = tempoGroundDownArrow;
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, somDownArrowChao));
        }
        else if (chegouNoChao && jaTocou)
        {
            tempoEmRecuperacao -= Time.deltaTime;
            if (tempoEmRecuperacao <= 0)
            {
                tempoEmRecuperacao = coolDownRecharge;
                jaTocou = false;
                downArrowJumpInGround.SetActive(false);
                return true;
            }

        }
        return false;
    }

    public void InstanciarDownArrow()
    {
        downArrowJumpCollider.SetActive(true);
    }

    public void InstanciaProjetil(Vector3 pos,float dir)
    {
        tempoEmRecuperacao = coolDownMagic;
        GameObject G = MonoBehaviour.Instantiate(projetil, pos+Vector3.right*dir, projetil.transform.rotation);
        MonoBehaviour.Destroy(G, 3);
        G.name = "MagicAttack";

        if (dir <= 0)
        {
            G.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, -1));
        }

        MovimentaMagia M = G.AddComponent<MovimentaMagia>();
        M.Iniciar(dir,particulaDoCurou.gameObject);
    }
}
