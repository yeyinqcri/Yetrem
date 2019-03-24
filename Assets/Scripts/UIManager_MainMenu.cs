using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_MainMenu : MonoBehaviour
{
    public Animator MenuAnimator;

    private LevelManager levelManager;
    private GameManager gameManager;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void EnterGallery()
    {
        MenuAnimator.SetBool("isMovingFromGallery", false);
        MenuAnimator.SetBool("isMovingToGallery", true);
    }
    public void ExitGallery()
    {
        MenuAnimator.SetBool("isMovingToGallery", false);
        MenuAnimator.SetBool("isMovingFromGallery", true);
    }
    public void EnterListPictures()
    {
        MenuAnimator.SetBool("isMovingFromListPictures", false);
        MenuAnimator.SetBool("isMovingToListPictures", true);
    }
    public void ExitListPictures()
    {
        MenuAnimator.SetBool("isMovingToListPictures", false);
        MenuAnimator.SetBool("isMovingFromListPictures", true);
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
