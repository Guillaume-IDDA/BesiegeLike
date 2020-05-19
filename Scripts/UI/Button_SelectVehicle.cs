using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class Button_SelectVehicle : MonoBehaviour
{
    [SerializeField] GameObject choiceMenu;

    UI_Creation ui;
    UI_Creation.VehicleJson vehicleJson;
    string vehiclePath;

    GameObject vehicleBasePrefab;

    GameObject vehicle;

    private void Start()
    {
        vehicleBasePrefab = Resources.Load<GameObject>("Blocks/Vehicle");
    }

    public void InitButton(UI_Creation.VehicleJson _vehicleJson, string _pathFile, UI_Creation _uiCreation)
    {
        vehicleJson = _vehicleJson;
        ui = _uiCreation;
        vehiclePath = _pathFile;
    }

    private void CreateVehicle()
    {
        vehicle = Instantiate(vehicleBasePrefab, vehicleJson.position, vehicleJson.rotation);

        string[] filesBlock = vehicleJson.blocksPath;
        for (int i = 0; i < filesBlock.Length; i++)
        {
            Debug.Log(Application.dataPath + vehiclePath + vehicleJson.name + '/' + filesBlock[i] + ".txt");

            string jsonString = File.ReadAllText(Application.dataPath + vehiclePath + vehicleJson.name + '/' + filesBlock[i]);
            UI_Creation.BlockJson block = JsonUtility.FromJson<UI_Creation.BlockJson>(jsonString);
            GameObject prefabGo = Resources.Load<GameObject>("Blocks/" + block.name);
            GameObject blockGO = Instantiate(prefabGo, block.position, block.rotation, vehicle.transform);

            if (block.name == "Center")
            {
                vehicle.GetComponent<Vehicle>().SetCenter(blockGO.transform);
            }
        }
    }

    public void DisplayChoiceMenu()
    {
        if (!choiceMenu.activeSelf)
        {
            choiceMenu.SetActive(true);
        }
        else
        {
            choiceMenu.SetActive(false);
        }
    }

    public void LoadVehicle()
    {
        Destroy(LevelManager.Instance.vehicle.gameObject);
        CreateVehicle();
        LevelManager.Instance.vehicle = vehicle.GetComponent<Vehicle>();

        ui.HideVehiclesView();
        choiceMenu.SetActive(false);
    }

    public void DeleteVehicle()
    {
        string[] files = Directory.GetFiles(Application.dataPath + vehiclePath + vehicleJson.name + '/');

        foreach (var file in files)
        {
            File.Delete(file);
        }

        Directory.Delete(Application.dataPath + vehiclePath + vehicleJson.name +'/');
        File.Delete(Application.dataPath + vehiclePath + vehicleJson.name + ".meta");
        File.Delete(Application.dataPath + vehiclePath + vehicleJson.name + ".txt");
        File.Delete(Application.dataPath + vehiclePath + vehicleJson.name + ".txt.meta");
        choiceMenu.SetActive(false);
        Destroy(gameObject);
    }
}
