using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spells.EditorTool
{
    [CreateAssetMenu(fileName = "SpellDatabase", menuName = "Spells/Spell Database")]
    public class SpellDatabase : ScriptableObject
    {
        public List<SpellElementDefinition> elements = new();
        public List<SpellTypeDefinition> spellTypes = new();
        public List<SpellComboEntry> combos = new();

        [Serializable]
        public class SpellComboEntry
        {
            public SpellElementDefinition element;
            public SpellTypeDefinition spellType;
            public SpellComboDefinition combo;
        }

        public SpellComboDefinition GetCombo(SpellElementDefinition element, SpellTypeDefinition spellType)
        {
            for (int i = 0; i < combos.Count; i++)
            {
                SpellComboEntry entry = combos[i];
                if (entry.element == element && entry.spellType == spellType)
                {
                    return entry.combo;
                }
            }

            return null;
        }

        public SpellComboEntry GetComboEntry(SpellElementDefinition element, SpellTypeDefinition spellType)
        {
            for (int i = 0; i < combos.Count; i++)
            {
                SpellComboEntry entry = combos[i];
                if (entry.element == element && entry.spellType == spellType)
                {
                    return entry;
                }
            }

            return null;
        }

        public void SetCombo(SpellElementDefinition element, SpellTypeDefinition spellType, SpellComboDefinition combo)
        {
            SpellComboEntry existing = GetComboEntry(element, spellType);

            if (existing != null)
            {
                existing.combo = combo;
                return;
            }

            combos.Add(new SpellComboEntry
            {
                element = element,
                spellType = spellType,
                combo = combo
            });
        }

        public void RemoveNullReferences()
        {
            elements.RemoveAll(x => x == null);
            spellTypes.RemoveAll(x => x == null);

            combos.RemoveAll(entry =>
                entry == null ||
                entry.element == null ||
                entry.spellType == null);
        }
    }
}