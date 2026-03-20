using System;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject areaOfEffectPrefab;
    [SerializeField] private GameObject homingProjectilePrefab;
    
    public void CastSpell(SpellComboDefinition combo, Transform target = null)
    {
        
    }
}
