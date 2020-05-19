using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Vehicle : MonoBehaviour, I_ObjectInstantiate
{
    [SerializeField] Transform center;

    float frontItem = 0;
    float backItem = 0;
    float rightItem = 0;
    float leftItem =0;
    float upItem =0;
    float downItem =0;

    public void OnStartGame()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = NbOfChildrens() * 100;
    }

    private int NbOfChildrens()
    {
        GameObject[] objects = GetComponentsInChildren<I_ObjectInstantiate>().Select(x => (x as MonoBehaviour).gameObject).ToArray();

        return objects.Length;
    }

    public Vector3 GetGlobalView()
    {
        GameObject[] objects = GetComponentsInChildren<I_ObjectInstantiate>().Select(x => (x as MonoBehaviour).gameObject).ToArray();

        int nbOfChildren = objects.Length;

        Vector3 globalView = Vector3.zero;

        for (int i = 0; i < objects.Length; i++)
        {
            globalView += objects[i].transform.position - center.position;

            //stock cube at which is the most of the right
            if(objects[i].transform.position.x > rightItem)
            {
                rightItem = objects[i].transform.position.x;
            }

            //stock cube at which is the most of the left
            if (objects[i].transform.position.x < leftItem)
            {
                leftItem = objects[i].transform.position.x;
            }

            //stock cube at which is the most of the top
            if (objects[i].transform.position.y > upItem)
            {
                upItem = objects[i].transform.position.y;
            }

            //stock cube at which is the most of the down
            if (objects[i].transform.position.y < downItem)
            {
                downItem = objects[i].transform.position.y;
            }

            //stock cube at which is the most of the front
            if (objects[i].transform.position.z > frontItem)
            {
                frontItem = objects[i].transform.position.z;
            }

            //stock cube at which is the most of the back
            if (objects[i].transform.position.z < backItem)
            {
                backItem = objects[i].transform.position.z;
            }

        }

        globalView /= nbOfChildren;

        return globalView;
    }

    public float GetTopPos()
    {
        return upItem;
    }

    public float GetBotPos()
    {
        return downItem;
    }

    public float GetRightos()
    {
        return rightItem;
    }

    public float GetLeftPos()
    {
        return leftItem;
    }

    public float GetFrontPos()
    {
        return frontItem;
    }

    public float GetBackPos()
    {
        return backItem;
    }

    public Reactor[] GetReactors()
    {
        Reactor[] reactors = GetComponentsInChildren<Reactor>();
        return reactors;
    }

    public void StopVehicle()
    {
        Wheel[] objects = GetComponentsInChildren<Wheel>().Select(x => (x as Wheel)).ToArray();

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].StopWheel();
        }
    }

    public void SetCenter(Transform _center)
    {
        center = _center;
    }
}
