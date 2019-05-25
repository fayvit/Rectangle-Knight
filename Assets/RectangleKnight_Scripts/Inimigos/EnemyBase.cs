using System.Collections;
using UnityEngine;

public class EnemyBase : CharacterBase {
    
    //[SerializeField] private EstadoDePersonagem estado = EstadoDePersonagem.naoIniciado;
    
    [SerializeField] private int premioEmDinheiro = 5;
    [SerializeField] private SoundEffectID damageEffect = SoundEffectID.Damage3;

    protected virtual void Start()
    {
        EventAgregator.AddListener(EventKey.sendDamageForEnemy, OnReceivedDamageAmount);
    }

    protected virtual void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.sendDamageForEnemy, OnReceivedDamageAmount);
    }

    IEnumerator SomDoDano()
    {
        yield return new WaitForSeconds(.2f);
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, damageEffect));
    }

    protected virtual void OnReceivedDamageAmount(IGameEvent obj)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)obj;

        if (obj.Sender == gameObject)
        {

            GameController.g.StartCoroutine(SomDoDano());

            AplicaDano((int)ssge.MyObject[0]);

            /*
            Dados.AplicaDano();

            if (Dados.PontosDeVida <= 0)
            {
                SpawnMoedas.Spawn(transform.position, premioEmDinheiro);
                
                Destroy(gameObject);
            }*/
        }
    }

    protected void AplicaDano(int quanto)
    {
        Dados.AplicaDano(quanto);

        if (Dados.PontosDeVida <= 0)
        {
            OnDefeated();
        }
    }

    protected virtual void OnDefeated()
    {
        SpawnMoedas.Spawn(transform.position, premioEmDinheiro);
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                bool sentidoPositivo = transform.position.x - collision.transform.position.x > 0;
                EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.heroDamage,sentidoPositivo,Dados.AtaqueBasico));
            }
        }

        //Debug.Log(collision.name + " : " + collision.tag);

        if (collision.tag == "attackCollisor")
        {
            EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.enemyContactDamage,collision.name));
        }

    } 
}
