using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    public Text MainMenu_Button_NewDrawing;
    public Text MainMenu_Button_Gallery;
    public Text Gallery_Button_Return;
    public Text Gallery_Text_EmptyText;
    public Text Gallery_Text_EmptyDescription;
    public Text Gallery_Text_Title;
    public Text Gallery_Button_Import;
    public Text Gallery_DeletePanel_Text;
    public Text NewDrawing_Button_Return;
    public Text NewDrawing_Text_Title;
    public Text NewDrawing_Button_Filter;
    public Text Game_PauseMenu_SaveToGallery;
    public Text Game_PauseMenu_Export;
    public Text Game_PauseMenu_MainMenu;
    public Text Game_ContinuePanel_Text;
    public Text Game_ExitMenu_Return;
    public Text Game_ExitMenu_MainMenu;
    public Text Game_ExitMenu_ExitGame;
    public Text Game_SaveToGallery_Text;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if(gameManager.TranslatedText != null)
            UpdateLanguage(FindObjectOfType<GameManager>().TranslatedText);
    }

    public void UpdateLanguage(Translation t)
    {
        if(MainMenu_Button_NewDrawing != null)    
            MainMenu_Button_NewDrawing.text = t.MainMenu_Button_NewDrawing;

        if (MainMenu_Button_Gallery != null)
            MainMenu_Button_Gallery.text = t.MainMenu_Button_Gallery;

        if (Gallery_Button_Return != null)
            Gallery_Button_Return.text = t.Gallery_Button_Return;

        if (Gallery_Text_EmptyText != null)
            Gallery_Text_EmptyText.text = t.Gallery_Text_EmptyText;

        if (Gallery_Text_EmptyDescription != null)
            Gallery_Text_EmptyDescription.text = t.Gallery_Text_EmptyDescription;

        if (Gallery_Text_Title != null)
            Gallery_Text_Title.text = t.Gallery_Text_Title;

        if (Gallery_Button_Import != null)
            Gallery_Button_Import.text = t.Gallery_Button_Import;

        if (Gallery_DeletePanel_Text != null)
            Gallery_DeletePanel_Text.text = t.Gallery_DeletePanel_Text;

        if (NewDrawing_Button_Return != null)
            NewDrawing_Button_Return.text = t.NewDrawing_Button_Return;

        if (NewDrawing_Text_Title != null)
            NewDrawing_Text_Title.text = t.NewDrawing_Text_Title;

        if (NewDrawing_Button_Filter != null)
            NewDrawing_Button_Filter.text = t.NewDrawing_Button_Filter;

        if (Game_PauseMenu_SaveToGallery != null)
            Game_PauseMenu_SaveToGallery.text = t.Game_PauseMenu_SaveToGallery;

        if (Game_PauseMenu_Export != null)
            Game_PauseMenu_Export.text = t.Game_PauseMenu_Export;

        if (Game_PauseMenu_MainMenu != null)
            Game_PauseMenu_MainMenu.text = t.Game_PauseMenu_MainMenu;

        if (Game_ContinuePanel_Text != null)
            Game_ContinuePanel_Text.text = t.Game_ContinuePanel_Text;

        if (Game_ExitMenu_Return != null)
            Game_ExitMenu_Return.text = t.Game_ExitMenu_Return;

        if (Game_ExitMenu_MainMenu != null)
            Game_ExitMenu_MainMenu.text = t.Game_ExitMenu_MainMenu;

        if (Game_ExitMenu_ExitGame != null)
            Game_ExitMenu_ExitGame.text = t.Game_ExitMenu_ExitGame;

        if (Game_SaveToGallery_Text != null)
            Game_SaveToGallery_Text.text = t.Game_SaveToGallery_Text;
    }
}
