using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordColorManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private MyButtonEvents[] colorButtons;
    [SerializeField] private Sprite doSelecionado;
    [SerializeField] private Sprite doPadrao;
#pragma warning restore 0649

    private Sprite DoSelecionado { get => doSelecionado; set => doSelecionado = value; }

    // Start is called before the first frame update
    void Start()
    {
        EventAgregator.AddListener(EventKey.colorChanged, OnColorChanged);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.colorChanged, OnColorChanged);
    }

    void OnColorChanged(IGameEvent e)
    {
        StandardSendGameEvent ssge = (StandardSendGameEvent)e;
        for (int i = 0; i < colorButtons.Length; i++)
        {
            if ((int)ssge.MyObject[0] == i)
                colorButtons[i].GetComponent<Image>().sprite = DoSelecionado;
            else
                colorButtons[i].GetComponent<Image>().sprite = doPadrao;

        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < colorButtons.Length;i++)
        {
            if (colorButtons[i].buttonUp)
            {
                EventAgregator.Publish(new StandardSendGameEvent(gameObject, EventKey.colorButtonPressed, i));
            }
        }
    }
}
