using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Spawn Rainbow")]
public class SpawnRainbow : CircleAbility
{
    public override void Do(Circle owner, GameManager gameManager)
    {
        Debug.Log("Do Spawn rainbow");
        gameManager.SpawnRainbow(owner.Tile);
        gameManager.CheckForPoints(owner);
    }
}