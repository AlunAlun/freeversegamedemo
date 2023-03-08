using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphQL;
using UnityEngine.Networking;
using Newtonsoft.Json;

/*
 * Classes for deserialing raw GraphQL response
 */
public class AllAssets
{
    public List<Node> nodes { get; set; }
}

public class Data
{
    public AllAssets allAssets { get; set; }
}

public class Node
{
    public string id { get; set; }
    public string ownerId { get; set; }
    public string props { get; set; }
}

public class Root
{
    public Data data { get; set; }
}

/*
 * Classes for Deserialing asset properties
 */
public class Attribute
{
    public string trait_type { get; set; }
    public string value { get; set; }
}

public class RootProps
{
    public string name { get; set; }
    public string description { get; set; }
    public string image { get; set; }
    public int collectionId { get; set; }
    public List<Attribute> attributes { get; set; }
}


public class CharacterSetup : MonoBehaviour
{
    public CharacterDisplay[] characterDisplays;
    public GameObject loginButton;
    public GameObject marketButton;


    // Start is called before the first frame update
    void Start()
    {
        //test values, ignore these!
        //ConfigStatic.FV_id = "0x47032f4dBCeD2dE3c267ee749e2E206268Ebaf06";
        //ConfigStatic.FV_id = "0x2e8d99C867496ea899a19AFF0129841087ecF66B";
        //ConfigStatic.FV_id = "0xA23c93D7C8FDf6C5136E3d4CdC0664AF9Ed7265B";

        //Hide all character displays first
        foreach (CharacterDisplay cd in characterDisplays)
            cd.gameObject.SetActive(false);

        //return if no user
        if (ConfigStatic.FV_id == null) {
            return;
        }
        
        //deactivate default buttons
        loginButton.SetActive(false);
        marketButton.SetActive(false);

        //make call to API
        StartCoroutine (QueryCall( (bool success) => {
            if (success)
            Debug.Log( "success!");
            else
            Debug.Log( "fail!");
        }));
    }

    //this function calls the Freeverse API (url in ConfigStatic.js) and get the assets
    //that the current user has, within the universe of the game (universe ID also set in ConfigStatic.js)
    public IEnumerator QueryCall (System.Action<bool> callback) {
        
        string query = "query {allAssets(condition: {universeId:"+ConfigStatic.FV_UniID+", ownerId: \"" +ConfigStatic.FV_id+"\"}) {nodes {id ownerId props }}}";

        GraphQLClient client = new GraphQLClient (ConfigStatic.FV_ApiUrl);

        using( UnityWebRequest www = client.Query(query, "{}", "")) {
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError) {
            Debug.Log (www.error);

            callback (false);
        } else {

            SetupCharacters(www.downloadHandler.text);
            
            callback (true);
        }
        }
    }

    //parses GraphQL response and gets information about character
    public void SetupCharacters(string responseString) 
    {

        //deserialize core JSON response
        Root root = JsonConvert.DeserializeObject<Root>(responseString);
        
        //we only can show a max of 4 characters on screen at the moment
        int maxOwnedCharacters = (root.data.allAssets.nodes.Count > 4 ? 4 : root.data.allAssets.nodes.Count);

        if (maxOwnedCharacters == 0)
            marketButton.SetActive(true);

        //loop all characters
        for (int i = 0; i < maxOwnedCharacters; i++) {
            //deserialize asset props for this character
            RootProps rootProps = JsonConvert.DeserializeObject<RootProps>(root.data.allAssets.nodes[i].props);
           
            //default character style
            CharacterStyle newStyle = CharacterStyle.Astronaut; 
            string newSpeed = "-1";
            string newJump = "-1";

            //error handling
            if (rootProps.attributes == null){
                characterDisplays[i].gameObject.SetActive(true);
                characterDisplays[i].SetCharacter(newStyle, rootProps.name, newSpeed, newJump, "-1");
                continue;
            }

            //loop and extract all properties
            for (int j = 0; j < rootProps.attributes.Count; j++) {
                string trait_type = rootProps.attributes[j].trait_type;
                string value = rootProps.attributes[j].value;
                switch (trait_type) {
                    case "style":
                        switch (value) {
                            case "adventurer":
                                newStyle = CharacterStyle.Adventurer;
                                break;
                            case "astronaut":
                                newStyle = CharacterStyle.Astronaut;
                                break;
                            case "pizza":
                                newStyle = CharacterStyle.Pizza;
                                break;
                            case "pirate":
                                newStyle = CharacterStyle.Pirate;
                                break;
                        }
                        break;
                    case "speed":
                        newSpeed = value;
                        break;
                    case "jump":
                        newJump = value;
                        break;
                    default:
                        break;
                }
            }
            characterDisplays[i].gameObject.SetActive(true);
            characterDisplays[i].SetCharacter(newStyle, rootProps.name, newSpeed, newJump, root.data.allAssets.nodes[i].id);
        } 
    }
}
