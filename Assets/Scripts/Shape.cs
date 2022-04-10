using UnityEngine;

public abstract class Shape : MonoBehaviour
{
    public Tile Tile { get; set; }
    
    public abstract Color Color {get; set; }
    public abstract bool MatchColor(Color color);

    public abstract void Select();
    
    public abstract void DeSelect();
    public abstract void Remove();
}
