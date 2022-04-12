using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Problem")]
public class Problem : CircleAbility
{
    [SerializeField, Range(1, 5), Tooltip("In turns.")] private int blockDuration = 1;
    public override void Do(Circle owner, GameManager gameManager)
    {
        foreach (var shape in gameManager.Spawner.Shapes)
        {
            if (shape.gameObject.activeSelf && shape is Circle c)
                c.BlockFor(blockDuration);
        }
    }
}