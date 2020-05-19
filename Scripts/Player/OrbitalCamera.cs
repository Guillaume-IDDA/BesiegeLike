using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    Transform CenterOfWorld;

    private float yaw;
    private float pitch;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        yaw = 0.0f;
        pitch = 0.0f;
    }

    private void Update()
    {
        transform.Translate(0, 0, Input.mouseScrollDelta.y);
        if (Input.GetMouseButton(1))
        {
            pitch -= Input.GetAxis("Mouse Y") * 5;
            yaw += Input.GetAxis("Mouse X") * 5;

            pitch = Mathf.Clamp(pitch, -85.0f, 85.0f);

            Vector3 rotation = new Vector3(pitch, yaw, 0);

            transform.parent.rotation = Quaternion.Euler(rotation);
        }

        transform.LookAt(LevelManager.Instance.vehicle.GetGlobalView());
    }


}
