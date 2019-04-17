using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacoDeDinheiro : MonoBehaviour
{
#pragma warning disable 0649 
    [SerializeField] private GameObject particulaDaPegada;
#pragma warning restore 0649 

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 startMovePosition;
    private float tempoDecorrido = 0;
    private float tempoDeDeslocamento = 1;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        
        SetarTargetPosition();
    }

    void SetarTargetPosition()
    {
        startMovePosition = transform.position;

        if (targetPosition != startPosition)
            targetPosition = startPosition;
        else
        {
            Vector3 dir = Random.insideUnitSphere;
            dir = new Vector3(dir.x, dir.y, 0);
            dir.Normalize();

            targetPosition = startPosition + 0.5f * dir;
        }
    }

    // Update is called once per frame
    void Update()
    {
        tempoDecorrido += Time.deltaTime;
        transform.position = Vector3.Slerp(startMovePosition,targetPosition,tempoDecorrido/tempoDeDeslocamento);

        if (tempoDecorrido > tempoDeDeslocamento)
        {
            tempoDecorrido = 0;
            SetarTargetPosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.getCoinBag));

                particulaDaPegada.SetActive(true);

                Destroy(
                Instantiate(particulaDaPegada, transform.position, Quaternion.identity),5);
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.VariasMoedas));
                Destroy(gameObject);
            }
        }
    }
}
