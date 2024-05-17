using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform PlayerPosition;
    public Vector3 Offset;

    private Camera Camera;

    private void Start()
    {
        Camera = GetComponent<Camera>();
    }
    void Update()
    {
        // Camera.transform.position = new Vector3(0, 0, -10);
        if (PlayerPosition != null)
        {
            Camera.transform.position = PlayerPosition.position + Offset;
        }
    }
}
