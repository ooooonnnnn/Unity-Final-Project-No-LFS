using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class StrikeBehavior : SpellBase
{
    public Vector3 targetPosition = Vector3.zero;
    [SerializeField] private float arcHeight = 5f;
    [SerializeField] private Rigidbody rb;

    private Vector3 startPosition;
    private float progress;

    protected override void Awake()
    {
        base.Awake();
        ActiveParticlePrefab.Pause();
        startPosition = transform.position;
    }

    public void Update()
    {
        if (progress >= 2)
        {
            StartCoroutine(Explode());
            enabled = false;
            return;
        }

        progress = Mathf.Min(progress + Time.deltaTime, 2);
        Vector3 horizontalPosition = Vector3.Lerp(startPosition, targetPosition, progress / 2);
        float height = Mathf.Sin(progress / 2 * Mathf.PI) * arcHeight;
        transform.position = new Vector3(horizontalPosition.x, horizontalPosition.y + height, horizontalPosition.z);
    }

    // Spawn the area of effect zone and apply spell effects to targets within the zone
    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Spell")) return; // Ignore collisions with other spells
        if (ignorePlayer && other.gameObject.CompareTag("Player")) return;
        if (ignoreEnemies && other.gameObject.CompareTag("Enemy")) return;
        print("Strike collided with " + other.gameObject.name);
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        gameObject.transform.rotation = quaternion.identity;
        rb.isKinematic = true; // Stop movement
        ActiveParticlePrefab.transform.position = transform.position;
        ActiveParticlePrefab.Play();
        Collider[] targets = Physics.OverlapSphere(transform.position, spellCombo.radius / 2f);

        foreach (var target in targets)
        {
            target.TryGetComponent<ITakeSpellData>(out var taker);
            taker?.TakeSpellData(spellCombo);
        }
        
        yield return new WaitForSeconds(1f);

        SelfDestruct();
    }
    
    public override void ChangeElement(SpellComboDefinition newCombo)
    {
        if (spellCombo.spellType != newCombo.spellType) return;
        
        spellCombo = newCombo;
        if (ActiveParticlePrefab) Destroy(ActiveParticlePrefab);
        InstantiateActiveParticle(true);
    }
}