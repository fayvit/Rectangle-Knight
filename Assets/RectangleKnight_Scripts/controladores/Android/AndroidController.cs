using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// CLasse responsável pelo joystick android.
/// O pai deve ser alinhado ao canto enquanto o filho alinhado ao centro do pai
/// </summary>
public class AndroidController : MonoBehaviour
{
    public static AndroidController a;

#pragma warning disable 0649
    [SerializeField] private RectTransform joyImage;
    [SerializeField] private RectTransform pai;
    [SerializeField] private float deslJoy = 10;
#pragma warning restore 0649

    private bool joyClicked = false;
    private int jId = 0;
    private Vector2 anchoredOriginal = Vector2.zero;
    private Vector2 anchoredCentral = Vector2.zero;
    //private Vector3 posClicked = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        a = this;
        anchoredOriginal = pai.anchoredPosition;

        anchoredCentral = joyImage.anchoredPosition;
        faca();
    }

    void faca()
    {
        Button[] B = FindObjectsOfType<Button>();
        foreach (Button UIElement in B)
        {
            EventTrigger trigger = UIElement.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();

            entry.eventID = EventTriggerType.PointerUp;
            entry.callback = new EventTrigger.TriggerEvent();
            UnityEngine.Events.UnityAction<BaseEventData> call = new UnityEngine.Events.UnityAction<BaseEventData>(SolteiBotao);
            entry.callback.AddListener(call);

            trigger.triggers.Add(entry);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (joyClicked)
        {
            CanvasScaler scaler = joyImage.parent.parent.GetComponent<CanvasScaler>();
            Vector2 posDeInput = Vector2.zero;
            if (Input.touchCount > 0)
                posDeInput = Input.GetTouch(jId).position;
            else
                posDeInput = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            Vector3 posAlvo =
                  new Vector2(posDeInput.x * scaler.referenceResolution.x / Screen.width,
                posDeInput.y * scaler.referenceResolution.y / Screen.height) - anchoredOriginal;

            posAlvo = CalculoDoAlvo(posAlvo);
            joyImage.anchoredPosition = posAlvo;
        }

    }

    public Vector3 ValorParaEixos()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        float x = -(anchoredCentral.x - joyImage.anchoredPosition.x) / deslJoy;
        float y = -(anchoredCentral.y - joyImage.anchoredPosition.y) / deslJoy;

        Vector3 retorno = new Vector3(x, 0, y);

        if ((Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0) && pai.gameObject.activeSelf)
            retorno = new Vector3(h, 0, v);

        return retorno;// retorno.magnitude > 1 ? retorno.normalized : retorno;
    }

    Vector2 CalculoDoAlvo(Vector2 posAlvo)
    {
        float x = posAlvo.x;
        float y = posAlvo.y;
        if (Mathf.Abs(anchoredCentral.x - x) > deslJoy)
            x = deslJoy * ((anchoredCentral.x - x) > 0 ? -1 : 1);

        if (Mathf.Abs(anchoredCentral.y - y) > deslJoy)
            y = deslJoy * ((anchoredCentral.y - y) > 0 ? -1 : 1);


        return new Vector2(x, y);
    }


    public void OnJoyTouchEnter(BaseEventData data)
    {
        joyClicked = true;
        jId = ((PointerEventData)data).pointerId;
        //posClicked = Input.mousePosition;

    }

    public void OnJoyTouchExit()
    {
        joyClicked = false;
        joyImage.anchoredPosition = anchoredCentral;
    }


    /// <summary>
    /// Função responsavel por atualizar o id do toque quando soltamos um botão. Necessária pois o android joy 
    /// utiliza o id do toque que também á atualizado quando pressionamos um botão
    /// </summary>
    /// <param name="eventData">O parametro é utilizado para encontrar o id do toque</param>
    public void SolteiBotao(BaseEventData eventData)
    {
        if (jId > ((PointerEventData)eventData).pointerId)
        {
            // print(((PointerEventData)eventData).pointerId);
            jId--;
        }

    }

    public void DesligarControlador()
    {
        pai.gameObject.SetActive(false);
        OnJoyTouchExit();
    }

    public void LigarControlador()
    {
        pai.gameObject.SetActive(true);
    }

    /*
    public void EstouSegurandoOBotao1(BaseEventData eventData)
    {
        Debug.Log(((PointerEventData)eventData).pointerId);
        infos.text = "EstouSegurando o botão 1";
    }

    

    public void EstouSegurandoOBotao2()
    {

        infos.text = "EstouSegurando o botão 2";
    }

    public void SolteiBotao2()
    {
        infos.text = "Soltei2";
    }*/


}
