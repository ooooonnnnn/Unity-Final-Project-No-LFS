using UnityEngine;

public interface ISpellable
{
    void Initialize(SpellComboDefinition combo, Transform caster, Transform target);
    void Initialize(SpellComboDefinition combo, Transform caster);
}
