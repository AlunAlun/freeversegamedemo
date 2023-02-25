using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SceneCharacterActivator : MonoBehaviour
{
    public GameObject characterPizza;
    public GameObject characterAdventurer;
    public GameObject characterAstronaut;
    public GameObject characterPirate;
    public CinemachineFreeLook cam;

    // Start is called before the first frame update
    void Start()
    {
        //get global config
        string sceneCharacter = ConfigStatic.SceneCharacter;
        if (sceneCharacter == null) sceneCharacter = "astronaut"; //error handling

        characterPizza.SetActive(false);
        characterAstronaut.SetActive(false);
        characterAdventurer.SetActive(false);
        characterPirate.SetActive(false);
        switch (sceneCharacter) {
            case "pizza":
                SetCharacter(characterPizza);
                break;
            case "astronaut":
                SetCharacter(characterAstronaut);
                break;
            case "adventurer":
                SetCharacter(characterAdventurer);
                break;
            case "pirate":
                SetCharacter(characterPirate);
                break;
        }
    }

    public void SetCharacter(GameObject character) {
        character.SetActive(true);
        cam.m_Follow = character.transform;
        cam.m_LookAt = character.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
