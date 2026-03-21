using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

    public class LevelManager : MonoBehaviour
    {

        public event Action OnLevelLoaded;
        
        public static LevelManager Instance;
        
        
        public string CurrentLevelName { get; private set; }

        public int CurrentLevelIndex { get; private set; }

        [SerializeField] private LevelDatabase levelScenes;
        
        
        private LevelData currentScene;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            //TODO remove post dev
            foreach (var scene in levelScenes.levels)
            {
                if (scene.name == SceneManager.GetActiveScene().name)
                {
                    currentScene = scene;
                    CurrentLevelName = scene.name;
                    break;
                }
            }
            
            DontDestroyOnLoad(gameObject);
        }

        public void LoadLevel(int levelIndex)
        {
            CurrentLevelIndex = levelIndex;
            currentScene = levelScenes.levels[levelIndex];
            CurrentLevelName = currentScene.name;            
            SceneManager.LoadScene(levelIndex);
            OnLevelLoaded?.Invoke();
        }
    
        public void ReturnToLevelSelect()
        {
            SceneManager.LoadScene("LevelSelect");
        }
    }
