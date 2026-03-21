
using UnityEngine;

public class ProjectileBehavior : SpellBase
{
    public float projectileSpeed = 10f;


    private void Update()
    {
        transform.position += transform.forward * (projectileSpeed * Time.deltaTime);
    }
}