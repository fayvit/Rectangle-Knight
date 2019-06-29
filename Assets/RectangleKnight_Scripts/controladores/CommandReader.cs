using UnityEngine;
using System.Collections.Generic;

public static class CommandReader
{
    private static Dictionary<string, bool> zerados = new Dictionary<string, bool>();
    public static bool VerificaUsoDesseControle(int esse)
    {
        bool retorno = false;
        //Input.ResetInputAxes();
        if (Input.GetAxis("joy " + esse + " horizontal") != 0 ||
            Input.GetAxis("joy " + esse + " vertical") != 0 ||
            Input.GetAxis("joy " + esse + " triggers") != 0 ||
            Input.GetAxis("joy " + esse + " Xcam") != 0 ||
            Input.GetAxis("joy " + esse + " Ycam") != 0 ||
            Input.GetAxis("joy " + esse + " HDpad") != 0 ||
            Input.GetAxis("joy " + esse + " VDpad") != 0)
        {
            retorno = true;
        }
        else
        {
            for (int i = 0; i < 20; i++)
            {
                if (Input.GetKey("joystick " + esse + " button " + i))
                    retorno = true;
            }
        }
        return retorno;
    }

    public static bool SubmitButtonDown()
    {
        bool foi = false;
        for (int i = -1; i < System.Enum.GetValues(typeof(Controlador)).Length; i++)
            if (i != 0)
                foi |= SubmitButtonDown((Controlador)i);

        return foi;
    }

    public static bool SubmitButtonDown(Controlador c)
    {
        switch (c)
        {
            case Controlador.teclado:
                return ButtonDown(7, -1) || ButtonDown(0, -1);
            case Controlador.N3DS:
                return Input.GetKeyDown(KeyCode.A);
            default:
                return ButtonDown(0, (int)c);
        }
        /*
        if (c == Controlador.teclado)
            return ButtonDown(7, -1) || ButtonDown(0, -1);
        else if (c == Controlador.N3DS)
            return Input.GetKeyDown(KeyCode.A);
        else
            return ButtonDown(0, (int)c);*/
    }

    public static int InputDoControle()
    {
        int retorno = 0;
        for (int i = 0; i < 4; i++)
        {
            if (VerificaUsoDesseControle(i))
                retorno = i;
        }
        return retorno;
    }

    public static bool PressionadoBotao(int numButton)
    {
        bool foi = false;
        for (int i = -1; i < 5; i++)
            if (i != 0)
                foi |= PressionadoBotao(numButton, i);

        return foi;
    }

    public static bool PressionadoBotao(int numButton, Controlador numControl)
    {
        return PressionadoBotao(numButton, (int)numControl);
    }

    public static bool PressionadoBotao(int numButton, int numControl)
    {
        Controlador c = (Controlador)numControl;
        switch (c)
        {
            case Controlador.teclado:
                return Input.GetButton("joystick " + numControl + " button " + numButton);
            case Controlador.N3DS:
                return Input.GetKey(N3DS_KeysDic.dicKeys[numButton]);
            case Controlador.Android:

                MyButtonEvents b = null;

                if (ControladorDeJoystick.cj!=null)
                    b = ControladorDeJoystick.cj.GetButton(numButton);

                if (b != null)
                    return b.buttonPress;
                else
                    return false;
            default:
                return Input.GetKey((KeyCode)(350 + (numControl - 1) * 20 + numButton));

        }
        /*
        bool retorno = false;
        if (numControl == -1)
            retorno = Input.GetButton("joystick " + numControl + " button " + numButton);
        else if (numControl == (int)Controlador.N3DS)
            retorno = Input.GetKey(N3DS_KeysDic.dicKeys[numButton]);
        else
            retorno = Input.GetKey((KeyCode)(350 + (numControl - 1) * 20 + numButton));
        return retorno;*/
    }

    public static bool ButtonDown(int numButton)
    {
        bool foi = false;

        for (int i = -1; i < 5; i++)
            if (i != 0)
                foi |= ButtonDown(numButton, i);

        return foi;
    }

    public static bool ButtonUp(int numButton, Controlador control)
    {
        return ButtonUp(numButton, (int)control);
    }

    public static bool ButtonUp(int numButton, int numControl)
    {
        Controlador c = (Controlador)numControl;
        switch (c)
        {
            case Controlador.teclado:
                return Input.GetButtonUp("joystick " + numControl + " button " + numButton);
            case Controlador.N3DS:
                return Input.GetKeyUp(N3DS_KeysDic.dicKeys[numButton]);
            case Controlador.Android:
                if (ControladorDeJoystick.cj != null)
                {
                    MyButtonEvents b = ControladorDeJoystick.cj.GetButton(numButton);
                    if (b != null)
                        return b.buttonUp;
                }

                return false;
            default:
                return Input.GetKeyUp((KeyCode)(350 + (numControl - 1) * 20 + numButton));

        }
        /*
        bool retorno = false;
        if (numControl == -1)
            retorno = Input.GetButtonUp("joystick " + numControl + " button " + numButton);
        else if (numControl == (int)Controlador.N3DS)
            retorno = Input.GetKeyUp(N3DS_KeysDic.dicKeys[numButton]);
        else
            retorno = Input.GetKeyUp((KeyCode)(350 + (numControl - 1) * 20 + numButton));
        return retorno;*/
    }

    public static bool ButtonDown(int numButton, Controlador control)
    {
        return ButtonDown(numButton, (int)control);
    }

    public static bool ButtonDown(int numButton, int numControl)
    {
        Controlador c = (Controlador)numControl;
        switch (c)
        {
            case Controlador.teclado:
                return Input.GetButtonDown("joystick " + numControl + " button " + numButton);
            case Controlador.N3DS:
                return Input.GetKeyDown(N3DS_KeysDic.dicKeys[numButton]);
            case Controlador.Android:
                if (ControladorDeJoystick.cj != null)
                {
                    MyButtonEvents b = ControladorDeJoystick.cj.GetButton(numButton);
                    if (b != null)
                        return b.buttonDown;
                    
                    
                }

                return false;
            default:
                return Input.GetKeyDown((KeyCode)(350 + (numControl - 1) * 20 + numButton));

        }
    }

    public static float GetAxis(string esseGatilho, Controlador control)
    {
        return GetAxis(esseGatilho, (int)control);
    }

    public static float GetAxis(string esseGatilho, int numControlador)
    {
        Controlador c = (Controlador)numControlador;
        float val = 0;
        switch (c)
        {
            case Controlador.N3DS:
                val = N3dsAxis.GetAxis(esseGatilho);
            break;
            case Controlador.Android:
                if(ControladorDeJoystick.cj!=null)
                    val = ControladorDeJoystick.cj.GetInputVal(esseGatilho);
            break;
            default:
                val = Input.GetAxisRaw("joy " + numControlador + " " + esseGatilho);
            break;
        }

        return val;
    }


    public static Vector3 VetorDirecao(Controlador control)
    {
        return VetorDirecao((int)control);
    }

    public static Vector3 VetorDirecao(int numControlador)
    {

        float h = 0;
        float v = 0;

        if (numControlador == (int)Controlador.N3DS)
        {
            if (!PressionadoBotao(4, numControlador))
            {
                h = Mathf.Min(N3dsAxis.GetAxis("horizontal") + N3dsAxis.GetAxis("HDpad"), 1);
                v = Mathf.Min(N3dsAxis.GetAxis("vertical") + N3dsAxis.GetAxis("VDpad"), 1);
            }
            else
            {
                h = N3dsAxis.GetAxis("HDpad");
                v = N3dsAxis.GetAxis("VDpad");
            }

        }
        else
        {
            h = Mathf.Clamp(GetAxis("horizontal", numControlador), -1, 1);
            v = Mathf.Clamp(GetAxis("vertical", numControlador), -1, 1);
        }

        Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);//new Vector3(1, 0, 0);
        /*
        if (AplicadorDeCamera.cam != null)
            if (AplicadorDeCamera.cam.Cdir != null)
                forward = AplicadorDeCamera.cam.Cdir.DirecaoInduzida(h, v);*/

        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        //Debug.Log(forward + " : " + right + " :" + h + " : " + v);
        return (h * right + v * forward);

    }

    static void VerificaExisteZerado(string esseGatinlho)
    {
        if (!zerados.ContainsKey(esseGatinlho))
            zerados[esseGatinlho] = true;
    }

    static public int ValorDeGatilhos(string esseGatilho, Controlador control)
    {
        return ValorDeGatilhos(esseGatilho, (int)control);
    }

    static float GatilhoVal(string esseGatilho,int numControlador)
    {
        float val = 0;
        Controlador c = (Controlador)numControlador;

        switch (c)
        {
            case Controlador.N3DS:
                val = N3dsAxis.GetAxis(esseGatilho);
            break;
            case Controlador.Android:
                if (ControladorDeJoystick.cj != null)
                    val = ControladorDeJoystick.cj.GetInputVal(esseGatilho);
            break;                
            default:
                esseGatilho = "joy " + numControlador + " " + esseGatilho;
                val = Input.GetAxisRaw(esseGatilho);
            break;
        }

        return val;
    }

    static public int ValorDeGatilhos(string esseGatilho, int numControlador)
    {
        int retorno = 0;
        float val = GatilhoVal(esseGatilho,numControlador);       

        
        /*
        if (numControlador == (int)Controlador.N3DS)
        {
            val = N3dsAxis.GetAxis(esseGatilho);
        }
        else
        {
            esseGatilho = "joy " + numControlador + " " + esseGatilho;
            val = Input.GetAxisRaw(esseGatilho);
        }*/

        VerificaExisteZerado(esseGatilho);


        if (zerados[esseGatilho])
        {
            if (val != 0)
            {
                zerados[esseGatilho] = false;

            }

            if (val > 0)
                retorno = 1;
            else if (val < 0)
                retorno = -1;

        }
        else
        {

            retorno = 0;
            if (val > -0.1f && val < 0.1f)
                zerados[esseGatilho] = true;

        }


        return retorno;

    }

    static public int ValorDeGatilhosTeclado(string esseGatilho, Controlador control)
    {
        return ValorDeGatilhosTeclado(esseGatilho, (int)control);
    }

    static public int ValorDeGatilhosTeclado(string esseGatilho, int numControlador)
    {
        int retorno = 0;
        float val = GatilhoVal(esseGatilho, numControlador);
        /*
        if (numControlador == (int)Controlador.N3DS)
        {
            val = N3dsAxis.GetAxis(esseGatilho);
        }
        else
        {
            esseGatilho = "joy " + numControlador + " " + esseGatilho;
            val = Input.GetAxisRaw(esseGatilho);
        }*/

        VerificaExisteZerado(esseGatilho);

        if (zerados[esseGatilho])
        {
            if (val != 0)
            {
                zerados[esseGatilho] = false;
            }

            if (val > 0)
                retorno = 1;
            else if (val < 0)
                retorno = -1;

        }
        else
        {

            retorno = 0;
            if (val > -0.01f && val < 0.01f)
                zerados[esseGatilho] = true;

        }

        return retorno;
    }
}

public enum Controlador
{
    teclado = -1,
    nulo = 0,
    joystick1,
    joystick2,
    joystick3,
    joystick4,
    rede,
    N3DS,
    Android
}