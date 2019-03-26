using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject PauseMenuPanel;
    public Camera renderCamera;

    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
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
}