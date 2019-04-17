using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosanguloManager : MonoBehaviour
{
    #region inspector
    [SerializeField] private Sprite spriteAmarelo = default;
    [SerializeField] private GameObject particulaDaConfirmacao = default;
    [SerializeField] private GameObject particulaPoeira = default;
    [SerializeField] private CofreDosLosangulos[] cofres = default;
    #endregion

    public static LosanguloManager l;

    public GameObject ParticulaPoeira { get => particulaPoeira; }

    //private SouUmLosanguloGerenciavel[] losangulos;

    // Start is called before the first frame update
    void Start()
    {
        l = this;
        //losangulos = FindObjectsOfType<SouUmLosanguloGerenciavel>();

        //Debug.Log(losangulos.Length);

        //GameController.g.MyKeys.MudaCont(KeyCont.losangulosPegos, 4);

        Invoke("ColocaLosangulosConfirmados",0.1f);

        EventAgregator.AddListener(EventKey.cofreRequisitado, OnRequestSafeBox);
        
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.cofreRequisitado, OnRequestSafeBox);
    }

    void OnRequestSafeBox(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;

        VerifiqueConfirmacao(ssge.Sender);
    }

    void VerifiqueConfirmacao(GameObject qual)
    {
        CofreDosLosangulos c = default;
        for (int i = 0; i < cofres.Length; i++)
        {
            if (qual == cofres[i].gameObject)
            {
                Debug.Log("indice do cofre é: " + i);
                c = cofres[i];
            }
        }

        KeyVar myKeys = GameController.g.MyKeys;
        int sum = 0;

        for (int i = myKeys.VerificaCont(KeyCont.losangulosConfirmados); i < myKeys.VerificaCont(KeyCont.losangulosPegos); i++)
        {
            if (i >= c.InicioDeAcao-1 && i < c.FinalDeAcao)
            {
                
                SouUmLosanguloGerenciavel s = transform.GetChild(i).GetComponent<SouUmLosanguloGerenciavel>();
                s.MySprite.sprite = spriteAmarelo;

                Debug.Log("filho " + i+" :"+s.name);

                GameObject G = Instantiate(particulaDaConfirmacao, s.transform.position, Quaternion.identity);
                G.SetActive(true);
                Destroy(G, 5);

                SpawnMoedas.Spawn(s.transform.position, Mathf.Max(5, i));
                sum++;
            }
        }

        if (sum > 0)
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.somParaGetLosangulo));
            new MyInvokeMethod().InvokeNoTempoDeJogo(() =>
            {
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.VariasMoedas));
            }, .35f);
        }

        myKeys.SomaCont(KeyCont.losangulosConfirmados, sum);

        Debug.Log(myKeys.VerificaCont(KeyCont.losangulosConfirmados)+" confirmados");
    }

    void ColocaLosangulosConfirmados()
    {
        for (int i = 0; i < GameController.g.MyKeys.VerificaCont(KeyCont.losangulosConfirmados); i++)
        {
            transform.GetChild(i).GetComponent<SouUmLosanguloGerenciavel>().MySprite.sprite = spriteAmarelo;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
