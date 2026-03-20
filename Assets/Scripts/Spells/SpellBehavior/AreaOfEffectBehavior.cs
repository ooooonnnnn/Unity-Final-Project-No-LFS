using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AreaOfEffectBehavior : MonoBehaviour, ISpellable
{
    public SpellComboDefinition Combo { get; private set; }
    [SerializeField] private GameObject areaOfEffectZone;
    private Transform caster;

    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float arcHeight = 5f;

    private Vector3 startPosition;
    private float progress;

    public void Initialize(SpellComboDefinition combo, Transform caster, Transform target)
    {
        areaOfEffectZone.SetActive(false);
        Combo = combo;
        this.caster = caster;
        transform.position = caster.position;
        transform.rotation = caster.rotation;
        startPosition = transform.position;
        progress = 0f;
        targetPosition = target.position;
    }

    public void Initialize(SpellComboDefinition combo, Transform caster)
    {
        areaOfEffectZone.SetActive(false);
        Combo = combo;
        this.caster = caster;
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
        if (collision.transform == caster) return;
        areaOfEffectZone.transform.position = transform.position;
        areaOfEffectZone.SetActive(true);
        Collider[] colliders = Physics.OverlapSphere(transform.position, areaOfEffectZone.transform.lossyScale.x / 2f);

        foreach (var collider in colliders)
        {
            var taker = collider.GetComponent<ITakeSpellCombo>();
            taker?.OnComboReceived(Combo);
        }
    }

}