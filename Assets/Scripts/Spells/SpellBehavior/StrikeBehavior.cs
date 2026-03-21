using System;
using UnityEngine;
using UnityEngine.Serialization;

public class StrikeBehavior : SpellBase
{
    [SerializeField] private Color fireColor;
    [SerializeField] private Color iceColor;
    [SerializeField] private Color lightColor;
    [SerializeField] private Color darkColor;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float arcHeight = 5f;

    private Vector3 startPosition;
    private float progress;

    protected override void Awake()
    {
        base.Awake();
        ActiveParticlePrefab.Stop();
    }

    public void LateUpdate()
    {
        if (progress >= spellCombo.duration - 1) return;
        progress += Time.deltaTime;

        Vector3 horizontalPosition =
            Vector3.Lerp(startPosition, targetPosition, progress);

        float height = Mathf.Sin(progress / (spellCombo.duration - 1) * Mathf.PI) * arcHeight;

        transform.position = new Vector3(horizontalPosition.x, horizontalPosition.y + height, horizontalPosition.z);
    }

    // Spawn the area of effect zone and apply spell effects to targets within the zone
    protected override void OnCollisionEnter(Collision collision)
    {
        //if (collision.transform == Caster) return;
        ActiveParticlePrefab.Play();
        Collider[] targets = Physics.OverlapSphere(transform.position, spellCombo.radius / 2f);

        foreach (var target in targets)
        {
            target.TryGetComponent<ITakeSpellData>(out var taker);
            taker?.TakeSpellData(spellCombo);
        }

        SelfDestruct();
    }
}