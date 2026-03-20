using UnityEngine;

namespace Spells.EditorTool
{
    [CreateAssetMenu(fileName = "SpellCombo", menuName = "Spells/Spell Combo Definition")]
    public class SpellComboDefinition : ScriptableObject
    {
        [Header("Links")]
        public SpellElementDefinition element;
        public SpellTypeDefinition spellType;

        [Header("Base Combo Info")]
        public string comboId = "new_combo";
        public string displayName = "New Combo";
        [TextArea] public string description;

        [Header("Gameplay Parameters")]
        public bool isUnlockedByDefault = true;
        public float powerMultiplier = 1f;
        public float duration = 1f;
        public float speed = 10f;
        public float radius = 1f;
        public GameObject prefab;

        [Header("Example Flags")]
        public bool appliesBurn;
        public bool appliesFreeze;
        public bool appliesPoison;
        public bool grantsShield;
        public bool healsTarget;

        public void AutoNameFromReferences()
        {
            string elementName = element != null ? element.displayName : "NoElement";
            string typeName = spellType != null ? spellType.displayName : "NoType";

            displayName = $"{elementName} {typeName}";
            comboId = $"{Sanitize(elementName)}_{Sanitize(typeName)}";
        }

        private string Sanitize(string value)
        {
            return value.Trim().ToLower().Replace(" ", "_");
        }
    }
}