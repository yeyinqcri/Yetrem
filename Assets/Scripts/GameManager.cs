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
        SetLanguage("english");
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
        var jsonTextFile = Resources.Load<TextAsset>("Languages/" + language);
        Translation t = JsonUtility.FromJson<Translation>(jsonTextFile.text);
        TranslatedText = t;
        FindObjectOfType<LanguageManager>().UpdateLanguage(t);
        Language = language;
    }
}
