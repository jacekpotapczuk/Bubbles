using UnityEngine;

public class CircleIndicator : MonoBehaviour
{
   [SerializeField] private SpriteRenderer spriteRenderer;
   private Color color;

   public Color Color
   {
      get => color;
      set
      {
         color = value;
         spriteRenderer.color = value;
      }
   }
   
   public Tile Tile { get; set; }
}
