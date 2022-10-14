using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SaveManager : Singleton<SaveManager>
{
    public SaveData saveData;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Load();
    }
    public void Save()
    {
        PlayerPrefs.SetString(PlayerPrefKeys.Save, SaveHelper.Serialize<SaveData>(saveData));
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(PlayerPrefKeys.Save))
        {
            saveData = SaveHelper.Deserialize<SaveData>(PlayerPrefs.GetString(PlayerPrefKeys.Save));
        }
        else
        {
            saveData = new SaveData();
            Save();
            Debug.Log("No new save file found creating new one");
        }
    }
    [Button]
    public void ResetSave()
    {
        PlayerPrefs.DeleteKey(PlayerPrefKeys.Save);
    }
}
