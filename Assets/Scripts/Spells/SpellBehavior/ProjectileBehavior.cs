using System;
using ScriptsMilana;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.Serialization;

public class ProjectileBehavior : SpellBase
{
    public float projectileSpeed = 10f;

    private void Update()
    {
        transform.position += transform.forward * (projectileSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.TryGetComponent<ITakeSpellCombo>(out var component);
        if (other.transform == Caster || component == null) return;
        // Give the component the spell combo information
        Destroy(gameObject);
    }

}