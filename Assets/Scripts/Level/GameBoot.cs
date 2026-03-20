using UnityEngine;

namespace ScriptsMilana
{
    public class GameBoot : MonoBehaviour
    {
        private void Awake()
        {
            SaveSystem.Load();
        }
    }
}
