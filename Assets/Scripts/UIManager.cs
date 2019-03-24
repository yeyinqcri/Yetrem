using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject PauseMenuPanel;

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
}
