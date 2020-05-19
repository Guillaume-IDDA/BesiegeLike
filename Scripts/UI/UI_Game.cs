using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{
    [SerializeField] GameObject menu;
    // Start is called before the first frame update
    public void DisplayEndGame()
    {
        menu.gameObject.SetActive(true);
    }
}
