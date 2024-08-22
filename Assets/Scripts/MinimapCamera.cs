using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public GameObject player;
    
    void Update()
    {
        Vector3 newPosition = player.transform.position;
        newPosition.z = transform.position.z; // Keep the z position of the minimap camera
        transform.position = newPosition;
    }

}
