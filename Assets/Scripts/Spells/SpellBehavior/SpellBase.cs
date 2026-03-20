using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public enum SpellElement
{
    Fire,
    Earth,
    Ice,
    Light,
    Dark
}

public class SpellBase : MonoBehaviour
{
    protected SpellElement Element;
    protected Transform Caster;
    protected float LifeTime = 5f;
    [SerializeField] private ParticleSystem fireParticleSystem;
    [SerializeField] private ParticleSystem iceParticleSystem;
    [SerializeField] private ParticleSystem lightParticleSystem;
    [SerializeField] private ParticleSystem shadowParticleSystem;

    protected virtual void Initialize(SpellElement element, Transform caster)
    {
        Element = element;
        this.Caster = caster;
        switch (Element)
        {
            case SpellElement.Fire:
                fireParticleSystem.Play();
                break;
            case SpellElement.Earth:
                break;
            case SpellElement.Ice:
                iceParticleSystem.Play();
                break;
            case SpellElement.Light:
                lightParticleSystem.Play();
                break;
            case SpellElement.Dark:
                shadowParticleSystem.Play();
                break;
            default:
                print("Unknown spell element: " + Element);
                break;
        }

        StartCoroutine(DestroyAfterLifeTime());
    }

    IEnumerator DestroyAfterLifeTime()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }
}