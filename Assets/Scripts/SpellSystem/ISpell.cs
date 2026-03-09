public interface ISpell
{
    public Element Element { get; set; }
    public SpellType SpellType { get; set; }
    public int Damage { get; set; }
    public int ManaCost { get; set; }
    public float Lifespan { get; set; }
    
}
