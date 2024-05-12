using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform PlayerPosition;
    public Vector3 Offset;
    public Vector2 CamOffSet;
    public Camera Camera;
    void Update()
    {
        Camera.transform.position = new Vector3(0, 0, -10);
        Camera.transform.position = PlayerPosition.position + Offset;
    }
}
