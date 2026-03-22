using UnityEngine;

public class DebugSpell : MonoBehaviour, ITakeSpellData
{
    [SerializeField] private Material material;

    public void TakeSpellData(SpellComboDefinition combo)
    {
        material.color = Color.red;
    }
}
