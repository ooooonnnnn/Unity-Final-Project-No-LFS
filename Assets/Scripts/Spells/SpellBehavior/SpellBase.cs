using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public enum SpellElement
{
    Fire,
    Ice,
    Light,
    Dark
}

public class SpellBase : MonoBehaviour
{
    public SpellElement element;
    [SerializeField] private ParticleSystem fireParticlePrefab;
    [SerializeField] private ParticleSystem iceParticlePrefab;
    // Add more particle prefabs for other elements as needed
    private ParticleSystem activeParticlePrefab;
    protected SpellDeliveryCategory SpellType;
    protected Transform Caster;
    private const float LifeTime = 5f;

    protected virtual void Awake()
    {
        this.Caster = gameObject.transform.parent;
        gameObject.transform.parent = null; // Detach from caster to allow independent movement
        transform.position = Caster.position;
        transform.rotation = Caster.rotation;

        switch (element)
        {
            case SpellElement.Fire:
                    activeParticlePrefab = Instantiate(fireParticlePrefab, transform.position, transform.rotation);
                    activeParticlePrefab.transform.parent = transform;
                break;
            case SpellElement.Ice:
                    activeParticlePrefab = Instantiate(iceParticlePrefab, transform.position, transform.rotation);
                    activeParticlePrefab.transform.parent = transform;
                break;
            case SpellElement.Light:
            case SpellElement.Dark:
                // Instantiate corresponding particle effects for Earth, Light, and Dark elements
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        StartCoroutine(DestroyAfterLifeTime());
    }
    IEnumerator DestroyAfterLifeTime()
    {
        yield return new WaitForSeconds(LifeTime);
        SelfDestruct();
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        other.gameObject.TryGetComponent<ITakeSpellData>(out var component);
        if (other.transform == Caster) return;
        component?.TakeSpellData(element, SpellType);
        SelfDestruct();
    }

    protected void SelfDestruct()
    {
        activeParticlePrefab.transform.parent = null;
        activeParticlePrefab.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        Destroy(gameObject);
    }
}