using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class UIManager_MainMenu : MonoBehaviour
{
    public Animator MenuAnimator;
    public Animator GalleryAnimator;
    public Animator GalleryIconAnimator;

    public Animator MainIconAnimator;
    private int animIndex = 0;

    public GameObject EmptyGalleryText;
    public Text GalleryNumberText;
    public Image GalleryIcon;
    public Sprite Icon_Sad;
    public Sprite Icon_Happy;

    public GameObject GalleryPicturesContainer;
    public GameObject GalleryPictureTemplate;
    public GameObject DeletePanel;

    public GameObject ListOfDrawings;
    private float ListOfDrawings_X;
    private float ListOfDrawings_Y;

    public GameObject GalleryList;
    private float GalleryList_X;
    private float GalleryList_Y;

    private LevelManager levelManager;
    private GameManager gameManager;
    private Sprite drawingDeleteOnHold;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        gameManager = FindObjectOfType<GameManager>();

        ConfigureGalleryUI(gameManager.GetGallerySize() < 1);
        float posX = 0;
        foreach (Sprite s in gameManager.GetGalleryPictures())
        {
            GameObject instance = Instantiate(GalleryPictureTemplate, GalleryPicturesContainer.transform);
            instance.transform.GetChild(0).GetComponent<Image>().sprite = s;
            instance.transform.localPosition += new Vector3(posX,0f,0f);
            instance.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { TaskWithParameters(instance.transform.GetChild(0).gameObject); });
            posX += 619f;

            Button deleteButton = instance.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>();
            deleteButton.onClick.AddListener(delegate { PromptDelete(s); });
        }
        ListOfDrawings_X = ListOfDrawings.GetComponent<RectTransform>().offsetMin.x;
        ListOfDrawings_Y = ListOfDrawings.GetComponent<RectTransform>().offsetMax.x;

        GalleryList_X = GalleryList.GetComponent<RectTransform>().offsetMin.x;
        GalleryList_Y = GalleryList.GetComponent<RectTransform>().offsetMax.x;

        animIndex = UnityEngine.Random.Range(0, MainIconAnimator.runtimeAnimatorController.animationClips.Length);
        MainIconAnimator.SetInteger("AnimIndex", animIndex);
        StartCoroutine("MainIconAnimations");
    }

    IEnumerator MainIconAnimations()
    {
        yield return new WaitForSeconds(MainIconAnimator.GetCurrentAnimatorStateInfo(0).length);
        animIndex = UnityEngine.Random.Range(0, MainIconAnimator.runtimeAnimatorController.animationClips.Length);
        MainIconAnimator.SetInteger("AnimIndex", animIndex);
        StartCoroutine("MainIconAnimations");
    }


    private void ConfigureGalleryUI(bool isEmpty)
    {
        EmptyGalleryText.SetActive(isEmpty);
        GalleryNumberText.gameObject.SetActive(!isEmpty);
        if (GalleryNumberText.gameObject.activeInHierarchy)
            GalleryNumberText.text = "You saved " + gameManager.GetGallerySize() + " drawings!";

        GalleryIcon.sprite = (isEmpty) ? Icon_Sad : Icon_Happy;
        GalleryAnimator.enabled = isEmpty;

        GalleryIconAnimator.SetBool("isGalleryEmpty", isEmpty);
    }

    public void ResetListOfDrawingsPosition()
    {
        ListOfDrawings.GetComponent<RectTransform>().offsetMin = new Vector2(ListOfDrawings_X, ListOfDrawings.GetComponent<RectTransform>().offsetMin.y);
        ListOfDrawings.GetComponent<RectTransform>().offsetMax = new Vector2(ListOfDrawings_Y, ListOfDrawings.GetComponent<RectTransform>().offsetMax.y);
    }

    public void ResetGalleryListPosition()
    {
        GalleryList.GetComponent<RectTransform>().offsetMin = new Vector2(GalleryList_X, GalleryList.GetComponent<RectTransform>().offsetMin.y);
        GalleryList.GetComponent<RectTransform>().offsetMax = new Vector2(GalleryList_Y, GalleryList.GetComponent<RectTransform>().offsetMax.y);
    }


    void PromptDelete(Sprite s)
    {
        drawingDeleteOnHold = s;
        DeletePanel.SetActive(true);
        GalleryIconAnimator.SetBool("isDeletingDrawing", true);
    }

    public void DeleteThisDrawingFromGallery()
    {
        gameManager.DeleteFromGallery(drawingDeleteOnHold);
        UpdateGallery();
        GalleryIconAnimator.SetBool("isDeletingDrawing", false);
    }

    void UpdateGallery()
    {
        for(int i = 0; i < GalleryPicturesContainer.transform.childCount; i++)
            Destroy(GalleryPicturesContainer.transform.GetChild(i).gameObject);

        ConfigureGalleryUI(gameManager.GetGallerySize() < 1);
        float posX = 0;
        foreach (Sprite s in gameManager.GetGalleryPictures())
        {
            GameObject instance = Instantiate(GalleryPictureTemplate, GalleryPicturesContainer.transform);
            instance.transform.GetChild(0).GetComponent<Image>().sprite = s;
            instance.transform.localPosition += new Vector3(posX, 0f, 0f);
            instance.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { TaskWithParameters(instance.transform.GetChild(0).gameObject); });
            posX += 619f;

            Button deleteButton = instance.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>();
            deleteButton.onClick.AddListener(delegate { PromptDelete(s); });
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

    public void ImportImageToGallery()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, 2048);
                if (texture == null)
                {
                    return;
                }
                Sprite image = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                gameManager.AddGalleryPicture(image);
                UpdateGallery();
            }
        }, "Import an Image", "image/*",2048);
        return;
    }
    public void UpdateGalleryIconState()
    {
        GalleryIconAnimator.SetBool("isDeletingDrawing", false);
    }
}
