using System;
using Managers;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    //[SerializeField] private GameObject areaOfEffectPrefab;
    [SerializeField] private GameObject strikePrefab;
    //[SerializeField] private GameObject shieldPrefab;
    [SerializeField] private SpellComboDefinition[] spellCombos;
    
    [SerializeField] private bool ignorePlayer = false;
    [SerializeField] private bool ignoreEnemies = false;
    
    private ProjectileBehavior projectileBehavior;
    
    private AreaOfEffectBehavior areaOfEffectCombo;
    
    private StrikeBehavior strikeBehavior;

    private void Awake()
    {
        projectileBehavior = projectilePrefab.GetComponent<ProjectileBehavior>();
        projectileBehavior.ignorePlayer = ignorePlayer;
        projectileBehavior.ignoreEnemies = ignoreEnemies;
        
        strikeBehavior = strikePrefab.GetComponent<StrikeBehavior>();
        strikeBehavior.ignorePlayer = ignorePlayer;
        strikeBehavior.ignoreEnemies = ignoreEnemies;
        
        //InputManager.Instance.OnRightClick.AddListener(() => CastSpell(spellCombos[0]));
    }

    public void CastSpell()
    {
        CastSpell(spellCombos[0]);
    }
    
    public void CastSpell(SpellComboDefinition combo)
    {
        if (!combo) return;
        switch (combo.spellType.spellTypeEnum)
        {
            case SpellDeliveryCategory.Projectile:
                projectileBehavior.ChangeElement(combo);
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                break;
            case SpellDeliveryCategory.AOE:
                //areaOfEffectCombo = combo;
                //Instantiate(areaOfEffectPrefab, transform.position, Quaternion.identity);
                break;
            case SpellDeliveryCategory.Strike:
                strikeBehavior.ChangeElement(combo);
                Instantiate(strikePrefab, transform.position, Quaternion.identity);
                break;
            case SpellDeliveryCategory.Shield:
                //shieldCombo = combo;
                //Instantiate(shieldPrefab, transform.position, Quaternion.identity);
                break;
            default:
                print("Invalid spell type");
                break;
        }
    }
}