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
    [SerializeField] private AudioClip somDoDano;
    [SerializeField] private AudioClip somDoDanoFatal;

#pragma warning restore 0649
    #endregion

    private TeleportDamage tDamage = new TeleportDamage();
    private ExternalPositionRequest positionRequest;

    public Controlador Control { get => GlobalController.g.Control;}
    public DadosDoJogador Dados { get => dados; set => dados = value; }
    public EstadoDePersonagem Estado { get => estado; private set => estado = value; }
    public int CorDaEspadaselecionada { get => atk.CorDeEspadaSelecionada; }

    // Start is called before the first frame update
    void Start()
    {
        positionRequest = new ExternalPositionRequest(transform, mov);
        mov.Iniciar(transform);
        dash.IniciarCampos(transform);

        emDano = new EstouEmDano(GetComponent<Rigidbody2D>());

        EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.starterHudForTest, dados));
        

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
        EventAgregator.AddListener(EventKey.getUpdateGeometry, OnGetUpdateGeometry);
        //EventAgregator.AddListener(EventKey.getPentagon, OnGetPentagon);
        EventAgregator.AddListener(EventKey.inicializaDisparaTexto, OnOpenExternalPanel);
        EventAgregator.AddListener(EventKey.finalizaDisparaTexto, OnCloseExternalPanel);
        EventAgregator.AddListener(EventKey.getNotch, OnGetNotch);
        EventAgregator.AddListener(EventKey.colisorNoQuicavel, OnRequestKick);
        EventAgregator.AddListener(EventKey.requestCharRepulse, OnRequestRepulse);
        EventAgregator.AddListener(EventKey.requestHeroPosition, OnRequestPosition);
        EventAgregator.AddListener(EventKey.getColorSword, OnGetColorSword);
        EventAgregator.AddListener(EventKey.getStamp, OnGetStamp);
        EventAgregator.AddListener(EventKey.getItem, OnGetItem);
        EventAgregator.AddListener(EventKey.colorChanged, OnSwordColorChanged);
        EventAgregator.AddListener(EventKey.getMagicAttack, OnGetMagicAttack);
        EventAgregator.AddListener(EventKey.updateGeometryComplete, OnUpdateGeometryComplete);
        EventAgregator.AddListener(EventKey.allAbilityOn, OnRequestAllAbility);


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
        EventAgregator.RemoveListener(EventKey.getUpdateGeometry, OnGetUpdateGeometry);
      //  EventAgregator.RemoveListener(EventKey.getPentagon, OnGetPentagon);
        EventAgregator.RemoveListener(EventKey.inicializaDisparaTexto, OnOpenExternalPanel);
        EventAgregator.RemoveListener(EventKey.finalizaDisparaTexto, OnCloseExternalPanel);
        EventAgregator.RemoveListener(EventKey.getNotch, OnGetNotch);
        EventAgregator.RemoveListener(EventKey.colisorNoQuicavel, OnRequestKick);
        EventAgregator.RemoveListener(EventKey.requestCharRepulse, OnRequestRepulse);
        EventAgregator.RemoveListener(EventKey.requestHeroPosition, OnRequestPosition);
        EventAgregator.RemoveListener(EventKey.getColorSword, OnGetColorSword);
        EventAgregator.RemoveListener(EventKey.getStamp, OnGetStamp);
        EventAgregator.RemoveListener(EventKey.getItem, OnGetItem);
        EventAgregator.RemoveListener(EventKey.colorChanged, OnSwordColorChanged);
        EventAgregator.RemoveListener(EventKey.getMagicAttack, OnGetMagicAttack);
        EventAgregator.RemoveListener(EventKey.updateGeometryComplete, OnUpdateGeometryComplete);
        EventAgregator.RemoveListener(EventKey.allAbilityOn, OnRequestAllAbility);
    }

    private void OnRequestAllAbility(IGameEvent e)
    {
        dados.TemMagicAttack = true;
        dados.EspadaAzul = true;
        dados.EspadaDourada = true;
        dados.EspadaVerde = true;
        dados.EspadaVermelha = true;
        dados.TemDash = true;
        dados.TemDoubleJump = true;
        dados.TemDownArrowJump = true;

    }

    private void OnUpdateGeometryComplete(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        if ((bool)ssge.MyObject[0])
        {
            dados.PentagonosCompletados++;
            dados.MaxMana = dados.BaseMaxMana + dados.PentagonosCompletados * dados.AddMagicBarAmount;
            dados.PontosDeMana = dados.MaxMana;
            dados.PartesDePentagonosObtidas = 0;
            
        }
        else
        {
            dados.PartesDeHexagonoObtidas = 0;
            dados.HexagonosCompletados++;
            dados.MaxVida = dados.BaseMaxLife + dados.HexagonosCompletados * dados.AddLifeBarAmount;
            dados.PontosDeVida = dados.MaxVida;
        }

        EventAgregator.Publish(new StandardSendGameEvent(EventKey.starterHudForTest, dados));
    }

    private void OnGetMagicAttack(IGameEvent e)
    {
        dados.TemMagicAttack = true;
    }

    private void OnSwordColorChanged(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        dados.CorDeEspadaSelecionada = (SwordColor)((int)ssge.MyObject[0]);
    }

    private void OnGetItem(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        ItemBase.AddItem(dados,(NomeItem)ssge.MyObject[0],(int)ssge.MyObject[1]);
    }

    private void OnGetStamp(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        dados.PegouSelo((SeloPositivista.TipoSelo)ssge.MyObject[0]);
    }

    private void OnGetColorSword(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        Dados.GetSword((SwordColor)ssge.MyObject[0]);
    }

    private void OnRequestPosition(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        estado = EstadoDePersonagem.movimentoRequerido;

        positionRequest.RequererMovimento(ssge.Sender,(Vector3)ssge.MyObject[0],4);

        atk.ResetaAttackManager();
        magic.RetornarAoModoDeEspera();
        dash.RetornarAoEstadoDeEspera();

    }

    private void OnRequestRepulse(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;
        mov.ApplyForce((Vector3)ssge.MyObject[0],(float)ssge.MyObject[1]);
    }

    private void OnRequestKick(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        if ((string)ssge.MyObject[0] == "colisorDoAtaquebaixo")
        {

            mov.JumpForce();
        }
    }

    private void OnGetNotch(IGameEvent e)
    {
        dados.EspacosDeEmblemas++;
    }

    /*
    private void OnGetPentagon(IGameEvent e)
    {
        dados.SomaPentagono();
    }*/

    private void OnGetUpdateGeometry(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        HexagonoColetavelBase.DadosDaGeometriaColetavel d = (HexagonoColetavelBase.DadosDaGeometriaColetavel)ssge.MyObject[0];

        OnOpenExternalPanel(null);

        if (d.ePentagono)
            dados.SomaPentagono();
        else
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
        RetornarComponentesAoPAdrao();
        mov.AplicadorDeMovimentos(Vector3.zero);        
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
        //dados.SetarManaMax();
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
        atk.ChangeSwirdColor((int)dados.CorDeEspadaSelecionada); 
        /*
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.changeMoneyAmount, Dados.Dinheiro));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.changeLifePoints, Dados.PontosDeVida,Dados.MaxVida));
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.changeMagicPoints, Dados.PontosDeMana, Dados.MaxMana));
        */
        EventAgregator.Publish(EventKey.colorSwordShow);
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
        if (dados.PontosDeVida > 0)
        {
            StandardSendGameEvent ssge = (StandardSendGameEvent)obj;
            Dados.ConsomeMana((int)ssge.MyObject[0]);
            Dados.AdicionarVida((int)ssge.MyObject[1]);

            estado = EstadoDePersonagem.aPasseio;

            EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.changeLifePoints, Dados.PontosDeVida, Dados.MaxVida));
            EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.changeMagicPoints, Dados.PontosDeMana, Dados.MaxMana));
        }
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
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestShakeCam,ShakeAxis.z,2,1f));

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
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;

        if ((!piscaI.Invuneravel||ssge.MyObject.Length>2) && Dados.PontosDeVida>0)
        {
            

            Dados.AplicaDano((int)ssge.MyObject[1]);
            EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.changeLifePoints, Dados.PontosDeVida, Dados.MaxVida));
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestShakeCam,ShakeAxis.y, 3,1f));

            RetornarComponentesAoPAdrao();
            emDano.Start(transform.position, new Vector3((bool)ssge.MyObject[0] ? -1 : 1, 1, 0));

            if (dados.PontosDeVida > 0)
            {
                if (GameController.g.MyKeys.VerificaAutoShift("equiped_" + NomesEmblemas.suspiroLongo))
                    piscaI.Start(2);
                else
                    piscaI.Start(1);

                estado = EstadoDePersonagem.emDano;
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom,somDoDano));

                if (ssge.MyObject.Length > 2)
                {
                    tDamage.agendado = true;
                    tDamage.pos = (Vector3)ssge.MyObject[2];
                }
            }
            else
            {
                EventAgregator.Publish(EventKey.requestHideControllers, null);
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestShakeCam,ShakeAxis.x,20,1f));
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, somDoDanoFatal));

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

    void RetornarComponentesAoPAdrao()
    {
        atk.ResetaAttackManager();
        magic.RetornarAoModoDeEspera();
        dash.RetornarAoEstadoDeEspera();
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

                    atk.UpdateAttack();

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
                    if (!mov.NoChao)
                        mov.AplicadorDeMovimentos(CommandReader.VetorDirecao(Control), CommandReader.ButtonDown(1, Control), dados.TemDoubleJump);
                    else
                        mov.AplicadorDeMovimentos(Vector3.Lerp(mov.Velocity.normalized,Vector3.zero,30*Time.deltaTime));

                    if (atk.UpdateAttack())
                        estado = EstadoDePersonagem.aPasseio;
                    #endregion
                break;
                case EstadoDePersonagem.emDano:
                    #region emDano
                    if (emDano.Update(mov, CommandReader.VetorDirecao(Control)))
                    {
                        if (tDamage.agendado)
                        {
                            mov.AplicadorDeMovimentos(Vector3.zero);
                            estado = EstadoDePersonagem.parado;
                            tDamage.Iniciar();
                        }
                        else
                        {
                            estado = EstadoDePersonagem.aPasseio;
                        }
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
                        EventAgregator.Publish(EventKey.checkPointExit,null);
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
                            //dados.SetarManaMax();

                            GameController.g.MyKeys.ReviverInimigos();

                            SaveDatesManager.SalvarAtualizandoDados(dados.ultimoCheckPoint.nomesDasCenas);
                            SceneLoader.IniciarCarregamento(SaveDatesManager.s.IndiceDoJogoAtualSelecionado,
                                ()=> {
                                    estado = EstadoDePersonagem.aPasseio;
                                    EventAgregator.Publish(EventKey.requestShowControllers, null);
                                    derrota.DesligarLosangulo();
                                });
                            estado = EstadoDePersonagem.naoIniciado;
                        }
                    }
                    #endregion
                break;
                case EstadoDePersonagem.movimentoRequerido:
                    if (positionRequest.UpdateMove())
                    {
                        mov.AplicadorDeMovimentos(Vector3.zero);
                        estado = EstadoDePersonagem.parado;
                        EventAgregator.Publish(new StandardGameEvent(positionRequest.Requisitor, EventKey.positionRequeredOk));
                    }
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
        if (estado == EstadoDePersonagem.aPasseio && atk.IniciarAtaqueSePodeAtacar())
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
    movimentoRequerido,
    derrotado,
    emCura,
    downArrowActive,
    inDash,
    inCheckPoint
}
