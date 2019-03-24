using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_MainMenu : MonoBehaviour
{
    private LevelManager levelManager;
    private GameManager gameManager;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void EnterGallery()
    {
        levelManager.LoadLevel(2);
    }

    public void SetPictureOnGameManager(GameObject g)
    {
        gameManager.SetPictureToDraw(g);
    }

    public void EnterGame()
    {
        levelManager.LoadLevel(1);
    }
}
