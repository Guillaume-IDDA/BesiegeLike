using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using System.IO;
using UnityEngine.UI;

public class UI_Creation : MonoBehaviour
{
    public struct VehicleJson
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
        public string[] blocksPath;
    }

    public struct BlockJson
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
    }

    //[SerializeField] GameObject wheelFront;
    //[SerializeField] GameObject wheelBack;
    //[SerializeField] GameObject cube;
    //[SerializeField] GameObject reactor;
    [SerializeField] Text vehicleName;

    [SerializeField] Button buttonCreaMode;
    [SerializeField] Button buttonSelectVehicle;
    [SerializeField] Button buttonSelectBlock;

    [SerializeField] ScrollRect view_LoadVehicles;
    [SerializeField] RectTransform contentVehicles;
    [SerializeField] ScrollRect view_LoadBlocks;
    [SerializeField] RectTransform contentBlocks;

    [SerializeField] Text infoText;
    bool asCoroutine = false;

    LevelManager levelManager;

    //string pathVehicles = "Assets/Resources/Vehicles/";
    string pathVehicles = "/Datas/Vehicles/";
    //string pathBlocks = "Assets/Resources/Blocks/";

    GameObject[] arrayBlocks;

    private void Start()
    {
        levelManager = LevelManager.Instance;
        ChangePreview(Resources.Load<GameObject>("Blocks/Cube"));
        LoadBlocks();
    }

    public void StartGame()
    {
        levelManager.StartGame();
        gameObject.SetActive(false);
        StopPreview();
    }

    public void SelectCreaMode()
    {
        if (levelManager.player.CreaType == PlayerController.creationMode.Destruction)
        {
            levelManager.player.ChangeConstructionMode(PlayerController.creationMode.Construction);
            buttonCreaMode.GetComponentInChildren<Text>().text = "On Construction";
        }
        else
        {
            levelManager.player.ChangeConstructionMode(PlayerController.creationMode.Destruction);
            buttonCreaMode.GetComponentInChildren<Text>().text = "On Destruction";
        }
    }

    public void ChangePreview(GameObject _go)
    {
        GameObject preview = levelManager.player.PreviewPrefab;
        int tempchild = preview.transform.childCount;
        if (tempchild > 0)
        {
            Destroy(levelManager.player.PreviewPrefab.transform.GetChild(0).gameObject);
        }

        GameObject temp = Instantiate(_go, preview.transform);

        temp.layer = LayerMask.NameToLayer("Default");
    }

    private void StopPreview()
    {
        Destroy(levelManager.player.PreviewPrefab.gameObject);
    }

    public void SaveVehicle()
    {
        if (CanSave())
        {
            if (vehicleName.text != "")
            {
                VehicleJson vehicleJson;
                BlockJson blockToSave;

                Vehicle _vehicle = levelManager.vehicle;
                int nbChild = _vehicle.transform.childCount;

                //création du json du vehicule (voir si a suppr)
                vehicleJson.name = vehicleName.text;
                vehicleJson.position = _vehicle.transform.position;
                vehicleJson.rotation = _vehicle.transform.rotation;
                vehicleJson.blocksPath = new string[nbChild];

                //création du dossier 
                string path = Application.dataPath + pathVehicles + vehicleName.text;
                Directory.CreateDirectory(path);
                string newPath = path + '/';

                string jsonData;

                for (int i = 0; i < nbChild; i++)
                {
                    GameObject _go = _vehicle.transform.GetChild(i).gameObject;
                    blockToSave.position = _go.transform.position;
                    blockToSave.rotation = _go.transform.rotation;
                    string[] _name = _go.name.Split('(');
                    blockToSave.name = _name[0];
                    jsonData = JsonUtility.ToJson(blockToSave, true);

                    string newblockPath = newPath + _go.gameObject.name + i + ".txt";
                    File.WriteAllText(newPath + _go.gameObject.name + i + ".txt", jsonData + "\n");

                    vehicleJson.blocksPath[i] = _go.gameObject.name + i + ".txt";
                    //File.AppendAllText(newPath + vehicleName.text + ".txt", jsonData +"\n");
                }

                //Création du fichier du pour les liens vers les blocks du vehicule
                jsonData = JsonUtility.ToJson(vehicleJson, true);
                File.WriteAllText(path + ".txt", jsonData + "\n");
            }
            else
            {
                infoText.text = "Enter a name for your vehicle";
                if (asCoroutine)
                {
                    StopCoroutine(Inform());
                }
                StartCoroutine(Inform());

            }
        }
        else
        {
            infoText.text = "You can't have more than 10 vehicles saved, please delete one of them to save this one";
            if (asCoroutine)
            {
                StopCoroutine(Inform());
            }
            StartCoroutine(Inform());
        }
    }

    IEnumerator Inform()
    {
        asCoroutine = true;
        infoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        infoText.gameObject.SetActive(false);
        asCoroutine = false;
    }

    private bool CanSave()
    {
        GameObject[] tempArrayVehicle = Resources.LoadAll<GameObject>("Vehicles");
        if (tempArrayVehicle.Length < 10)
        {
            return true;
        }
        return false;
    }

    public void LoadVehicles()
    {
        if (view_LoadVehicles.gameObject.activeSelf)
        {
            HideVehiclesView();
            return;
        }
        int nbChild = contentVehicles.childCount;

        for (int i = 0; i < nbChild; i++)
        {
            Destroy(contentVehicles.GetChild(i).gameObject);
        }

        ///////////////////
        string[] files = Directory.GetFiles(Application.dataPath + pathVehicles, "*.txt");

        foreach (var file in files)
        {
            string jsonString = File.ReadAllText(file);
            VehicleJson vehicle = JsonUtility.FromJson<VehicleJson>(jsonString);

            Button newButton = Instantiate(buttonSelectVehicle, contentVehicles);
            newButton.GetComponentInChildren<Text>().text = vehicle.name;

            Button_SelectVehicle tmp = newButton.GetComponent<Button_SelectVehicle>();
            tmp.InitButton(vehicle, pathVehicles, this);
        }

        view_LoadVehicles.gameObject.SetActive(true);

        levelManager.onPause = true;
    }

    public void LoadBlocks()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "blocks");
        string[] namesBlocks = Directory.GetFiles(path, "*.manifest");
        arrayBlocks = new GameObject[namesBlocks.Length];

        for (int i = 0; i < namesBlocks.Length; i++)
        {
            string vehicleBlockName = Path.GetFileName(namesBlocks[i]).Split('.')[0];
            AssetBundle block = AssetBundle.LoadFromFile(Path.Combine(path, Path.GetFileName(namesBlocks[i]).Split('.')[0]));
            block.LoadAsset<GameObject>(vehicleBlockName + "_obj");
            arrayBlocks[i] = block.LoadAsset<GameObject>(vehicleBlockName);

            bool exception = arrayBlocks[i].name == "CameraGame" || arrayBlocks[i].name == "Center" || arrayBlocks[i].name == "Vehicle";
            if (!exception)
            {
                Button newButton = Instantiate(buttonSelectBlock, contentBlocks);
                newButton.GetComponentInChildren<Text>().text = arrayBlocks[i].name;
                Button_SelectBlock tmp = newButton.GetComponent<Button_SelectBlock>();
                tmp.InitButton(arrayBlocks[i], path, this);
            }
        }
    }

    public void DisplayBlocks()
    {
        if (view_LoadBlocks.gameObject.activeSelf)
        {
            HideBlocksView();
            return;
        }

        view_LoadBlocks.gameObject.SetActive(true);
        levelManager.onPause = true;
    }

    public void HideVehiclesView()
    {
        view_LoadVehicles.gameObject.SetActive(false);
        levelManager.onPause = false;
    }

    public void HideBlocksView()
    {
        view_LoadBlocks.gameObject.SetActive(false);
        levelManager.onPause = false;
    }
}
