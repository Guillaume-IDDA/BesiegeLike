using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour, I_ObjectInstantiate
{
    enum TypeOfWheel
    {
        Front,
        Back
    }

    [SerializeField] TypeOfWheel type;

    public float maxMotorTorque;
    public float maxSteeringAngle;
    WheelCollider col;

    bool gameStarted = false;

    private void Start()
    {
        maxMotorTorque = 600;
        maxSteeringAngle = 45;
        col = GetComponent<WheelCollider>();
    }

    public void FixedUpdate()
    {
        if (type == TypeOfWheel.Front)
        {
            float motor = maxMotorTorque * Input.GetAxis("Vertical");
            float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

            col.steerAngle = steering;
            col.motorTorque = motor;
        }

        if (gameStarted)
        {
            ApplyLocalPositionToVisuals(col);

            if(Input.GetKeyDown(KeyCode.A))
            {
                StopWheel();
            }
        }
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);


        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void OnStartGame()
    {
        gameStarted = true;
        GetComponent<BoxCollider>().enabled = false;
    }

    public void StopWheel()
    {
        col.steerAngle = 0;
        col.motorTorque = 0;
    }
}

