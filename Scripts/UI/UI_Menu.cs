using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class UI_Menu : MonoBehaviour
{

    GameObject[] buttonArray;
    [SerializeField] Button prefabButtonSelectLevel;
    [SerializeField] Transform contentScenes;
    [SerializeField] GameObject view_Levels;

    private void Start()
    {
        LoadLevels();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            HideLevelsView();
        }
    }

    void LoadLevels()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "levels");
        string[] levels = Directory.GetFiles(path, "*.manifest");

        buttonArray = new GameObject[levels.Length];

        for (int i = 0; i < levels.Length; i++)
        {
            string levelName = Path.GetFileName(levels[i]).Split('.')[0];
            if(levelName != "menu")
            {
                Button newButton = Instantiate(prefabButtonSelectLevel, contentScenes);
                newButton.GetComponentInChildren<Text>().text = levelName.Split('.')[0];
                Button_SelectScene tmp = newButton.GetComponent<Button_SelectScene>();
                tmp.InitButton(levelName);
            }
        }
    }

    public void DisplayLevels()
    {
        if (view_Levels.gameObject.activeSelf)
        {
            HideLevelsView();
            return;
        }

        view_Levels.gameObject.SetActive(true);
    }

    public void HideLevelsView()
    {
        view_Levels.gameObject.SetActive(false);
    }
}
