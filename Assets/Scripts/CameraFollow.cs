
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform PlayerPosition;
    public Vector3 Offset;
    public Camera Camera;
    void Update()
    {
        // Camera.transform.position = new Vector3(0, 0, -10);
        Camera.transform.position = PlayerPosition.position + Offset;
    }
}
