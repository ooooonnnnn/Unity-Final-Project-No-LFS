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

    protected override void Initialize(SpellElement element, Transform caster)
    {
        base.Initialize(element, caster);
        areaOfEffectZone.SetActive(false);
        Element = element;
        Caster = caster;
        transform.position = caster.position;
        transform.rotation = caster.rotation;
        startPosition = transform.position;
        progress = 0f;
        targetPosition = caster.position + caster.forward * 10f;
    }
    public void Update()
    {
        if (!(progress < 1f)) return;
        progress += Time.deltaTime;

        Vector3 horizontalPosition = Vector3.Lerp(startPosition, targetPosition, progress);

        float height = Mathf.Sin(progress * Mathf.PI) * arcHeight;

        transform.position = new Vector3(horizontalPosition.x, horizontalPosition.y + height, horizontalPosition.z);
    }

    // Collision detection
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == Caster) return;
        areaOfEffectZone.transform.position = transform.position;
        areaOfEffectZone.SetActive(true);
        Collider[] colliders = Physics.OverlapSphere(transform.position, areaOfEffectZone.transform.lossyScale.x / 2f);

        foreach (var collider in colliders)
        {
            var taker = collider.GetComponent<ITakeSpellCombo>();
            // Give all components that can take the spell combo information
        }
    }

}