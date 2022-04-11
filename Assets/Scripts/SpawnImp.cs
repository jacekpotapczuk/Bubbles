using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Spawn Imp")]
public class SpawnImp : CircleAbility
{
    public override void Do(Circle owner, GameManager gameManager)
    {
        Debug.Log("Do Spawn imp");
        gameManager.SpawnImp(owner.Tile);
    }
}