using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject ExitMenu;
    public Camera renderCamera;
    public GameObject PencilSizeContainer;
    public GameObject SaveToGalleryPanel;
    public GameObject ContinuePanel;
    public GameObject PencilColorList;
    public GameObject SizeList;
    public Text SizeText;
    public GameObject SizeButton;

    private float CameraSize;
    public Slider SliderZoom;
    public GameObject ZoomingMenu;
    public GameObject NotZoomingMenu;

    public string ActionOnExit { get; set; }

    private LevelManager levelManager;
    private User user;
    private bool pauseOn = false;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        user = FindObjectOfType<User>();
        FindObjectOfType<AdManager>().ShowInterstitial(false);
        CameraSize = Camera.main.orthographicSize;
        Debug.Log(CameraSize);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<User>().IsPaused = true;
            ExitMenu.SetActive(true);

            SaveToGalleryPanel.SetActive(false);
            ContinuePanel.SetActive(false);
        }
    }

    public void ToggleSizeList()
    {
        SizeList.SetActive(!SizeList.activeInHierarchy);
        user.IsPaused = SizeList.activeInHierarchy;
        if (user.IsPaused)
            SizeButton.GetComponent<Image>().color = new Color(236/255f,236/255f,236/255f);
        else
            SizeButton.GetComponent<Image>().color = Color.white;
    }

    public void UpdatePencilSizeColor(Color color)
    {
        for (int i = 0; i < PencilSizeContainer.transform.childCount; i++)
            PencilSizeContainer.transform.GetChild(i).GetComponent<Image>().color = color;
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
        FindObjectOfType<GameManager>().SaveGaleryToDevice();

        PlayerPrefs.SetString("language", FindObjectOfType<GameManager>().Language);

        FindObjectOfType<AdManager>().ShowInterstitial(true);
    }

    public void ExitToMainMenu()
    {
        FindObjectOfType<AdManager>().ShowInterstitial(false);
        levelManager.LoadLevel(0);
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
            if (PencilSizeContainer.transform.GetChild(i).gameObject == pencilSize)
                PencilSizeContainer.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
            else
                PencilSizeContainer.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
        }
        SizeText.text = pencilSize.name;
    }
    public void HighlightPencil(GameObject p)
    {
        for(int i = 0; i < PencilColorList.transform.childCount; i++)
        {
            if (PencilColorList.transform.GetChild(i).name.Equals(p.name))
                PencilColorList.transform.GetChild(i).transform.localScale = new Vector2(0.4f,1f);
            else
                PencilColorList.transform.GetChild(i).transform.localScale = new Vector2(0.2f, 0.8f);

        }
    }    

    public void ChangeZoom()
    {
        CameraSize = SliderZoom.value;
        Camera.main.orthographicSize = CameraSize;

        if(CameraSize == 5f)
        {
            NotZoomingMenu.SetActive(true);
            ZoomingMenu.SetActive(false);
            Camera.main.transform.position = new Vector3(0f, 0f, -10f);
        }
        else
        {
            NotZoomingMenu.SetActive(false);
            ZoomingMenu.SetActive(true);
        }
    }
    public void MoveUp()
    {
        Camera.main.transform.position += new Vector3(0f,0.1f,0f);
    }
    public void MoveDown()
    {
        Camera.main.transform.position += new Vector3(0f, -0.1f, 0f);
    }
    public void MoveLeft()
    {
        Camera.main.transform.position += new Vector3(-0.1f,0f, 0f);
    }
    public void MoveRight()
    {
        Camera.main.transform.position += new Vector3(0.1f,0f, 0f);
    }
}