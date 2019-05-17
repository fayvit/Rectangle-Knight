using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlavancaDaGrade : MonoBehaviour
{
    [SerializeField] private GameObject grade = default;
    [SerializeField] private GameObject particulaDaAcao = default;
    [SerializeField] private string ID;
    [SerializeField] private float tempoDaAlavancaAaGrade = .5f;
    [SerializeField] private float tempoDaParticulaAaGrade = .5f;

    private Animator animador;

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    // Start is called before the first frame update
    void Start()
    {
        animador = GetComponent<Animator>();

        if (ExistenciaDoController.AgendaExiste(Start, this))
        {
            if (GameController.g.MyKeys.VerificaAutoShift(ID))
            {
                animador.SetTrigger("final");
                grade.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "attackCollisor" && !GameController.g.MyKeys.VerificaAutoShift(ID))
        {
            EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.Fire1));
            animador.SetTrigger("ativou");
            new MyInvokeMethod().InvokeNoTempoDeJogo(() => {

                EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.Wind1));
                InstanciaLigando.Instantiate(particulaDaAcao, grade.transform.position, 5);

                new MyInvokeMethod().InvokeNoTempoDeJogo(() =>
                {
                    grade.SetActive(false);
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.disparaSom, SoundEffectID.rockFalseAttack));
                    EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestChangeShiftKey, ID));
                }, tempoDaParticulaAaGrade);
            }, tempoDaAlavancaAaGrade);
        }
    }
}
