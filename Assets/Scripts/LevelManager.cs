using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    private void Awake()
    {

        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SceneManager.GetActiveScene().buildIndex == 1)
            FindObjectOfType<GameManager>().SetNewPicture();
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindObjectOfType<AdManager>().ShowInterstitial();

        if (SceneManager.GetActiveScene().buildIndex == 1)
            FindObjectOfType<GameManager>().SetNewPicture();
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
