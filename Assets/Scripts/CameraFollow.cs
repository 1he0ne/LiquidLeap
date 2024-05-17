using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 Offset;

    private Camera Camera;
    private Transform PlayerPosition;

    private void Start()
    {
        PlayerPosition = GameObject.Find("Player").transform;
        Camera = GetComponent<Camera>();
    }
    void Update()
    {
        if (PlayerPosition != null)
        {
            Camera.transform.position = PlayerPosition.position + Offset;
        }
    }
}
