using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;

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
        saveData.Save();
    }

    public void Load()
    {
        saveData = SaveData.Load();
    }
    [Button]
    public void ResetSave()
    {
        PlayerPrefs.DeleteKey(PlayerPrefKeys.Save);
    }
}
