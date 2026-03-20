using System;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject areaOfEffectPrefab;
    [SerializeField] private GameObject homingProjectilePrefab;
    
    public void CastSpell(SpellComboDefinition combo, Transform target = null)
    {
        if (combo == null) return;
        SpellDeliveryCategory spellType = combo.spellType.deliveryCategory;

        switch (spellType)
        {
            case SpellDeliveryCategory.Projectile:
                GameObject projectile = Instantiate(projectilePrefab);
                break;
            case SpellDeliveryCategory.AOE:
                GameObject areaOfEffect = Instantiate(areaOfEffectPrefab);
                break;
            case SpellDeliveryCategory.Strike:
                GameObject homingProjectile = Instantiate(homingProjectilePrefab);
                break;
            case SpellDeliveryCategory.Shield:
                break;
            default:
                print("Unknown spell type: " + spellType);
                break;
        }
    }
}
