using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        AssetBundle.UnloadAllAssetBundles(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
