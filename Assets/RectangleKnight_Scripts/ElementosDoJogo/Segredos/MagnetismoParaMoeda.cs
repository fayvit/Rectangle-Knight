using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetismoParaMoeda : MonoBehaviour
{
    private Transform personagem;
    private  Vector3 vetorDirecao;
    private Rigidbody2D r2;

    // Start is called before the first frame update
    void Start()
    {
        personagem = GameController.g.Manager.transform;
        r2 = GetComponent<Rigidbody2D>();
        vetorDirecao = r2.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, personagem.position) < 100)
        {
            vetorDirecao = Vector3.Lerp(vetorDirecao,
                Vector3.ProjectOnPlane((personagem.position - transform.position), Vector3.forward).normalized
                * (40 - Vector3.Distance(transform.position, personagem.position))
                , Time.deltaTime);

            r2.velocity = vetorDirecao;// * Time.deltaTime);
        }
        else
        {
            vetorDirecao = Vector3.zero;
        }
    }
}
