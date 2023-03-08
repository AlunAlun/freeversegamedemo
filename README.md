# freeversegamedemo

This is the basic example provided free by Unity, modified to load assets belonging to a user's web3 address into the game, so that they can be used by the player.

You can see a demo of it running here: https://demo.games.gorengine.com/

## Super quickstart guide

If you're short on time, read:

**Section 1** for to get a web3 address from the Freeverse marketplace into a Unity WebGL application

**Section 2.2** - particularly GraphQL.cs and CharacterSetup.cs for how to read Freeverse API from within Unity

## 1. Passing the user's web3 address from the marketplace to the unity binary

The user logs into the Freeverse webmarket, and when clicking the Play button the game is loaded within an iframe. The web3 address of the user logged into the marketplace is passed in encrypted form as a url parameter to the iframe.

In this example, the web3 address is decrypted within the javascript of the Unity web player, though a more secure solution would be to do this decryption within the Unity binary using Nethereum.

WebGLTemplates/Freeverse/bundle.js
* Browserfied bundle created from main.js in this repo: https://github.com/AlunAlun/eth-crypto-local

WebGLTemplates/Freeverse/index.html
* mostly standard, but references bundle.js from above
* loadAll function decrypts the encrypted key using the functions in bundle.js
* line: '''unityInstance.SendMessage("UserID", "SetFV_ID", theID);''' sends decrypted web3 address to unity binary

## 2. Modifications to the Unity example

The following section discusses changes this example makes to the boilerplate demo provided by Unity.

### 2.1 Scenes

CharacterSelect.unity 
* new scene which permits the player to select a character

LegoTutorial.unity
* modified to include all four character 'Minifigs' at startup, which are disabled/enabled based on selected character
* modified GameManager object to include a new ConfigStatic component (see scripts section below)

MenuIntro.unity
* modified to include a button to go to select character screen

### 2.2 Scripts

ConfigStatic.cs
* Static configuration file, stores freeverse API URL, universe ID, and character variables to be passed from CharacterSelect scene to LegoTutorial scene

Character/GraphQL.cs
* super simple GraphQL client for Unity, doesn't even support variables! 

Character/CharacterSetup.cs
* Contains classes for receiving parsed GraphQL data. Recommend using https://json2csharp.com/ to create these classes.
* Recommend testing queries first in GraphQL playground of FreeverseAPI
* Contains query for getting assets belonging to web3 address of user
* Parses this query and passes information to CharacterDisplay component

Character/CharacterDisplay.cs
* Fills information on each character within the elements in the scene
* Play button calls LoadSceneButtonFromCharacterSelect.cs

Character/LoadSceneButtonFromCharacterSelect.cs
* Passes information from selected CharacterDisplay to ConfigStatic.js
* Loads LegoTutorial scene

Character/SceneCharacterActivator.cs
* within LegoTutorialScene
* reads ConfigStatic.cs, activates/deactivates relevant characters in scene, sets jump and speed parameters

