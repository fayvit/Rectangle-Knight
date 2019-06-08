using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PonteAcorrentadaParaMagias : MonoBehaviour
{
    [SerializeField] private GameObject[] restritores = default;
    [SerializeField] private GameObject[] correntes = default;
    [SerializeField] private GameObject particulaDaAcao = default;
    [SerializeField] private EnemyBase oAcorrentado = default;
    [SerializeField] private float rotTarget = -90;
    [SerializeField] private float tempoDaParticulaAteAcao = 0.5f;
    [SerializeField] private float tempoDaRotacao = 1;

    [SerializeField] private string ID;

    private bool iniciarRotacao = false;
    private float tempoDecorrido = 0;

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    private void Start()
    {
        if (GameController.g.MyKeys.VerificaAutoShift(ID))
        {
            enabled = false;
            DestruirRestritores();
            EsconderCorrentes();
            Destroy(oAcorrentado.gameObject);
            transform.rotation = Quaternion.Euler(0, 0, rotTarget);
        }
       EventAgregator.AddListener(EventKey.triggerInfo, OnReceivedTriggerInfo);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.triggerInfo, OnReceivedTriggerInfo);
    }

    void OnReceivedTriggerInfo(IGameEvent e)
    {
        if(e.Sender!=null)
            if (e.Sender.transform.IsChildOf(transform))
            {
                DestruirRestritores();
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom,SoundEffectID.fakeWall));
                particulaDaAcao.SetActive(true);
                new MyInvokeMethod().InvokeNoTempoDeJogo(OnStartRotation, tempoDaParticulaAteAcao);
            }
    }

    void OnStartRotation()
    {
        EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.vinhetinhaDoSite));
        iniciarRotacao = true;
        EsconderCorrentes();
    }

    void EsconderCorrentes()
    {
        for (int i = 0; i < correntes.Length; i++)
        {
            correntes[i].SetActive(false);
        }
    }

    void DestruirRestritores()
    {
        for (int i = 0; i < restritores.Length; i++)
            Destroy(restritores[i]);
    }

    private void Update()
    {
        if (iniciarRotacao)
        {
            tempoDecorrido += Time.deltaTime;
            transform.rotation = Quaternion.Euler(0,0,Mathf.Lerp(0,rotTarget,tempoDecorrido/tempoDaRotacao));

            if (tempoDecorrido > tempoDaRotacao)
            {
                transform.rotation = Quaternion.Euler(0, 0, rotTarget);
                EventAgregator.Publish(EventKey.requestSceneCamLimits);
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, ID));
                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.pedrasQuebrando));
                enabled = false;
            }
        }
    }



}
