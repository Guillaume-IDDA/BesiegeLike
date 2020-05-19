using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public GameObject PrefabToInstantiate;
    [SerializeField] public GameObject PreviewPrefab;

    Camera cam;

    Vector3 posToInstantiate;
    Vector3 posInitPreview;

    LevelManager levelManager;

    public enum GameMode
    {
        Edition,
        Game
    }

    public enum creationMode
    {
        Construction,
        Destruction
    }

    private GameMode gameType = GameMode.Edition;
    private creationMode creaType = creationMode.Construction;

    public creationMode CreaType { get => creaType;}

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        levelManager = LevelManager.Instance;
        posInitPreview = new Vector3(0, -100, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(levelManager.ui_pauseMenu.gameObject.activeSelf)
            {
                levelManager.ui_pauseMenu.gameObject.SetActive(false);
                levelManager.onPause = false;
            }
            else
            {
                levelManager.ui_pauseMenu.gameObject.SetActive(true);
                levelManager.onPause = true;
            }
        }

        if (gameType == GameMode.Edition && !levelManager.onPause)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Construction")))
            {
                posToInstantiate = hit.transform.position + hit.normal;
                if (creaType == creationMode.Construction)
                {
                    PreviewPrefab.transform.position = posToInstantiate;
                }
                else
                {
                    PreviewPrefab.transform.position = posInitPreview;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (creaType == creationMode.Construction)
                    {
                        GameObject newBloc = Instantiate(PrefabToInstantiate, posToInstantiate, PreviewPrefab.transform.rotation, LevelManager.Instance.vehicle.transform);
                        newBloc.layer = LayerMask.NameToLayer("Construction");
                    }
                    else if (creaType == creationMode.Destruction)
                    {
                        if (hit.transform.tag != "CenterOfVehicle")
                        {
                            Destroy(hit.transform.gameObject);
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    PreviewPrefab.transform.RotateAroundLocal(Vector3.up, Mathf.PI / 2);
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    PreviewPrefab.transform.RotateAroundLocal(Vector3.up, -Mathf.PI / 2);
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    PreviewPrefab.transform.RotateAroundLocal(Vector3.right, Mathf.PI / 2);
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    PreviewPrefab.transform.RotateAroundLocal(Vector3.right, -Mathf.PI / 2);
                }
            }
            else
            {
                PreviewPrefab.transform.position = posInitPreview;
            }
        }

        if (gameType == GameMode.Game && !levelManager.onPause)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Vehicle vehicle = levelManager.vehicle;
                Reactor[] reactors = vehicle.GetReactors();

                for (int i = 0; i < reactors.Length; i++)
                {
                    reactors[i].Explosion(vehicle.GetComponent<Rigidbody>());
                }
            }
        }
    }

    public void ChangeConstructionMode(creationMode _mode)
    {
        creaType = _mode;
    }

    public void ChangeGamenMode(GameMode _mode)
    {
        gameType = _mode;
    }
}
