using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField]
    public Transform player; 
    public Vector3 offset = Vector3.zero; // Additional offset if needed

    private Camera cam;

    void Update()
    {
        cam = GetComponent<Camera>();

        if (cam.orthographic)
        {
            cam.orthographicSize = GlobalGameManager.Instance.orthographicSize; // Set the orthographic size
        }
    }

    void LateUpdate()
    {
        // Set the camera position directly above the player
        transform.position = player.position + new Vector3(0f, 0f, -10f) + offset; // Use a fixed Z position
    }
}
