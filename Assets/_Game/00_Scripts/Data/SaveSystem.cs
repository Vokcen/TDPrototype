using System.IO;
using UnityEngine;

public class SaveSystem
{
    private static readonly string SAVE_FILE_PATH = Application.dataPath + "/Saves/";

    public static void Init()
    {
        if (Directory.Exists(SAVE_FILE_PATH))
        {
            Directory.CreateDirectory(SAVE_FILE_PATH);
        }
    }

    public static void Save(string saveString)
    {
        File.WriteAllText(SAVE_FILE_PATH + "/save.txt", saveString);
    }

    public static string Load()
    {
        if (File.Exists(SAVE_FILE_PATH + "/save.txt"))
        {
            string jsonData = File.ReadAllText(SAVE_FILE_PATH + "/save.txt");
            return jsonData;


        }
        else
        {
            return null;
        }


    }
}
