using System.Collections.Generic;
using UnityEngine;


public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;

    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private int poolSize = 30;

    private Queue<Projectile> pool = new();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            Projectile proj = Instantiate(projectilePrefab);
            proj.gameObject.SetActive(false);
            pool.Enqueue(proj);
        }
    }

    public Projectile Get()
    {
        if (pool.Count == 0)
        {
            Projectile proj = Instantiate(projectilePrefab);
            return proj;
        }

        Projectile projectile = pool.Dequeue();
        projectile.gameObject.SetActive(true);

        return projectile;
    }

    public void Return(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        pool.Enqueue(projectile);
    }
}