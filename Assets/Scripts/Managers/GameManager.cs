using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject spawnPoint;

    [SerializeField] private InputManager inputManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private UIManager uiManager;


    private EnemySpawner enemySpawner;
    private PlayerController player;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        SaveSystem.Load();
        
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        levelManager.OnLevelLoaded += LoadLevelData;
    }

    private void OnDisable()
    {
        levelManager.OnLevelLoaded -= LoadLevelData;
    }

    private void Start()
    {
        Debug.Log("Game started, current level: " + levelManager.CurrentLevelName);
        
        
        
        
    }

    private void LoadLevelData()
    {
        player = PlayerController.Instance;
        player.OnPlayerDied += GameLost;
        enemySpawner = EnemySpawner.Instance;
        enemySpawner.OnWaveCompleted += LevelWon;
    }

    private void LevelWon()
    {
        SaveSystem.UnlockNextLevel(levelManager.CurrentLevelIndex);
        uiManager.LevelWon();
    }

    private void GameLost()
    {
    }

    public void LoadLevel(int levelIndex)
    {
        throw new System.NotImplementedException();
    }
}