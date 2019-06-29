using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScriptTestador : MonoBehaviour
{
    /*
    [SerializeField] private GameController g;
    [SerializeField] private bool foi;
    private void Update()
    {
        if (foi)
        {
            g.MenuU.CopyMyVector();
            foi = false;
        }
    }

    */
    AreaEffector2D a2d;
    Rigidbody2D r2;
    bool foi = false;
    // Start is called before the first frame update
    void Start()
    {
        a2d = GetComponent<AreaEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (foi)
            a2d.forceMagnitude = Mathf.Min(Mathf.Lerp(a2d.forceMagnitude, 30 * r2.velocity.x, 5f * Time.deltaTime),600);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            foi = true;

            if (r2 == null)
                r2 = collision.GetComponent<Rigidbody2D>();

            a2d.forceMagnitude = Mathf.Sign(r2.velocity.x)*Mathf.Min(30*Mathf.Abs(r2.velocity.x),600);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            foi = false;
        }
    }
}
