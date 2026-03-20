using UnityEngine;

namespace Spells.EditorTool
{
    [CreateAssetMenu(fileName = "SpellElement", menuName = "Spells/Element Definition")]
    public class SpellElementDefinition : ScriptableObject
    {
        [Header("Base Info")]
        public string elementId = "new_element";
        public string displayName = "New Element";
        public Color elementColor = Color.white;
        [TextArea] public string description;

        [Header("Example Future Parameters")]
        public float basePowerModifier = 1f;
        public float statusChance = 0f;
    }
}