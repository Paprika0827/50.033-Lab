using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.SearchService;
using UnityEngine;

[Serializable]
public class LevelData {
    public int level;
    public string Scene;
    public Vector3 MarioPosition = new Vector3(0, 0, 0);
    public Vector3[] GoombaPosition = {    };
}

public static class Levels {
    public static LevelData[] levels;
}