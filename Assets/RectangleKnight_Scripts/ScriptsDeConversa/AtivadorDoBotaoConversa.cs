using UnityEngine;
using System.Collections;

public class AtivadorDoBotaoConversa : AtivadorDeBotao
{
    [SerializeField] protected NPCdeConversa npc;
    [SerializeField] private NameMusicaComVolumeConfig nameMusic = new NameMusicaComVolumeConfig()
    {
        Musica = NameMusic.nula,
        Volume = 1
    };
    //private Vector3 forwardInicialDoBotao;

    // Use this for initialization
    protected void Start()
    {
        npc.Start(transform);
       // forwardInicialDoBotao = btn.transform.parent.forward;
        SempreEstaNoTrigger();
    }

    new protected void Update()
    {
        base.Update();

        /*
        if (btn.activeSelf)
            btn.transform.parent.forward = forwardInicialDoBotao;*/

        if (npc.Update())
        {
            EventAgregator.Publish(EventKey.returnRememberedMusic,null);
            //GameController.g.Manager.AoHeroi();
        }
    }

    void BotaoConversa()
    {
        if (nameMusic.Musica != NameMusic.nula)
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.changeMusicWithRecovery, nameMusic));
        }
        FluxoDeBotao();

        //Transform T = TransformPosDeConversa.MeAjude(transform);

        npc.IniciaConversa();

        //AplicadorDeCamera.cam.InicializaCameraExibicionista(T, 1, true);
    }

    public override void FuncaoDoBotao()
    {
        BotaoConversa();
    }
}
