using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

public class Movimentacao : MonoBehaviour
{

    [SerializeField] private float m_MaxSpeed = 10f;
    [SerializeField] private float m_JumpForce = 400f;
    [SerializeField] private bool m_AirControl = false;
#pragma warning disable 0649
    [SerializeField] private ControladorDeJoystick cj;
    [SerializeField] private MyButtonEvents pulo;
    [SerializeField] private MyButtonEvents esquerda;
    [SerializeField] private MyButtonEvents direita;
    [SerializeField] private UnityEngine.UI.Text txt;
#pragma warning restore 0649
    private Transform m_GroundCheck;
    private Transform m_CeilingCheck;

    private bool m_Grounded;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_Jump;
    private bool m_FacingRight = true;

    const float k_GroundedRadius = .2f;
    const float k_CeilingRadius = .01f;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_GroundCheck = transform.Find("checkGround");
        m_CeilingCheck = transform.Find("topo");
    }



    private void Update()
    {
        if (!m_Jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = Input.GetButtonDown("Jump")||pulo.buttonDown;
        }
    }

    private void FixedUpdate()
    {
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }

        bool crouch = Input.GetKey(KeyCode.LeftControl);
        float h = Input.GetAxis("Horizontal")+(cj.GetInputDirection().x!=0?1* cj.GetInputDirection().magnitude*Mathf.Sign(cj.GetInputDirection().x) :0)
            //+AndroidController.a.ValorParaEixos().x
            +((esquerda.buttonPress)?-1:0)
            +((direita.buttonPress)?1:0)
            ;

        Debug.Log(h+" : "+Input.GetAxis("Horizontal"));
        // Pass all parameters to the character control script.
        Move(h, crouch, m_Jump);
        m_Jump = false;
    }

    public void Move(float move, bool crouch, bool jump)
    {
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius))
            {
                crouch = true;
            }
        }

        if (m_Grounded || m_AirControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            //  move = (crouch ? move * m_CrouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            //    m_Anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }

            if (m_Grounded && jump)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void MeuPulo()
    {
        txt.text = "pulo";
        m_Jump = true;
        StartCoroutine(x());
    }

    IEnumerator x()
    {
        yield return new WaitForSeconds(0.5f);
        txt.text = "neutro";
    }
}

