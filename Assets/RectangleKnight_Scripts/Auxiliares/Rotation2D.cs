using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation2D
{
    public static Quaternion GetRotation(Vector3 rightForward)
    {
        return Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward,rightForward));
    }

    public static Quaternion GetRotation(Vector3 A, Vector3 B)
    {
        return GetRotation(B - A);
    }
}
