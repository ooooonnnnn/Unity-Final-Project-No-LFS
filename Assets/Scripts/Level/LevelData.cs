using UnityEditor;
using UnityEngine;


    [CreateAssetMenu(menuName = "Game/Level Data")]
    public class LevelData : ScriptableObject
    {
        public string levelName;
        public SceneAsset levelScene;
        public WaveData[] waves;
    
        public int levelIndex;
    }
