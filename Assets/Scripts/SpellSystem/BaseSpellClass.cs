using System.Collections;
using UnityEngine;

public class BaseSpellClass : MonoBehaviour, ISpell
{
    public Element Element { get; set; }
    public SpellType SpellType { get; set; }
    public int Damage { get; set; }
    public int ManaCost { get; set; }
    public float Lifespan { get; set; }

    // Set this from the spawner / caster when spawning the spell
    public GameObject Caster { get; set; }

    // Movement speed for projectile spells
    public float Speed { get; set; } = 15f;

    private Rigidbody rb;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Projectiles shouldn't be affected by gravity by default
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        switch (SpellType)
        {
            case SpellType.Projectile:
                ProjectileSetup();
                break;

            case SpellType.AreaOfEffect:
                // Implement AoE behavior
                break;
            case SpellType.Strike:
                // Implement strike behavior
                break;
            case SpellType.Reinforce:
                // Implement reinforce behavior
                break;
            default:
                Debug.LogWarning("Spell type not recognized: " + SpellType);
                break;
        }

        StartCoroutine(DeleteAfterLifetime());
    }

    private IEnumerator DeleteAfterLifetime()
    {
        yield return new WaitForSeconds(Lifespan);
        SpellPool.ReturnSpell(this);
    }

    private void ProjectileSetup()
    {
        if (Caster != null)
        {
            // Place spell at caster position and align forward
            transform.position = Caster.transform.position;
            transform.forward = Caster.transform.forward;

            // Give initial velocity so it moves forward until collision
            rb.linearVelocity = transform.forward * Speed;
        }
        else
        {
            Debug.LogWarning("Projectile spell spawned without a Caster set. Using current transform for direction.");
            rb.linearVelocity = transform.forward * Speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ignore collisions with the caster (and its children)
        if (Caster != null)
        {
            if (collision.gameObject == Caster || collision.transform.IsChildOf(Caster.transform))
                return;
        }

        // TODO: apply damage/effects to collision.gameObject here

        SpellPool.ReturnSpell(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Same logic for trigger-based collisions
        if (Caster != null)
        {
            if (other.gameObject == Caster || other.transform.IsChildOf(Caster.transform))
                return;
        }

        // TODO: apply damage/effects to other.gameObject here

        SpellPool.ReturnSpell(this);
    }
}