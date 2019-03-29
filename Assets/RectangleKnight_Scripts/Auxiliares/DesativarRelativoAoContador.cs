using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesativarRelativoAoContador : MonoBehaviour
{
    [SerializeField] private KeyCont kCont;
    [SerializeField] private int menorQue;
    // Start is called before the first frame update

    void Start()
    {
        if (GameController.g.MyKeys.VerificaCont(kCont) < menorQue)
            gameObject.SetActive(false);
    }
}
