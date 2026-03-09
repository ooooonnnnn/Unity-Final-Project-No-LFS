using UnityEngine;
using UnityEngine.Pool;

public class SpellPool : MonoBehaviour
{
    public static SpellPool Instance { get; private set; }
    private ObjectPool<BaseSpellClass> spellPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void ReturnSpell(BaseSpellClass spell)
    {
        spell.gameObject.SetActive(false);
        Instance.spellPool.Release(spell);
    }
}
