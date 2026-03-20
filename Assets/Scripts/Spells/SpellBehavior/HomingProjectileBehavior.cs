using UnityEngine;

public class HomingProjectileBehavior : MonoBehaviour
{
    public SpellComboDefinition Combo { get; private set; }
    private Transform caster;
    private Transform target;
    public float projectileSpeed = 10f;
    public float turnSpeed = 5f;

    public void Initialize(SpellComboDefinition combo, Transform caster, Transform target)
    {
        Combo = combo;
        this.caster = caster;
        this.target = target;
        transform.position = caster.position;
        transform.rotation = caster.rotation;
    }

    public void Initialize(SpellComboDefinition combo, Transform caster)
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        transform.position += transform.forward * (projectileSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.TryGetComponent<ITakeSpellCombo>(out var component);
        if (other.transform == caster || component == null) return;
        component.OnComboReceived(Combo);
        Destroy(gameObject);
    }
}
