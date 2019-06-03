using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporizedForwardProjectle : MonoBehaviour
{
    [SerializeField] private float vel=10;
    [SerializeField] private float startDelay = 1;
    [SerializeField] private GameObject particula=default;
    [SerializeField] private SoundEffectID somDeImpacto = SoundEffectID.lancaProjetilInimigo;

    private bool iniciado = false;
    public float StartDelay { get { return startDelay; }set { startDelay = value; } }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Iniciar", startDelay);
    }

    void Iniciar()
    {
        iniciado = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(iniciado)
            transform.position += transform.right *vel * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProjetilInimigo.OnTriggerEnterEnemyProjectile(collision, gameObject, particula, somDeImpacto);
    }
}
