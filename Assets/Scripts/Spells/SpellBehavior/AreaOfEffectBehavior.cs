using UnityEngine;

public class AreaOfEffectBehavior : SpellBase
{
    protected override void Awake()
    {
        base.Awake();
        SpellType = SpellDeliveryCategory.AOE;
    }

    // Apply spell effects to targets within the area of effect zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == Caster) return;
        other.TryGetComponent<ITakeSpellData>(out var taker);
        taker?.TakeSpellData(element, SpellType);
    }
    
}
