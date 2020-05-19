using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_SelectBlock : MonoBehaviour
{
    GameObject block;
    UI_Creation ui;
    string blockPath;

    public void InitButton(GameObject _goVehicle, string _path, UI_Creation _uiCreation)
    {
        block = _goVehicle;
        ui = _uiCreation;
        blockPath = _path;
    }

    public void SelectBlock()
    {
        LevelManager.Instance.player.PrefabToInstantiate = block;
        ui.ChangePreview(block);
        ui.HideBlocksView();
    }
}
