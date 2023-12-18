using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{


    private void Awake()
    {
        SaveSystem.Init();
    }
    public void SaveGame(GameData gameData)
    {
        string jsonData = JsonUtility.ToJson(gameData);

        SaveSystem.Save(jsonData);
  
    }

    public GameData LoadGame()
    {
        string jsonData = SaveSystem.Load();
        if(jsonData != null) { 
            
         
            return JsonUtility.FromJson<GameData>(jsonData);
        }
        else
        {
            Debug.Log("Save file not found.");
            return null;
        }
    }
}