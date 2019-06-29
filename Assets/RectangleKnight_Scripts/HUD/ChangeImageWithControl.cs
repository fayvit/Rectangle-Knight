using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImageWithControl : MonoBehaviour
{
    [SerializeField] ImageVsControl[] imgsVsControl = default;
    [SerializeField] Image[] imgs = default;

    [System.Serializable]
    private struct ImageVsControl
    {
        public Controlador control;
        public Sprite S;
        public int indiceDoImage;

        public ImageVsControl(Controlador c, Sprite ss, int indice)
        {
            control = c;
            S = ss;
            indiceDoImage = indice;
        }
    }

    private void OnEnable()
    {
        for (int j = 0; j < imgs.Length; j++)
        {
            for (int i = 0; i < imgsVsControl.Length; i++)
            {
                ImageVsControl ivc = imgsVsControl[i];
                if (GlobalController.g.Control == ivc.control && ivc.indiceDoImage==j)
                {
                    imgs[j].sprite = ivc.S;
                }
            }
        }
    }
}
