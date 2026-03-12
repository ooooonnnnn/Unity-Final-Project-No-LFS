using UnityEngine;

namespace ScriptsMilana
{
    [CreateAssetMenu(menuName = "Game/Level Database")]
    public class LevelDatabase : ScriptableObject
    {
        public LevelData[] levels;
    }
}