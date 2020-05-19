using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour, I_ObjectInstantiate
{
    LevelManager levelManager;
    Vehicle vehicle;
    float force = 300;

    private void Start()
    {
        levelManager = LevelManager.Instance;
        vehicle = levelManager.vehicle;
    }

    public void Explosion(Rigidbody _rb)
    {
        _rb.AddForceAtPosition(-transform.forward*force, transform.position, ForceMode.Impulse);
    }

    public void OnStartGame()
    {
        GetComponent<BoxCollider>().enabled = false;
    }
}
