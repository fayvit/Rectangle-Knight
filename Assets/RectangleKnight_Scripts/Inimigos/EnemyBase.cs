using UnityEngine;

public class EnemyBase : CharacterBase {
    
    //[SerializeField] private EstadoDePersonagem estado = EstadoDePersonagem.naoIniciado;
    [SerializeField] private string ID;
    [SerializeField] private int premioEmDinheiro = 5;
   
    protected virtual void Start () {

        if (ExistenciaDoController.AgendaExiste(Start, this))
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.enemyDefeatedCheck, ID, gameObject));

            EventAgregator.AddListener(EventKey.sendDamageForEnemy, OnReceivedDamageAmount);
        }
    }

    protected virtual void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.sendDamageForEnemy, OnReceivedDamageAmount);
    }

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    private void OnReceivedDamageAmount(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;

        if (obj.Sender == gameObject)
        {
            Dados.AplicaDano((int)ssge.MyObject[0]);

            if (Dados.PontosDeVida <= 0)
            {
                SpawnMoedas.Spawn(transform.position, premioEmDinheiro);
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeEnemyKey,ID));
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Debug.Log(collision.name + " : " + collision.tag);
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                bool sentidoPositivo = transform.position.x - collision.transform.position.x > 0;
                EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.heroDamage,sentidoPositivo,Dados.AtaqueBasico));
            }
        }

        
        if (collision.tag == "attackCollisor")
        {
            EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.enemyContactDamage,collision.name));
        }
    }



    /*
    // Update is called once per frame
    void Update () {
        switch (estado)
        {
            case EstadoDePersonagem.aPasseio:
                Ia.Update();
            break;
            case EstadoDePersonagem.emDano:
                if (EmDano.Update(transform))
                {

                }
                else
                {
                    estado = EstadoDePersonagem.aPasseio;
                }
            break;
            case EstadoDePersonagem.derrotado:
                tempoDecorrido += Time.deltaTime;
                if (tempoDecorrido > tempoDeDestruicao)
                {
                    RPC_Listener.RPC(NameOfRPC.DestruirEnemyBase, PhotonTargets.Others, MyView.viewID);
                    DestruirEnemyBase();
                }
            break;
        }


        if (barraDeVida.transform.parent.gameObject.activeSelf)
        {
            tempodeBarraDeVidaAtiva -= Time.deltaTime;

            if (tempodeBarraDeVidaAtiva < 0)
                RPC_Listener.RPC(NameOfRPC.SwitchLifeBarView, PhotonTargets.All, MyView.viewID, false, barraDeVida.fillAmount);
                
        }

	}

    public override void TomaDano(IGolpeBase golpe, GameObject atacante)
    {

        VerificaBarraDeVida();
        Dados.AplicaDano(golpe.ValorDeDano);
        barraDeVida.fillAmount = Mathf.Max(0, (float)Dados.PontosDeVida / Dados.MaxVida);
        tempodeBarraDeVidaAtiva = 5;

        RPC_Listener.RPC(NameOfRPC.SwitchLifeBarView, PhotonTargets.All, MyView.viewID, true, barraDeVida.fillAmount);

        Ia.MudeParaAoAtaque(atacante.transform);

        if (_Animator == null)
            _Animator = GetComponent<Animator>();

        if (VerificaDerrota())
        {
            AcoesDeDerrotado();

            RPC_Listener.RPC(NameOfRPC.BasicEnemyDefeated, PhotonTargets.Others,GetComponent<PhotonView>().viewID);
            
            estado = EstadoDePersonagem.derrotado;
        }
        else
        {
            EventAgregator.Publish(EventKey.enemyDamage, new EnemyDamageEvent(this));

            estado = EstadoDePersonagem.emDano;

            if (EmDano == null)
                EmDano = new EstouEmDano(GetComponent<CharacterController>());

            EmDano.Start(transform.position, 0, golpe);
            RPC_Listener.RPC(NameOfRPC.AnimaPlay, PhotonTargets.All, "dano1", GetComponent<PhotonView>().viewID);
        }
    }

    public void OnDefeatedInLan(IGameEvent e)
    {
        StandardSendIntEvent s = e as StandardSendIntEvent;
        if (s.MyInt==MyView.viewID)
        {
            AcoesDeDerrotado();
        }
    }

    public void AcoesDeDerrotado()
    {
        EventAgregator.Publish(EventKey.enemyDefeated, new StandardGameEvent(gameObject, EventKey.enemyDefeated));
        MonoBehaviour.Destroy(MonoBehaviour.Instantiate(PrefabList.Load(PrefabListName.enemyBasicDeathSound)), 3);
        _Animator.Play("cair");
        controle.enabled = false;
    }

    public void DestruirEnemyBase()
    {
        EventAgregator.Publish(EventKey.gameObjectDisabled,new EnemyDamageEvent(this));

        Destroy(
        Instantiate(PrefabList.Load(PrefabListName.destroyEnemyParticle),transform.position,Quaternion.identity),5);


        if(PhotonNetwork.isMasterClient)
            PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider hit)
    {
        
        if (tag=="enemy" && hit.gameObject.tag == "Player" && MyView!=null)
        {
            if(MyView.isMine)
            TomaDano(new IGolpeBase()
            { ValorDeDano = 0,
              DirDeREpulsao = (transform.position - hit.transform.position).normalized,
                DistanciaDeRepulsao = 3,
              VelocidadeDeRepulsao = 15,
              TempoNoDano = 1
            },gameObject
                );
        }
    }

    private void OnDrawGizmos()
    {
        CharacterController controle = GetComponent<CharacterController>();
        Gizmos.DrawWireCube(transform.position+1.1f*transform.forward*controle.radius+controle.center, 
            new Vector3(controle.radius, 0.5f*(controle.height-controle.radius), 0.2f));
    }*/
}
