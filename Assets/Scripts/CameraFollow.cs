using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 Offset;

    private Camera Camera;
    private Transform PlayerPosition;

    private void Start()
    {
        PlayerPosition = GameObject.FindGameObjectsWithTag("Player")[0].transform;
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
