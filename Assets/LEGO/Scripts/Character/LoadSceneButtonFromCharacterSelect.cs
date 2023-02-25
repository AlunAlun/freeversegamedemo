using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButtonFromCharacterSelect : MonoBehaviour
{

    public void LoadScene()
    {
        CharacterDisplay cd = GetComponentInParent<CharacterDisplay>();

        switch(cd.m_characterStyle) {
            case CharacterStyle.Pizza:
                ConfigStatic.SceneCharacter = "pizza";
                break;
            case CharacterStyle.Astronaut:
                ConfigStatic.SceneCharacter = "astronaut";
                break;
                case CharacterStyle.Pirate:
                ConfigStatic.SceneCharacter = "pirate";
                break;
                case CharacterStyle.Adventurer:
                ConfigStatic.SceneCharacter = "adventurer";
                break;
        }

        SceneManager.LoadScene("LEGO Tutorial");
    }

    public void LoadMenu() {
        SceneManager.LoadScene("Menu Intro");
    }

    public void GoToSellURL() {

        CharacterDisplay cd = GetComponentInParent<CharacterDisplay>();

        Application.OpenURL("https://demo.games.gorengine.com/asset/" + cd.m_id);
    }

    public void GoToLoginURL() {

        Application.OpenURL("https://demo.games.gorengine.com/");
    }

    public void GoToMarketURL() {

        Application.OpenURL("https://demo.games.gorengine.com/drops");
    }

}
