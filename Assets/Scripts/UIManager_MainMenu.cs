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

    public GameObject NewDrawingContainer;
    public GameObject NewDrawingTemplate;
    public Text NewDrawingNumberText;
    public Text NewDrawingCategoryText;
    public GameObject CategoryListContainer;

    public GameObject GalleryPicturesContainer;
    public GameObject GalleryPictureTemplate;
    public GameObject DeletePanel;
    public GameObject LanguagePanel;

    public GameObject LanguageScrollPosY;

    public Text CategoryName;
    public Text CategoryTitle;
    public Text CategoryConfirmButtonText;

    public GameObject ListOfDrawings;
    private float ListOfDrawings_X;
    private float ListOfDrawings_Y;

    public GameObject GalleryList;
    private float GalleryList_X;
    private float GalleryList_Y;

    private LevelManager levelManager;
    private GameManager gameManager;
    private Sprite drawingDeleteOnHold;

    private string[] categoriesNames = {"All","Animal","Building","Complex","Face","Fantastic","Flower","Fruit","Job","Love","Object","Space","Vehicle", "Other" };

    private string category = "";

    private void Start()
    {

        levelManager = FindObjectOfType<LevelManager>();
        gameManager = FindObjectOfType<GameManager>();

        ListOfDrawings_X = ListOfDrawings.GetComponent<RectTransform>().offsetMin.x;
        ListOfDrawings_Y = ListOfDrawings.GetComponent<RectTransform>().offsetMax.x;

        GenerateNewDrawingMenu("All");

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
        

        GalleryList_X = GalleryList.GetComponent<RectTransform>().offsetMin.x;
        GalleryList_Y = GalleryList.GetComponent<RectTransform>().offsetMax.x;

        animIndex = UnityEngine.Random.Range(0, MainIconAnimator.runtimeAnimatorController.animationClips.Length);
        MainIconAnimator.SetInteger("AnimIndex", animIndex);
        StartCoroutine("MainIconAnimations");

        float x = 0f;
        switch(gameManager.Language)
        {
            case "english":
                x = 0f;
                break;
            case "french":
                x = 120f;
                break;
            case "portuguese":
                x = 240f;
                break;
            case "spanish":
                x = 360f;
                break;
        }
        LanguageScrollPosY.GetComponent<RectTransform>().localPosition += new Vector3(0f,x, 0f);

        LanguagePanel.SetActive(gameManager.FirstTimeRun);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<GameManager>().SaveGaleryToDevice();
            PlayerPrefs.SetString("language", FindObjectOfType<GameManager>().Language);

            gameManager.GetComponent<AdManager>().ShowInterstitial();
            Application.Quit();
        }
    }

    public void GenerateNewDrawingMenu(string category)
    {
        this.category = category;
        for (int i = 0; i < NewDrawingContainer.transform.childCount; i++)
            Destroy(NewDrawingContainer.transform.GetChild(i).gameObject);

        int count = 0;
        float posX = 0;
        foreach (Sprite o in Resources.LoadAll<Sprite>("New Drawing"+(category.Equals("All") ? "" : "/"+category)))
        {
            GameObject instance = Instantiate(NewDrawingTemplate, NewDrawingContainer.transform);
            instance.transform.GetChild(0).GetComponent<Image>().sprite = o;
            instance.transform.localPosition += new Vector3(posX, 0f,0f);
            posX += 619f;
            instance.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { TaskWithParameters(instance.transform.GetChild(0).gameObject); });
            count++;
        }
        if(gameManager.Language.Equals("english"))
            NewDrawingNumberText.text = (count > 1) ? ("There are " + count + " drawings available!") : ("There are " + count + " drawing available!");
        else if(gameManager.Language.Equals("portuguese"))
            NewDrawingNumberText.text = (count > 1) ? ("Existem " + count + " desenhos disponiveis!") : ("Existe " + count + " desenho disponivel!");
        else if (gameManager.Language.Equals("spanish"))
            NewDrawingNumberText.text = (count > 1) ? ("Hay " + count + " disenos disponibles!") : ("Hay " + count + " dibujo disponible!");
        else if (gameManager.Language.Equals("french"))
            NewDrawingNumberText.text = (count > 1) ? ("Il y a " + count + " dessins disponibles!") : ("Il y a " + count + " dessin disponible!");

        NewDrawingCategoryText.text = category;

        for (int i = 0; i < CategoryListContainer.transform.childCount; i++)
        {
            GameObject child = CategoryListContainer.transform.GetChild(i).gameObject;
            string text = child.name;

            Color currentColor = child.GetComponent<Image>().color;
            if (text.Equals(category))
                currentColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
            else
                currentColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);

            child.GetComponent<Image>().color = currentColor;
        }
        ResetListOfDrawingsPosition();

        switch (category)
        {
            case "All":
                if (gameManager.Language.Equals("english"))
                    CategoryName.text = "All";
                else if (gameManager.Language.Equals("french"))
                    CategoryName.text = "Tout";
                else if (gameManager.Language.Equals("portuguese"))
                    CategoryName.text = "Todos";
                else if (gameManager.Language.Equals("spanish"))
                    CategoryName.text = "Todos";
                break;
            case "Animal":
                if (gameManager.Language.Equals("english"))
                    CategoryName.text = "Animal";
                else if (gameManager.Language.Equals("french"))
                    CategoryName.text = "Animal";
                else if (gameManager.Language.Equals("portuguese"))
                    CategoryName.text = "Animal";
                else if (gameManager.Language.Equals("spanish"))
                    CategoryName.text = "Animal";
                break;
            case "Building":
                if (gameManager.Language.Equals("english"))
                    CategoryName.text = "Building";
                else if (gameManager.Language.Equals("french"))
                    CategoryName.text = "Edifice";
                else if (gameManager.Language.Equals("portuguese"))
                    CategoryName.text = "Edificio";
                else if (gameManager.Language.Equals("spanish"))
                    CategoryName.text = "Edificio";
                break;
            case "Complex":
                if (gameManager.Language.Equals("english"))
                    CategoryName.text = "Complex";
                else if (gameManager.Language.Equals("french"))
                    CategoryName.text = "Complexe";
                else if (gameManager.Language.Equals("portuguese"))
                    CategoryName.text = "Complexo";
                else if (gameManager.Language.Equals("spanish"))
                    CategoryName.text = "Complejo";
                break;
            case "Face":
                if (gameManager.Language.Equals("english"))
                    CategoryName.text = "Face";
                else if (gameManager.Language.Equals("french"))
                    CategoryName.text = "Face";
                else if (gameManager.Language.Equals("portuguese"))
                    CategoryName.text = "Face";
                else if (gameManager.Language.Equals("spanish"))
                    CategoryName.text = "Cara";
                break;
            case "Fantastic":
                if (gameManager.Language.Equals("english"))
                    CategoryName.text = "Fantastic";
                else if (gameManager.Language.Equals("french"))
                    CategoryName.text = "Imaginaire";
                else if (gameManager.Language.Equals("portuguese"))
                    CategoryName.text = "Fantastico";
                else if (gameManager.Language.Equals("spanish"))
                    CategoryName.text = "Fantastico";
                break;
            case "Flower":
                if (gameManager.Language.Equals("english"))
                    CategoryName.text = "Flower";
                else if (gameManager.Language.Equals("french"))
                    CategoryName.text = "Fleur";
                else if (gameManager.Language.Equals("portuguese"))
                    CategoryName.text = "Flor";
                else if (gameManager.Language.Equals("spanish"))
                    CategoryName.text = "Flor";
                break;
            case "Fruit":
                if (gameManager.Language.Equals("english"))
                    CategoryName.text = "Fruit";
                else if (gameManager.Language.Equals("french"))
                    CategoryName.text = "Fruit";
                else if (gameManager.Language.Equals("portuguese"))
                    CategoryName.text = "Fruta";
                else if (gameManager.Language.Equals("spanish"))
                    CategoryName.text = "Fruta";
                break;
            case "Job":
                if (gameManager.Language.Equals("english"))
                    CategoryName.text = "Job";
                else if (gameManager.Language.Equals("french"))
                    CategoryName.text = "Emploi";
                else if (gameManager.Language.Equals("portuguese"))
                    CategoryName.text = "Trabalho";
                else if (gameManager.Language.Equals("spanish"))
                    CategoryName.text = "Trabajo";
                break;
            case "Love":
                if (gameManager.Language.Equals("english"))
                    CategoryName.text = "Love";
                else if (gameManager.Language.Equals("french"))
                    CategoryName.text = "Amour";
                else if (gameManager.Language.Equals("portuguese"))
                    CategoryName.text = "Amor";
                else if (gameManager.Language.Equals("spanish"))
                    CategoryName.text = "Amor";
                break;
            case "Object":
                if (gameManager.Language.Equals("english"))
                    CategoryName.text = "Object";
                else if (gameManager.Language.Equals("french"))
                    CategoryName.text = "Objet";
                else if (gameManager.Language.Equals("portuguese"))
                    CategoryName.text = "Objeto";
                else if (gameManager.Language.Equals("spanish"))
                    CategoryName.text = "Objeto";
                break;
            case "Other":
                if (gameManager.Language.Equals("english"))
                    CategoryName.text = "Other";
                else if (gameManager.Language.Equals("french"))
                    CategoryName.text = "Autre";
                else if (gameManager.Language.Equals("portuguese"))
                    CategoryName.text = "Outros";
                else if (gameManager.Language.Equals("spanish"))
                    CategoryName.text = "Otros";
                break;
            case "Space":
                if (gameManager.Language.Equals("english"))
                    CategoryName.text = "Space";
                else if (gameManager.Language.Equals("french"))
                    CategoryName.text = "Cosmique";
                else if (gameManager.Language.Equals("portuguese"))
                    CategoryName.text = "Cosmico";
                else if (gameManager.Language.Equals("spanish"))
                    CategoryName.text = "Cosmico";
                break;
            case "Vehicle":
                if (gameManager.Language.Equals("english"))
                    CategoryName.text = "Vehicle";
                else if (gameManager.Language.Equals("french"))
                    CategoryName.text = "Vehicule";
                else if (gameManager.Language.Equals("portuguese"))
                    CategoryName.text = "Veiculo";
                else if (gameManager.Language.Equals("spanish"))
                    CategoryName.text = "Vehiculo";
                break;
        }

        if (gameManager.Language.Equals("english"))
        {
            CategoryTitle.text = "Categories";
            CategoryConfirmButtonText.text = "Confirm";
        }
        else if(gameManager.Language.Equals("french"))
        {
            CategoryTitle.text = "Categories";
            CategoryConfirmButtonText.text = "Confirmer";
        }
        else if (gameManager.Language.Equals("portuguese"))
        {
            CategoryTitle.text = "Categorias";
            CategoryConfirmButtonText.text = "Confirmar";
        }
        else if (gameManager.Language.Equals("spanish"))
        {
            CategoryTitle.text = "Categorias";
            CategoryConfirmButtonText.text = "Confirmar";
        }
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
        {
            if(gameManager.Language.Equals("english"))
                GalleryNumberText.text = (gameManager.GetGallerySize() > 1) ? ("You saved " + gameManager.GetGallerySize() + " drawings!") : ("You saved " + gameManager.GetGallerySize() + " drawing!");
            else if (gameManager.Language.Equals("portuguese"))
                GalleryNumberText.text = (gameManager.GetGallerySize() > 1) ? ("Guardaste " + gameManager.GetGallerySize() + " desenhos!") : ("Guardaste " + gameManager.GetGallerySize() + " desenho!");
            else if (gameManager.Language.Equals("spanish"))
                GalleryNumberText.text = (gameManager.GetGallerySize() > 1) ? ("Guardaste " + gameManager.GetGallerySize() + " dibujos!") : ("Guardaste " + gameManager.GetGallerySize() + " dibujo!");
            else if (gameManager.Language.Equals("french"))
                GalleryNumberText.text = (gameManager.GetGallerySize() > 1) ? ("Vous avez enregistre " + gameManager.GetGallerySize() + " dessins!") : ("Vous avez enregistre " + gameManager.GetGallerySize() + " dessin!");
        }

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
    public void UpdateCategoriesLanguage()
    {
        for (int i = 0; i < CategoryListContainer.transform.childCount; i++)
        {
            Text t = CategoryListContainer.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
            switch (categoriesNames[i])
            {
                case "All":
                    if (gameManager.Language.Equals("english"))
                        t.text = "All";
                    else if (gameManager.Language.Equals("french"))
                        t.text = "Tout";
                    else if (gameManager.Language.Equals("portuguese"))
                        t.text = "Todos";
                    else if (gameManager.Language.Equals("spanish"))
                        t.text = "Todos";
                    break;
                case "Animal":
                    if (gameManager.Language.Equals("english"))
                        t.text = "Animal";
                    else if (gameManager.Language.Equals("french"))
                        t.text = "Animal";
                    else if (gameManager.Language.Equals("portuguese"))
                        t.text = "Animal";
                    else if (gameManager.Language.Equals("spanish"))
                        t.text = "Animal";
                    break;
                case "Building":
                    if (gameManager.Language.Equals("english"))
                        t.text = "Building";
                    else if (gameManager.Language.Equals("french"))
                        t.text = "Edifice";
                    else if (gameManager.Language.Equals("portuguese"))
                        t.text = "Edificio";
                    else if (gameManager.Language.Equals("spanish"))
                        t.text = "Edificio";
                    break;
                case "Complex":
                    if (gameManager.Language.Equals("english"))
                        t.text = "Complex";
                    else if (gameManager.Language.Equals("french"))
                        t.text = "Complexe";
                    else if (gameManager.Language.Equals("portuguese"))
                        t.text = "Complexo";
                    else if (gameManager.Language.Equals("spanish"))
                        t.text = "Complejo";
                    break;
                case "Face":
                    if (gameManager.Language.Equals("english"))
                        t.text = "Face";
                    else if (gameManager.Language.Equals("french"))
                        t.text = "Face";
                    else if (gameManager.Language.Equals("portuguese"))
                        t.text = "Face";
                    else if (gameManager.Language.Equals("spanish"))
                        t.text = "Cara";
                    break;
                case "Fantastic":
                    if (gameManager.Language.Equals("english"))
                        t.text = "Fantastic";
                    else if (gameManager.Language.Equals("french"))
                        t.text = "Imaginaire";
                    else if (gameManager.Language.Equals("portuguese"))
                        t.text = "Fantastico";
                    else if (gameManager.Language.Equals("spanish"))
                        t.text = "Fantastico";
                    break;
                case "Flower":
                    if (gameManager.Language.Equals("english"))
                        t.text = "Flower";
                    else if (gameManager.Language.Equals("french"))
                        t.text = "Fleur";
                    else if (gameManager.Language.Equals("portuguese"))
                        t.text = "Flor";
                    else if (gameManager.Language.Equals("spanish"))
                        t.text = "Flor";
                    break;
                case "Fruit":
                    if (gameManager.Language.Equals("english"))
                        t.text = "Fruit";
                    else if (gameManager.Language.Equals("french"))
                        t.text = "Fruit";
                    else if (gameManager.Language.Equals("portuguese"))
                        t.text = "Fruta";
                    else if (gameManager.Language.Equals("spanish"))
                        t.text = "Fruta";
                    break;
                case "Job":
                    if (gameManager.Language.Equals("english"))
                        t.text = "Job";
                    else if (gameManager.Language.Equals("french"))
                        t.text = "Emploi";
                    else if (gameManager.Language.Equals("portuguese"))
                        t.text = "Trabalho";
                    else if (gameManager.Language.Equals("spanish"))
                        t.text = "Trabajo";
                    break;
                case "Love":
                    if (gameManager.Language.Equals("english"))
                        t.text = "Love";
                    else if (gameManager.Language.Equals("french"))
                        t.text = "Amour";
                    else if (gameManager.Language.Equals("portuguese"))
                        t.text = "Amor";
                    else if (gameManager.Language.Equals("spanish"))
                        t.text = "Amor";
                    break;
                case "Object":
                    if (gameManager.Language.Equals("english"))
                        t.text = "Object";
                    else if (gameManager.Language.Equals("french"))
                        t.text = "Objet";
                    else if (gameManager.Language.Equals("portuguese"))
                        t.text = "Objeto";
                    else if (gameManager.Language.Equals("spanish"))
                        t.text = "Objeto";
                    break;
                case "Other":
                    if (gameManager.Language.Equals("english"))
                        t.text = "Other";
                    else if (gameManager.Language.Equals("french"))
                        t.text = "Autre";
                    else if (gameManager.Language.Equals("portuguese"))
                        t.text = "Outros";
                    else if (gameManager.Language.Equals("spanish"))
                        t.text = "Otros";
                    break;
                case "Space":
                    if (gameManager.Language.Equals("english"))
                        t.text = "Space";
                    else if (gameManager.Language.Equals("french"))
                        t.text = "Cosmique";
                    else if (gameManager.Language.Equals("portuguese"))
                        t.text = "Cosmico";
                    else if (gameManager.Language.Equals("spanish"))
                        t.text = "Cosmico";
                    break;
                case "Vehicle":
                    if (gameManager.Language.Equals("english"))
                        t.text = "Vehicle";
                    else if (gameManager.Language.Equals("french"))
                        t.text = "Vehicule";
                    else if (gameManager.Language.Equals("portuguese"))
                        t.text = "Veiculo";
                    else if (gameManager.Language.Equals("spanish"))
                        t.text = "Vehiculo";
                    break;
            }
        }
    }
    public void SetLanguage(string language)
    {
        FindObjectOfType<GameManager>().SetLanguage(language);
        GenerateNewDrawingMenu(this.category);
        UpdateGallery();
        UpdateCategoriesLanguage();
        float x = 0f;
        switch (gameManager.Language)
        {
            case "english":
                x = 0f;
                break;
            case "french":
                x = 120f;
                break;
            case "portuguese":
                x = 240f;
                break;
            case "spanish":
                x = 360f;
                break;
        }
        LanguageScrollPosY.GetComponent<RectTransform>().localPosition += new Vector3(0f, x, 0f);
    }
}
