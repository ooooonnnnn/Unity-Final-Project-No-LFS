using System;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject areaOfEffectPrefab;
    [SerializeField] private GameObject strikePrefab;
    [SerializeField] private GameObject shieldPrefab;
    
    private SpellComboDefinition projectileCombo;
    private Transform projectileTransform;
    
    private SpellComboDefinition areaOfEffectCombo;
    private Transform areaOfEffectTransform;
    
    private SpellComboDefinition strikeCombo;
    private Transform strikeTransform;
    
    private SpellComboDefinition shieldCombo;
    private Transform shieldTransform;

    private void Awake()
    {
        projectileCombo = projectilePrefab.GetComponent<SpellBase>().spellCombo;
        projectileTransform = projectilePrefab.transform;
        
        areaOfEffectCombo = areaOfEffectPrefab.GetComponent<SpellBase>().spellCombo;
        areaOfEffectTransform = areaOfEffectPrefab.transform;
        
        strikeCombo = strikePrefab.GetComponent<SpellBase>().spellCombo;
        strikeTransform = strikePrefab.transform;
        
        shieldCombo = shieldPrefab.GetComponent<SpellBase>().spellCombo;
        shieldTransform = shieldPrefab.transform;
        
        //subscribe to event
    }

    public void CastSpell(SpellComboDefinition combo = null, Transform target = null)
    {
        if (!combo) return;
        switch (combo.spellType.spellTypeEnum)
        {
            case SpellDeliveryCategory.Projectile:
                projectileCombo = combo;
                projectileTransform.parent = gameObject.transform;
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                break;
            case SpellDeliveryCategory.AOE:
                areaOfEffectCombo = combo;
                areaOfEffectTransform.parent = gameObject.transform;
                Instantiate(areaOfEffectPrefab, transform.position, Quaternion.identity);
                break;
            case SpellDeliveryCategory.Strike:
                strikeCombo = combo;
                strikeTransform.parent = gameObject.transform;
                Instantiate(strikePrefab, transform.position, Quaternion.identity);
                break;
            case SpellDeliveryCategory.Shield:
                shieldCombo = combo;
                shieldTransform.parent = gameObject.transform;
                Instantiate(shieldPrefab, transform.position, Quaternion.identity);
                break;
            default:
                print("Invalid spell type");
                break;
        }
    }
}