using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGame : MonoBehaviour
{
    LevelManager levelManager;
    Vehicle vehicle;

    private void OnEnable()
    {
        levelManager = LevelManager.Instance;
        vehicle = levelManager.vehicle;
        transform.position = (vehicle.GetTopPos() + 2) * Vector3.up + (vehicle.GetBackPos() - 3) * Vector3.forward;
    }
}
