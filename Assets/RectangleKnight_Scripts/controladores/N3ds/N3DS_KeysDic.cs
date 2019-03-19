using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N3DS_KeysDic
{

    public static readonly Dictionary<int, KeyCode> dicKeys = new Dictionary<int, KeyCode>()
    {
        { 0,KeyCode.A},
        { 1,KeyCode.B},
        { 2,KeyCode.Y},
        { 3,KeyCode.X},
        { 4,KeyCode.L},
        { 5,KeyCode.R},
        { 6,KeyCode.Escape},
        { 7,KeyCode.Return},
        { 8,KeyCode.LeftAlt},
        { 9,KeyCode.RightAlt}
    };

    public static readonly Dictionary<string, ValAxis> dicAxis = new Dictionary<string, ValAxis>()
    {
        { "horizontal",new ValAxis(KeyCode.Keypad6,KeyCode.Keypad4) },
        { "vertical",new ValAxis(KeyCode.Keypad8,KeyCode.Keypad2)},
        { "Xcam",new ValAxis(KeyCode.Alpha4,KeyCode.Alpha3)},
        { "Ycam",new ValAxis(KeyCode.Alpha2,KeyCode.Alpha1)},
        {"HDpad",new ValAxis(KeyCode.RightArrow,KeyCode.LeftArrow) },
        {"VDpad",new ValAxis(KeyCode.UpArrow,KeyCode.DownArrow) }
    };
}

public struct ValAxis
{
    public KeyCode pos;
    public KeyCode neg;

    public ValAxis(KeyCode pos, KeyCode neg)
    {
        this.pos = pos;
        this.neg = neg;
    }
}
