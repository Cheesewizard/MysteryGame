using System;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // camera will follow this object
    public Transform camera;

    private void LateUpdate()
    {
        // update position
        camera.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
