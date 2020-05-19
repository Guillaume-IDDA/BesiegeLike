using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    public static LevelManager Instance { get => instance; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    [SerializeField] public PlayerController player;
    [SerializeField] public Vehicle vehicle;
    [SerializeField] public Camera cameraConstruction;
    [SerializeField] public Transform startPos;
    CameraGame cameraGame;
    [SerializeField] public UI_Creation ui_creation;
    [SerializeField] public UI_Game ui_game;
    [SerializeField] public UI_PauseMenu ui_pauseMenu;
    public bool onPause = false;

    public void StartGame()
    {
        I_ObjectInstantiate[] listObject = vehicle.GetComponentsInChildren<I_ObjectInstantiate>();

        I_ObjectInstantiate vehicleInterface = vehicle.GetComponent<I_ObjectInstantiate>();

        for (int i = 0; i < listObject.Length; i++)
        {
            listObject[i].OnStartGame();
        }

        CameraGame[] cam = vehicle.gameObject.GetComponentsInChildren<CameraGame>(true);

        cameraGame = cam[0];
        cameraConstruction.gameObject.SetActive(false);
        cameraGame.gameObject.SetActive(true);

        player.ChangeGamenMode(PlayerController.GameMode.Game);
    }
}
