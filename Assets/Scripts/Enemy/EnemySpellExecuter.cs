using Extra;
using Interface;
using Spells.EditorTool;
using UnityEngine;

namespace Enemy
{
    public class EnemySpellExecutor : MonoBehaviour
    {
        [SerializeField] private SpellComboDefinition spellCombo;

        public SpellComboDefinition SpellCombo => spellCombo;

        public void Execute(Transform caster, Transform target, Transform firePoint = null)
        {
            if (!spellCombo)
            {
                Debug.LogWarning($"{name}: No SpellComboDefinition assigned.");
                return;
            }
        
            if (spellCombo.prefab)
            {
                Vector3 spawnPosition = firePoint ? firePoint.position : caster.position;
                Quaternion spawnRotation = firePoint ? firePoint.rotation : caster.rotation;

                GameObject spellObject = ProjectilePool.Instance.Get(spellCombo.prefab);
                spellObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);

                if (spellObject.TryGetComponent<ISpellable>(out var spellBehaviour))
                {
                    spellBehaviour.Initialize(spellCombo, caster, target);
                }
            }
            else
            {
                ApplyDirectEffect(target);
            }
        }

        private void ApplyDirectEffect(Transform target)
        {
            if (!target)
                return;

            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(spellCombo.powerMultiplier);
            }

            if (spellCombo.appliesBurn)
            {
                Debug.Log("Apply Burn");
            }

            if (spellCombo.appliesFreeze)
            {
                Debug.Log("Apply Freeze");
            }

            if (spellCombo.appliesPoison)
            {
                Debug.Log("Apply Poison");
            }

            if (spellCombo.healsTarget)
            {
                Debug.Log("Heal Target");
            }

            if (spellCombo.grantsShield)
            {
                Debug.Log("Grant Shield");
            }
        }
    }
}

