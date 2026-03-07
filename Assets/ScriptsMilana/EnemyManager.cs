using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static readonly List<EnemyBase> enemies = new();
    public static IReadOnlyList<EnemyBase> Enemies => enemies;

    public static void Register(EnemyBase enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public static void Unregister(EnemyBase enemy)
    {
        enemies.Remove(enemy);
    }

    private void Update()
    {
        Debug.Log("Enemies: " + enemies.Count);
        float dt = Time.deltaTime;

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Tick(dt);
        }
        
        
    }
}