using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmblemaColetavel : ActiveFalseForShift
{
    #region inspector
    [SerializeField] private NomesEmblemas nome = NomesEmblemas.nulo;
    [SerializeField] private PainelUmaMensagem painelEmblema = default;
    [SerializeField] private Text descricaoDoEmblema = default;
    [SerializeField] private Image imgDoEmblema = default;
    [SerializeField] private GameObject particulaDaColeta = default;
    #endregion



    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (UnicidadeDoPlayer.Verifique(collision))
            {
                Time.timeScale = 0;
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, ID));
                EventAgregator.Publish(EventKey.abriuPainelSuspenso, null);
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.getEmblem,nome));

                GetComponent<SpriteRenderer>().enabled = false;
                Destroy(GetComponent<Collider2D>());

                int idDoEmblema = (int)nome;
                painelEmblema.ConstroiPainelUmaMensagem(DeuOkNoPainel,
                    BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.emblemasTitle)[idDoEmblema]);
                descricaoDoEmblema.text = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.emblemasInfo)[idDoEmblema];

                Texture2D t2d = (Texture2D)Resources.Load(nome.ToString());
                Sprite S = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), t2d.texelSize);

                imgDoEmblema.sprite = S;


            }
        }
    }

    private void DeuOkNoPainel()
    {
        if (!GameController.g.MyKeys.VerificaAutoShift(KeyShift.pegouPrimeiroEmblema.ToString()))
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, KeyShift.pegouPrimeiroEmblema.ToString()));
            EventAgregator.Publish(new SendMethodEvent(EventKey.requestInfoEmblemPanel, DeuOkNoPainel));
        }
        else
        {
            Time.timeScale = 1;
            EventAgregator.Publish(EventKey.fechouPainelSuspenso, null);
        }
    }
}
