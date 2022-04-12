using UnityEngine;

public class CircleIndicator : MonoBehaviour
{
   [SerializeField] private SpriteRenderer spriteRenderer;
   
   public Tile Tile { get; set; }
   
   public Color Color
   {
      get => color;
      set
      {
         color = value;
         spriteRenderer.color = value;
      }
   }
   private Color color;
}
