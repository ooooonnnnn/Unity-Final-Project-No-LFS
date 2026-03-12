using System.IO;
using UnityEngine;

namespace ScriptsMilana
{
    public static class SaveSystem
    {
        private static string SavePath => Application.persistentDataPath + "/save.json";

        private static SaveData CurrentSave { get; set; }

        public static void Load()
        {
            if (File.Exists(SavePath))
            {
                string json = File.ReadAllText(SavePath);
                CurrentSave = JsonUtility.FromJson<SaveData>(json);
            }
            else
            {
                CurrentSave = new SaveData();
                Save();
            }
        }

        public static void Save()
        {
            string json = JsonUtility.ToJson(CurrentSave, true);
            File.WriteAllText(SavePath, json);
        }
        
        public static void UnlockNextLevel(int currentLevel)
        {
            if (currentLevel >= CurrentSave.highestUnlockedLevel)
            {
                CurrentSave.highestUnlockedLevel = currentLevel + 1;
                Save();
            }
        }
    }
}