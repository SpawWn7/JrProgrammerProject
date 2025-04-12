using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    public Color TeamColor;

    private void Awake()
    {

        if (Instance != null) // If an instance allready exists then we destroy the new instance that is attempting to be initalized. This prevents having multiple instances of the MainManager gameObject, effectively a singleton pattern or design.
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // This prevents the gameObject from being destroyed when moving/transitioning/loading/ from scene to scene

        LoadColor();
    }

    [System.Serializable] // We are creating a class that is serializable so it can be converted to a json format. We want to save the color chosen by the user is json so we can load it back in between sessions.
    class SaveData
    {
        public Color TeamColor;
    }

    public void SaveColor() 
    {
        SaveData data = new SaveData();
        data.TeamColor = TeamColor;

        string json = JsonUtility.ToJson(data); // We convert the data into a json format 

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json); // We save the json string into a save file that is json formattted. We achieve this by writing onto a file given its path.
    }

    public void LoadColor()
    {
        string path = Application.persistentDataPath + "/savefile.json"; // We retrieve the file path that contains our saved data

        if (File.Exists(path)) // If a json file with that path exists then we continue
        {
            string json = File.ReadAllText(path); // We retrieve the json string saved within the file given it's path
            SaveData data = JsonUtility.FromJson<SaveData>(json); // We un-jsonfy the string and convert back into a Savedata instance 

            TeamColor = data.TeamColor; // We set the color based on the last saved color data
        }
    }
}
