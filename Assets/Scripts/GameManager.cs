using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private Sprite imageToDraw;

    private void Awake()
    {

        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void SetPictureToDraw(GameObject g)
    {
        imageToDraw = g.GetComponent<Image>().sprite;
        Debug.Log("image to draw = " + imageToDraw);
    }

    public void SetNewPicture()
    {
        Debug.Log("h");
        FindObjectOfType<Drawing>().gameObject.GetComponent<SpriteRenderer>().sprite = imageToDraw;
    }
}
