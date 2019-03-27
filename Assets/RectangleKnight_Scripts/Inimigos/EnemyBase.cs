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



    
}
