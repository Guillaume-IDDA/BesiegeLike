using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_PauseMenu : MonoBehaviour
{
    public void Restart()
    {
        AssetBundle.UnloadAllAssetBundles(true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }
}
