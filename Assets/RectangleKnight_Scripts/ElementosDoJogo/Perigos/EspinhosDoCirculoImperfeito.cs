using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspinhosDoCirculoImperfeito : MonoBehaviour
{
    public GameObject circulo;
    public GameObject espinhos;
    public GameObject particulaDosEspinhos;

    private void Start()
    {
        circulo = transform.GetChild(0).gameObject;
        espinhos = transform.GetChild(1).gameObject;
        particulaDosEspinhos = transform.GetChild(2).gameObject;
    }
}
