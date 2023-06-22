using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    // Create a field for the save file.
    string saveFile;
    public static GameDataManager i {get; private set; }
    // Create a GameData field.
    GameData gameData = new GameData();

    public GameData GameData {get => gameData; set => gameData = value;}
    
    void Awake()
    {
        i = this;
        Debug.Log("PATH = " + Application.persistentDataPath);
        // Update the path once the persistent path exists.
        saveFile = Application.persistentDataPath + "/gamedata.json";
    }
    public void readFile()
    {
        // Does the file exist?
        if (File.Exists(saveFile))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(saveFile);

            // Deserialize the JSON data 
            //  into a pattern matching the GameData class.
            gameData = JsonUtility.FromJson<GameData>(fileContents);
        }
    }

    public void writeFile()
    {
        // Serialize the object into JSON and save string.
        string jsonString = JsonUtility.ToJson(gameData);

        // Write JSON to file.
        File.WriteAllText(saveFile, jsonString);
        //File.AppendAllText(saveFile, jsonString);
    }
}