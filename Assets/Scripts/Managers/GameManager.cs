using System.Collections.Generic;
using Camera;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public GameObject spawnPoint;

        [SerializeField] private InputManager inputManager;
        [SerializeField] private CharacterComponents selectedCharacter;


        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            selectedCharacter.OnPlayerDied += PlayerLost;
            EnemySpawner.OnWaveCompleted += PlayerWon;
        }

        private void OnDisable()
        {
            selectedCharacter.OnPlayerDied -= PlayerLost;
            EnemySpawner.OnWaveCompleted -= PlayerWon;
        }

        private void PlayerWon()
        {
            SceneManager.LoadScene("LevelSelect");
        }


        private void PlayerLost()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}