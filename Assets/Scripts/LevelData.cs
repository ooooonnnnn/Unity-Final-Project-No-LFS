using UnityEngine;

namespace ScriptsMilana
{
    [CreateAssetMenu(menuName = "Game/Level Data")]
    public class LevelData : ScriptableObject
    {
        public string levelName;

        public WaveData wave;
    
        public int levelIndex;
    }
}