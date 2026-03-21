using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }


    [SerializeField] private CharacterComponents _characterComponents;

    [SerializeField] private float health = 100;


    public event Action OnPlayerDied;


    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        
    }

    
}