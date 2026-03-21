using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ProjectileBehavior : SpellBase
{
    public float projectileSpeed = 10f;
    
    protected override void Awake()
    {
        base.Awake();
        SpellType = SpellDeliveryCategory.Projectile;
    }
    
    private void Update()
    {
        transform.position += transform.forward * (projectileSpeed * Time.deltaTime);
    }
}