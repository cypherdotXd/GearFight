using UnityEngine;

[CreateAssetMenu(menuName = "Gears/Rotatable")]
public class Rotatable_SO : ScriptableObject
{
    public virtual Rotatable CreateRotatable()
    {
        return new Rotatable();
    }
}