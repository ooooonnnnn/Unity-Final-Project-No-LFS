using Save;
using UnityEngine;

namespace Level
{
    public class GameBoot : MonoBehaviour
    {
        private void Awake()
        {
            SaveSystem.Load();
        }
    }
}
