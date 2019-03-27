using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    #region inspector
#pragma warning disable 0649
    [SerializeField] private DadosDoJogador dados;
    [SerializeField] private MovimentacaoBasica mov;
    [SerializeField] private AttackManager atk;
    [SerializeField] private EstouEmDano emDano;
    [SerializeField] private MagicAttack magic;
    [SerializeField] private PiscaInvunerabilidade piscaI;
    [SerializeField] private DashMovement dash;
    [SerializeField] private DerrotaDoJogador derrota;
    [SerializeField] private EstadoDePersonagem estado = EstadoDePersonagem.naoIniciado;
    [SerializeField] private GameObject heroParticleDamage;
    [SerializeField] private GameObject enemyParticleDamage;
    [SerializeField] private GameObject particulaDoDescanso;
    [SerializeField] private GameObject particulaDoDanoMortal;
    [SerializeField] private GameObject particulaDoMorrendo;
    [SerializeField] private ParticleSystem particulaSaiuDoDescanso;
#pragma warning restore 0649
    #endregion

    public Controlador Control { get => GlobalController.g.Control;}
    public DadosDoJogador Dados { get => dados; set => dados = value; }
    public EstadoDePersonagem Estado { get => estado; private set => estado = value; }
    public int CorDaEspadaselecionada { get => atk.CorDeEspadaSelecionada; }

    // Start is called before the first frame update
    void Start()
    {
        mov.Iniciar(transform);
        dash.IniciarCampos(transform);

        emDano = new EstouEmDano(GetComponent<Rigidbody2D>());

        EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.changeLifePoints, Dados.PontosDeVida, Dados.MaxVida));
        EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.changeMagicPoints, Dados.PontosDeMana, Dados.MaxMana));

        EventAgregator.AddListener(EventKey.heroDamage, OnHeroDamage);
        EventAgregator.AddListener(EventKey.enemyContactDamage, OnEnemyContactDamage);
        EventAgregator.AddListener(EventKey.curaCancelada, OnCancelCure);
        EventAgregator.AddListener(EventKey.curaDisparada, OnCureInvoke);
        EventAgregator.AddListener(EventKey.requestMagicAttack, OnRequestMagicAttack);
        EventAgregator.AddListener(EventKey.requestDownArrowMagic, OnRequestDownArrowMagic);
        EventAgregator.AddListener(EventKey.colorButtonPressed, OnColorButtonPressed);
        EventAgregator.AddListener(EventKey.requestToFillDates, OnRequestFillDates);
        EventAgregator.AddListener(EventKey.startCheckPoint, OnStartCheckPoint);
        EventAgregator.AddListener(EventKey.checkPointLoad, OnCheckPointLoad);
        EventAgregator.AddListener(EventKey.getCoin, OnGetCoin);
        EventAgregator.AddListener(EventKey.getCoinBag, OnGetCoinBag);
        EventAgregator.AddListener(EventKey.enterPause, OnOpenExternalPanel);
        EventAgregator.AddListener(EventKey.exitPause, OnExitPause);
        EventAgregator.AddListener(EventKey.abriuPainelSuspenso, OnOpenExternalPanel);
        EventAgregator.AddListener(EventKey.fechouPainelSuspenso, OnCloseExternalPanel);
        EventAgregator.AddListener(EventKey.getEmblem, OnGetEmblem);
        EventAgregator.AddListener(EventKey.getHexagon, OnGetHexagon);
        EventAgregator.AddListener(EventKey.getPentagon, OnGetPentagon);
        EventAgregator.AddListener(EventKey.inicializaDisparaTexto, OnOpenExternalPanel);
        EventAgregator.AddListener(EventKey.finalizaDisparaTexto, OnCloseExternalPanel);


        GameController.g.Manager = this;
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.heroDamage, OnHeroDamage);
        EventAgregator.RemoveListener(EventKey.enemyContactDamage, OnEnemyContactDamage);
        EventAgregator.RemoveListener(EventKey.curaCancelada, OnCancelCure);
        EventAgregator.RemoveListener(EventKey.curaDisparada, OnCureInvoke);
        EventAgregator.RemoveListener(EventKey.requestMagicAttack, OnRequestMagicAttack);
        EventAgregator.RemoveListener(EventKey.requestDownArrowMagic, OnRequestDownArrowMagic);
        EventAgregator.RemoveListener(EventKey.colorButtonPressed, OnColorButtonPressed);
        EventAgregator.RemoveListener(EventKey.requestToFillDates, OnRequestFillDates);
        EventAgregator.RemoveListener(EventKey.startCheckPoint, OnStartCheckPoint);
        EventAgregator.RemoveListener(EventKey.checkPointLoad, OnCheckPointLoad);
        EventAgregator.RemoveListener(EventKey.getCoin, OnGetCoin);
        EventAgregator.RemoveListener(EventKey.getCoinBag, OnGetCoinBag);
        EventAgregator.RemoveListener(EventKey.enterPause, OnOpenExternalPanel);
        EventAgregator.RemoveListener(EventKey.exitPause, OnExitPause);
        EventAgregator.RemoveListener(EventKey.abriuPainelSuspenso, OnOpenExternalPanel);
        EventAgregator.RemoveListener(EventKey.fechouPainelSuspenso, OnCloseExternalPanel);
        EventAgregator.RemoveListener(EventKey.getEmblem, OnGetEmblem);
        EventAgregator.RemoveListener(EventKey.getHexagon, OnGetHexagon);
        EventAgregator.RemoveListener(EventKey.getPentagon, OnGetPentagon);
        EventAgregator.RemoveListener(EventKey.inicializaDisparaTexto, OnOpenExternalPanel);
        EventAgregator.RemoveListener(EventKey.finalizaDisparaTexto, OnCloseExternalPanel);
    }

    private void OnGetPentagon(IGameEvent e)
    {
        dados.SomaPentagono();
    }

    private void OnGetHexagon(IGameEvent e)
    {
        dados.SomaHexagono();
    }

    private void OnGetEmblem(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        dados.MeusEmblemas.Add(Emblema.GetEmblem((NomesEmblemas)ssge.MyObject[0]));
    }

    private void OnCloseExternalPanel(IGameEvent e)
    {
        estado = EstadoDePersonagem.aPasseio;
    }

    private void OnOpenExternalPanel(IGameEvent e)
    {
        estado = EstadoDePersonagem.parado;
    }

    private void OnExitPause(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;
        estado = (EstadoDePersonagem)ssge.MyObject[0];
    }

    /*
    private void OnEnterPause(IGameEvent e)
    {
        estado = EstadoDePersonagem.parado;
    }*/

    private void OnGetCoinBag(IGameEvent obj)
    {
        dados.Dinheiro += dados.DinheiroCaido.valor;
        dados.DinheiroCaido.estaCaido = false;
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.changeMoneyAmount, dados.Dinheiro));
    }

    private void OnGetCoin(IGameEvent e)
    {
        Dados.Dinheiro += (int)((StandardSendGameEvent)e).MyObject[0];
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.changeMoneyAmount, Dados.Dinheiro));
    }

    private void OnCheckPointLoad(IGameEvent e)
    {
        particulaDoDescanso.SetActive(true);
        estado = EstadoDePersonagem.inCheckPoint;
    }

    private void OnStartCheckPoint(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        dados.SetarVidaMax();
        dados.SetarManaMax();
        dados.ultimoCheckPoint = new UltimoCheckPoint()
        {
            nomesDasCenas = (NomesCenas[])ssge.MyObject[0],
            Pos = MelhoraPos.NovaPos(e.Sender.transform.position,0.1f)
        };

        SaveDatesManager.SalvarAtualizandoDados();
        
    }

    private void OnRequestFillDates(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        SaveDates S = (SaveDates)ssge.MyObject[0];

        if (S == null)
        {
            Dados = new DadosDoJogador();
            transform.position = new Vector3(-8,-2,0);
        }
        else
        {
            Dados = S.Dados;
            transform.position = S.Posicao;
        }

        particulaDoDanoMortal.SetActive(false);
        particulaDoMorrendo.SetActive(false);
        derrota.DesligarLosangulo();
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.changeMoneyAmount, Dados.Dinheiro));
    }

    private void OnColorButtonPressed(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        if (estado == EstadoDePersonagem.aPasseio)
        {
            atk.ChangeSwirdColor((int)ssge.MyObject[0]);
        }
    }

    private void OnRequestDownArrowMagic(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        Dados.ConsomeMana((int)ssge.MyObject[0]);
        estado = EstadoDePersonagem.downArrowActive;
        magic.InstanciarDownArrow();
        piscaI.Start(5);
        EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.changeMagicPoints, Dados.PontosDeMana, Dados.MaxMana));
    }

    private void OnRequestMagicAttack(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        Dados.ConsomeMana((int)ssge.MyObject[0]);
        magic.InstanciaProjetil(transform.position,Mathf.Sign(transform.localScale.x));
        EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.changeMagicPoints, Dados.PontosDeMana, Dados.MaxMana));
    }

    private void OnCureInvoke(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;
        Dados.ConsomeMana((int)ssge.MyObject[0]);
        Dados.AdicionarVida((int)ssge.MyObject[1]);

        estado = EstadoDePersonagem.aPasseio;

        EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.changeLifePoints, Dados.PontosDeVida, Dados.MaxVida));
        EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.changeMagicPoints, Dados.PontosDeMana, Dados.MaxMana));
    }

    private void OnCancelCure(IGameEvent obj)
    {
        estado = EstadoDePersonagem.aPasseio;
    }

    private void OnEnemyContactDamage(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;

        if ((string)ssge.MyObject[0] == "MagicAttack")
            EventAgregator.Publish(new StandardSendGameEvent(obj.Sender, EventKey.sendDamageForEnemy, Dados.AtaqueMagico));
        else
        {
            EventAgregator.Publish(new StandardSendGameEvent(obj.Sender, EventKey.sendDamageForEnemy, Dados.AtaqueBasico));

            Dados.AdicionarMana(1);
            EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.changeMagicPoints, Dados.PontosDeMana, Dados.MaxMana));


        }

        if ((string)ssge.MyObject[0] == "colisorDoAtaquebaixo")
        {
            
            mov.JumpForce();
        }

        Destroy(
        Instantiate(enemyParticleDamage, obj.Sender.transform.position, Quaternion.identity), 5);
    }

    private void OnHeroDamage(IGameEvent obj)
    {
        if (!piscaI.Invuneravel && Dados.PontosDeVida>0)
        {
            StandardSendGameEvent ssge = (StandardSendGameEvent)obj;

            Dados.AplicaDano((int)ssge.MyObject[1]);
            EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.changeLifePoints, Dados.PontosDeVida, Dados.MaxVida));

            atk.ResetaAttackManager();
            magic.RetornarAoModoDeEspera();
            dash.RetornarAoEstadoDeEspera();
            emDano.Start(transform.position, new Vector3((bool)ssge.MyObject[0] ? -1 : 1, 1, 0));

            if (dados.PontosDeVida > 0)
            {
                piscaI.Start(1);
                estado = EstadoDePersonagem.emDano;
            }
            else
            {
                string nomeCena = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                Debug.Log("cena onde dinehiro caiu: "+nomeCena);
                dados.DinheiroCaido = new DinheiroCaido()
                {
                    valor = dados.Dinheiro,
                    Pos = ssge.Sender.transform.position,
                    estaCaido = true,
                    cenaOndeCaiu = StringParaEnum.ObterEnum <NomesCenas>(nomeCena)
                };

                dados.Dinheiro = 0;
                particulaDoDanoMortal.SetActive(true);
                estado = EstadoDePersonagem.derrotado;
            }
            
            Destroy(
            Instantiate(heroParticleDamage, transform.position, Quaternion.identity), 5);
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        piscaI.Update();
        atk.AttackIntervalUpdate();

        if (Control != Controlador.nulo)
        {
            switch (estado)
            {
                case EstadoDePersonagem.aPasseio:
                    #region aPasseio

                    bool noChao = mov.NoChao;
                    Vector3 V = CommandReader.VetorDirecao(Control);
                    mov.AplicadorDeMovimentos(V, CommandReader.ButtonDown(1, Control),dados.TemDoubleJump);

                    if (CommandReader.ButtonDown(0, Control))
                    {
                        BotaoAtacar();
                    }

                    magic.Update(Control, Dados.PontosDeMana, noChao, dados);

                    if (magic.EmTempoDeRecarga && magic.CustoParaRecarga <= Dados.PontosDeMana)
                    {
                        estado = EstadoDePersonagem.emCura;
                        mov.AplicadorDeMovimentos(Vector3.zero,false,false);
                    }

                    if (dados.TemDash && dash.PodeDarDash(noChao) && CommandReader.ButtonDown(3, Control) )
                    {
                        dash.Start(Mathf.Sign(transform.localScale.x),noChao);
                        estado = EstadoDePersonagem.inDash;
                    }

                    if (CommandReader.ButtonDown(4, Control))
                        atk.ModificouCorDaEspada(-1,dados);
                    else if (CommandReader.ButtonDown(5, Control))
                        atk.ModificouCorDaEspada(1,dados);

                    if (V.z > 0.75f && noChao)
                    {
                        mov.AplicadorDeMovimentos(Vector3.zero, false, false);
                        ActionManager.VerificaAcao();
                    }
                    #endregion
                break;
                case EstadoDePersonagem.emCura:
                    magic.Update(Control,Dados.PontosDeMana,mov.NoChao,dados);
                break;
                case EstadoDePersonagem.emAtk:
                    #region emAtk
                    mov.AplicadorDeMovimentos(CommandReader.VetorDirecao(Control), CommandReader.ButtonDown(1, Control),dados.TemDoubleJump);
                    if (atk.UpdateAttack())
                        estado = EstadoDePersonagem.aPasseio;
                    #endregion
                break;
                case EstadoDePersonagem.emDano:
                    #region emDano
                    if (emDano.Update(mov, CommandReader.VetorDirecao(Control)))
                    {
                        estado = EstadoDePersonagem.aPasseio;
                    }
                    #endregion
                break;
                case EstadoDePersonagem.downArrowActive:
                    #region downArrowActive
                    if (!mov.NoChao)
                    {
                        piscaI.Start(0.5f,4);
                        mov.GravityScaled(250);
                    }
                    else
                    {
                        mov.AplicadorDeMovimentos(Vector3.zero,false,false);
                       if(magic.FinalizaDownArrow(mov.NoChao))
                            estado = EstadoDePersonagem.aPasseio;
                    }
                    #endregion
                break;
                case EstadoDePersonagem.inDash:
                    #region inDash
                    if (dash.Update(Mathf.Sign(transform.localScale.x),Mathf.Sign(CommandReader.VetorDirecao(Control).x)))
                    {
                        estado = EstadoDePersonagem.aPasseio;
                    }

                    if (CommandReader.ButtonDown(0, Control))
                    {
                        dash.RetornarAoEstadoDeEspera();
                        estado = EstadoDePersonagem.aPasseio;
                        BotaoAtacar();
                    }
                    #endregion
                break;
                case EstadoDePersonagem.inCheckPoint:
                    #region inCheckPoint
                    if (Mathf.Abs(CommandReader.VetorDirecao(GlobalController.g.Control).x) > 0.5f)
                    {
                        particulaSaiuDoDescanso.gameObject.SetActive(true);
                        particulaSaiuDoDescanso.Play();
                        particulaDoDescanso.SetActive(false);
                        estado = EstadoDePersonagem.aPasseio;
                    }
                    #endregion
                break;
                case EstadoDePersonagem.derrotado:
                    #region derrotado
                    if (emDano.Update(mov, CommandReader.VetorDirecao(Control)))
                    {
                        mov.AplicadorDeMovimentos(Vector3.zero, false, false);
                        particulaDoMorrendo.SetActive(true);
                        if (derrota.Update())
                        {
                            transform.position = dados.ultimoCheckPoint.Pos;
                            dados.SetarVidaMax();
                            dados.SetarManaMax();
                            SaveDatesManager.SalvarAtualizandoDados(dados.ultimoCheckPoint.nomesDasCenas);
                            SceneLoader.IniciarCarregamento(SaveDatesManager.s.IndiceDoJogoAtualSelecionado,
                                ()=> { estado = EstadoDePersonagem.aPasseio; });
                            estado = EstadoDePersonagem.naoIniciado;
                        }
                    }
                    #endregion
                break;
            }
        }
    }

    public void Pulo()
    {
        mov.Pulo();
    }

    public void BotaoAtacar()
    {
        if (estado == EstadoDePersonagem.aPasseio)
        {

            if (CommandReader.VetorDirecao(Control).z > 0.5f)
            {
                atk.DisparaAtaquePraCima();
            }
            else if (CommandReader.VetorDirecao(Control).z < -0.25f && !mov.NoChao)
            {
                atk.DisparaAtaquePuloPraBaixo();
            }
            else
                atk.DisparaAtaqueComum();
            estado = EstadoDePersonagem.emAtk;
        }
    }
}

public enum EstadoDePersonagem
{
    naoIniciado = -1,
    aPasseio,
    parado,
    emAtk,
    emDano,
    movimentoDeFora,
    derrotado,
    emCura,
    downArrowActive,
    inDash,
    inCheckPoint
}
