using UnityEngine;

    public class GameBoot : MonoBehaviour
    {
        [SerializeField] private ParticleSystem ps;
        private void Awake()
        {
            SaveSystem.Load();
            ps.Play();
        }
    }

