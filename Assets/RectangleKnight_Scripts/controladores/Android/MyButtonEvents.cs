using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class MyButtonEvents : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
   // [SerializeField] private Text txt;
    public bool buttonDown = false;
    public bool buttonUp = false;
    public bool buttonPress = false;

    IEnumerator Falsear(bool down)
    {
        yield return new WaitForEndOfFrame();
        

        if (down)
            buttonDown = false;
        else
            buttonUp = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonDown = true;
        buttonPress = true;
        StartCoroutine(Falsear(true));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonUp = true;
        buttonPress = false;
        StartCoroutine(Falsear(false));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      //  txt.text = "buttonDown = " + buttonDown + " /n/r buttonUp = " + buttonUp+" buttonPress = "+buttonPress;
    }
}
