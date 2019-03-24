using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_MainMenu : MonoBehaviour
{
    public Animator MenuAnimator;

    public GameObject EmptyGalleryText;
    public GameObject GalleryPicturesContainer;
    public GameObject GalleryPictureTemplate;

    private LevelManager levelManager;
    private GameManager gameManager;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        gameManager = FindObjectOfType<GameManager>();

        EmptyGalleryText.SetActive(gameManager.GetGallerySize() < 1);
        float posX = 0;
        foreach (Sprite s in gameManager.GetGalleryPictures())
        {
            GameObject instance = Instantiate(GalleryPictureTemplate, GalleryPicturesContainer.transform);
            instance.GetComponent<Image>().sprite = s;
            instance.transform.localPosition += new Vector3(posX,0f,0f);
            instance.GetComponent<Button>().onClick.AddListener(delegate { TaskWithParameters(instance); });
            posX += 619f;

            Button deleteButton = instance.transform.GetChild(0).GetComponent<Button>();
            deleteButton.onClick.AddListener(delegate { DeleteThisDrawingFromGallery(s); });
        }
    }

    void DeleteThisDrawingFromGallery(Sprite s)
    {
        gameManager.DeleteFromGallery(s);
        UpdateGallery();
    }

    void UpdateGallery()
    {
        for(int i = 0; i < GalleryPicturesContainer.transform.childCount; i++)
            Destroy(GalleryPicturesContainer.transform.GetChild(i).gameObject);

        EmptyGalleryText.SetActive(gameManager.GetGallerySize() < 1);
        float posX = 0;
        foreach (Sprite s in gameManager.GetGalleryPictures())
        {
            GameObject instance = Instantiate(GalleryPictureTemplate, GalleryPicturesContainer.transform);
            instance.GetComponent<Image>().sprite = s;
            instance.transform.localPosition += new Vector3(posX, 0f, 0f);
            instance.GetComponent<Button>().onClick.AddListener(delegate { TaskWithParameters(instance); });
            posX += 619f;

            Button deleteButton = instance.transform.GetChild(0).GetComponent<Button>();
            deleteButton.onClick.AddListener(delegate { DeleteThisDrawingFromGallery(s); });
        }
    }

    void TaskWithParameters(GameObject g)
    {
        SetPictureOnGameManager(g);
        EnterGame();
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
