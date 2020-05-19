using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_SelectScene : MonoBehaviour
{
    string sceneName;

    public void InitButton(string _sceneName)
    {
        sceneName = _sceneName;
    }

    public void LoadScene()
    {
        AssetBundle.UnloadAllAssetBundles(true);
        
        SceneManager.LoadScene(sceneName);
    }
}
