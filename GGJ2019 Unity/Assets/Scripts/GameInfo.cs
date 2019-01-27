using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : Singleton<GameInfo>
{
    public float Y_Plane;
    public Transform TrunkStartAnchor;
    public Vector3 MainDirection;

    public Vector3 MouseWorldPosition;

    public float DistanceFromCamera = 2;

    public void LateUpdate()
    {
        MouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, DistanceFromCamera));
    }



}
