using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FalseWall : MonoBehaviour
{
    [SerializeField] private string ID;
    [SerializeField] private GameObject particulaDaAcao = default;
    [SerializeField] private Tilemap myTile;
    [SerializeField] private TilemapCollider2D myCollider;

    private float tempoDecorrido = 0;
    private EstadoDaqui estado = EstadoDaqui.emEspera;

    private const float TEMPO_DA_DESTRUICAO = 0.5F;

    private enum EstadoDaqui
    {
        emEspera,
        fade
        
    }

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ExistenciaDoController.AgendaExiste(Start, this))
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.destroyShiftCheck, ID, gameObject));
        }

        if(myCollider==null)
            myCollider = GetComponent<TilemapCollider2D>();

        if(myTile==null)
            myTile = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (estado)
        {
            case EstadoDaqui.fade:
                tempoDecorrido += Time.deltaTime;
                if (tempoDecorrido < TEMPO_DA_DESTRUICAO)
                {
                    Color C = myTile.color;
                    myTile.color = new Color(C.r, C.g, C.b, (TEMPO_DA_DESTRUICAO - tempoDecorrido) / TEMPO_DA_DESTRUICAO);
                }
                else
                    Destroy(gameObject);
            break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "attackCollisor")
        {
            estado = EstadoDaqui.fade;
            tempoDecorrido = 0;
            myCollider.enabled = false;
            particulaDaAcao.SetActive(true);
            Destroy(particulaDaAcao, 5);
            //Destroy(gameObject);
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, ID));
        }
    }
}
