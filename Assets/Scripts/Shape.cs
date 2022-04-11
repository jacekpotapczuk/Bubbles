using UnityEngine;

public abstract class Shape : MonoBehaviour
{
    public Tile Tile { get; set; }
    
    public abstract Color? Color {get; set; }
    public abstract bool MatchColor(Color? color);
    
    public abstract void Remove(); // just remove

    public abstract void Clear();

    public virtual void OnNewTurn(GameManager gameManager)
    {
    }

    public virtual void Select()
    {
    }

    public virtual void DeSelect()
    {
    }

    public virtual void Die(GameManager gameManager) // remove and optionally add bonus effects
    {
        Remove();
    }
}
