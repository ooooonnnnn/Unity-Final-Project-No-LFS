using System;
using UnityEngine;
using UnityEngine.Serialization;

public class StrikeBehavior : SpellBase
{
    [SerializeField] private GameObject areaOfEffectZone;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float arcHeight = 5f;

    private Vector3 startPosition;
    private float progress;

    protected override void Awake()
    {
        base.Awake();
        SpellType = SpellDeliveryCategory.Strike;
    }

    public void Update()
    {
        if (!(progress < 1f)) return;
        progress += Time.deltaTime;

        Vector3 horizontalPosition = Vector3.Lerp(startPosition, targetPosition, progress);

        float height = Mathf.Sin(progress * Mathf.PI) * arcHeight;

        transform.position = new Vector3(horizontalPosition.x, horizontalPosition.y + height, horizontalPosition.z);
    }

    // Spawn the area of effect zone and apply spell effects to targets within the zone
    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == Caster) return;
        areaOfEffectZone.transform.position = transform.position;
        areaOfEffectZone.SetActive(true);
        Collider[] targets = Physics.OverlapSphere(transform.position, areaOfEffectZone.transform.lossyScale.x / 2f);

        foreach (var target in targets)
        {
            target.TryGetComponent<ITakeSpellData>(out var taker);
            taker?.TakeSpellData(element, SpellType);
        }

        SelfDestruct();
    }
}