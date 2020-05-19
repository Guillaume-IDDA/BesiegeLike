using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        LevelManager.Instance.ui_game.DisplayEndGame();
    }
}
