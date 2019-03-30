using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject PauseMenuPanel;
    public GameObject ExitMenu;
    public Camera renderCamera;
    public GameObject PencilSizeContainer;

    public string ActionOnExit { get; set; }

    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<User>().IsPaused = true;
            ExitMenu.SetActive(true);
        }
    }

    public void ExecuteExitAction()
    {
        if (ActionOnExit.Equals("mainmenu"))
            ExitToMainMenu();
        else if (ActionOnExit.Equals("exit"))
            ExitGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ExitToMainMenu()
    {
        levelManager.LoadLevel(0);
    }
    public void TogglePauseMenu()
    {
        PauseMenuPanel.SetActive(!PauseMenuPanel.activeInHierarchy);
    }

    public void SaveCurrentPictureToGallery()
    {
        Texture2D texture = RTImage();
        

        Sprite screenshot = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),new Vector2(0.5f,0.5f));

        FindObjectOfType<GameManager>().AddGalleryPicture(screenshot);
    }

    public void ExportCurrentPicture()
    {
        Texture2D texture = RTImage();

        NativeGallery.SaveImageToGallery(texture, "Yetrem", "My img {0}.jpg");
    }

    Texture2D RTImage()
    {
        renderCamera.enabled = true;
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = renderCamera.targetTexture;

        // Render the camera's view.
        renderCamera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(renderCamera.targetTexture.width, renderCamera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, renderCamera.targetTexture.width, renderCamera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;
        return image;
    }

    public void SetPencilSizeSelected(GameObject pencilSize)
    {
        for(int i = 0; i < PencilSizeContainer.transform.childCount; i++)
        {
            Color currentColorBackground = PencilSizeContainer.transform.GetChild(i).GetComponent<Image>().color;

            if (PencilSizeContainer.transform.GetChild(i).gameObject == pencilSize)
                PencilSizeContainer.transform.GetChild(i).GetComponent<Image>().color = new Color(currentColorBackground.r, currentColorBackground.g, currentColorBackground.b, 1f);
            else
                PencilSizeContainer.transform.GetChild(i).GetComponent<Image>().color = new Color(currentColorBackground.r, currentColorBackground.g, currentColorBackground.b, 0f);
        }
    }
}