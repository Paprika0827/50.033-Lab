using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelParams", menuName = "Pro Platformer/Level", order = 1)]
public class Level : ScriptableObject {
    public bool autoReload = false;
    public LevelData[] levels;
    private Action reloadCallback;
    public void SetReloadCallback(Action onReload) {
        this.reloadCallback = onReload;
    }

    public void OnValidate() {
        if (autoReload) {
            ReloadParams();
        }
    }

    public void ReloadParams() {
        Debug.Log("=======更新所有Level配置参数");
        Levels.levels = levels;
    }
}