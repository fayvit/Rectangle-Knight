using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestLocalNameExibition : MonoBehaviour
{
    [SerializeField] private string ID;
    [SerializeField] private bool sempreDiscreto = false;
    [SerializeField] private SceneNamesForExibitions localName = SceneNamesForExibitions.acampamentoDosRejeitados;

    private void Start()
    {
        EventAgregator.AddListener(EventKey.localNameExibition, OnRequestLocalNameExibition);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.localNameExibition, OnRequestLocalNameExibition);
    }

    void OnRequestLocalNameExibition(IGameEvent e)
    {
        Debug.Log("invocado");
        if (GameController.g.MyKeys.VerificaAutoShift(ID))
        {
            GameController.g.LocalName.RequestLocalNameExibition(
                    BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.nomesParaCenarios)[(int)localName], true);
        }
    }

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !GameController.g.MyKeys.VerificaAutoShift(ID))
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                GameController.g.LocalName.RequestLocalNameExibition(
                    BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.nomesParaCenarios)[(int) localName],sempreDiscreto);
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, ID));
                
            }
        }
    }
}
