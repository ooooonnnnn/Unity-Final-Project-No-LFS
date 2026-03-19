using UnityEngine;

public enum SpellDeliveryCategory
{
    Projectile,
    Shield,
    Area,

}

[CreateAssetMenu(fileName = "SpellType", menuName = "Spells/Spell Type Definition")]
public class SpellTypeDefinition : ScriptableObject
{
    [Header("Base Info")]
    public string typeId = "new_type";
    public string displayName = "New Spell Type";
    public SpellDeliveryCategory deliveryCategory = SpellDeliveryCategory.Projectile;
    [TextArea] public string description;

    [Header("Example Future Parameters")]
    public float baseCooldown = 1f;
    public float manaCost = 10f;
}