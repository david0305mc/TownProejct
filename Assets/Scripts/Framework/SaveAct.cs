using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.Storage;

public abstract class SaveAct
{
    protected abstract string SaveKey { get; }
    protected abstract string SaveData { get; }

    protected virtual void Save()
    {
        ObscuredPrefs.SetString(SaveKey, SaveData);
    }

    protected virtual string Load()
    {
        return ObscuredPrefs.GetString(SaveKey);
    }
}
