using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private Sprite imageToDraw;
    private List<Sprite> GalleryPictures = new List<Sprite>();
    public Translation TranslatedText { get; set; }
    public string Language { get; set; }

    public bool FirstTimeRun { get; set; }

    private void Awake()
    {

        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadGalleryFromDevice();
        LoadLanguage();
    }

    public void SaveGaleryToDevice()
    {
        if(!System.IO.File.Exists(Application.persistentDataPath + "/Yetrem/Gallery/"))
        {
            if (!System.IO.File.Exists(Application.persistentDataPath + "/Yetrem/"))
            {
                System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(Application.persistentDataPath); 
                d.CreateSubdirectory("Yetrem");                               
            }

            System.IO.DirectoryInfo t = new System.IO.DirectoryInfo(Application.persistentDataPath+"/Yetrem/"); 
            t.CreateSubdirectory("Gallery");
        }
        EraseAllDeviceGalleryDrawings();
        string path = Application.persistentDataPath + "/Yetrem/Gallery/";
        int i = 0;
        foreach (Sprite drawing in GalleryPictures)
        {
            System.IO.File.WriteAllBytes(path + "image" + i+".jpg", drawing.texture.EncodeToJPG());
            i++;
        }
    }

    public void EraseAllDeviceGalleryDrawings()
    {
        string path = Application.persistentDataPath + "/Yetrem/Gallery/";
        System.IO.DirectoryInfo dataDir = new System.IO.DirectoryInfo(path);
        try
        {
            System.IO.FileInfo[] fileinfo = dataDir.GetFiles();
            for (int i = 0; i < fileinfo.Length; i++)
            {
                string name = fileinfo[i].Name;
                System.IO.File.Delete(Application.persistentDataPath + "/Yetrem/Gallery/" + name);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }
    public void LoadGalleryFromDevice()
    {
        string path = Application.persistentDataPath + "/Yetrem/Gallery/";
        System.IO.DirectoryInfo dataDir = new System.IO.DirectoryInfo(path);
        try
        {
            System.IO.FileInfo[] fileinfo = dataDir.GetFiles();
            for (int i = 0; i < fileinfo.Length; i++)
            {
                string name = fileinfo[i].Name;
                byte[] bytes = System.IO.File.ReadAllBytes(Application.persistentDataPath + "/Yetrem/Gallery/" + name);

                Texture2D tex = new Texture2D(567, 794);
                tex.LoadImage(bytes);
                Sprite s = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                AddGalleryPicture(s);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    public void LoadLanguage()
    {
        string loadedLanguage = PlayerPrefs.GetString("language");
        if (loadedLanguage.Equals(""))
        {
            FirstTimeRun = true;
            SetLanguage("english");
        }
        else
        {
            FirstTimeRun = false;
            SetLanguage(loadedLanguage);
        }
    }


    public void SetPictureToDraw(GameObject g)
    {
        imageToDraw = g.GetComponent<Image>().sprite;
    }

    public void SetNewPicture()
    {
        FindObjectOfType<Drawing>().gameObject.GetComponent<SpriteRenderer>().sprite = imageToDraw;
    }

    public int GetGallerySize()
    {
        return GalleryPictures.Count;
    }
    public List<Sprite> GetGalleryPictures()
    {
        return GalleryPictures;
    }
    public void AddGalleryPicture(Sprite s)
    {
        GalleryPictures.Add(s);
    }

    public void DeleteFromGallery(Sprite s)
    {
        GalleryPictures.Remove(s);
    }

    public void SetLanguage(string language)
    {
        Language = language;
        var jsonTextFile = Resources.Load<TextAsset>("Languages/" + language);
        Translation t = JsonUtility.FromJson<Translation>(jsonTextFile.text);
        TranslatedText = t;
        FindObjectOfType<LanguageManager>().UpdateLanguage(t);
    }
}
