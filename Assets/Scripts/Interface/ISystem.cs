using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public interface ISystem
{
    IEnumerator Initialize();
    public interface IGamePlaySystem
    {
        void Start([CanBeNull] string saveData);
        string GetSaveData();
        void ShutDown();
    }
}
