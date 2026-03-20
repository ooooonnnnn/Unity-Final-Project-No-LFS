using System;
using ScriptsMilana;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.Serialization;

public class ProjectileBehavior : MonoBehaviour, ISpellable
{
    public SpellComboDefinition Combo { get; private set; }
    private Transform caster;
    public float projectileSpeed = 10f;

    public void Initialize(SpellComboDefinition combo, Transform caster, Transform target)
    {
        throw new NotImplementedException();
    }

    public void Initialize(SpellComboDefinition combo, Transform caster)
    {
        Combo = combo;
        this.caster = caster;
        transform.position = caster.position;
        transform.rotation = caster.rotation;
    }
    
    private void Update()
    {
        transform.position += transform.forward * (projectileSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.TryGetComponent<ITakeSpellCombo>(out var component);
        if (other.transform == caster || component == null) return;
        component.OnComboReceived(Combo);
        Destroy(gameObject);
    }
}