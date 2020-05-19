using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] int nbOfScene;
    public void ChangeScene()
    {
        AssetBundle.UnloadAllAssetBundles(true);

        SceneManager.LoadScene(nbOfScene);
    }
}
