using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SetReaperState")]
public class SetReaperState : CircleAbility
{
    [SerializeField, Range(1, 5)] private int targetsToKill;
    public override void Do(Circle owner, GameManager gameManager)
    {
        gameManager.RequestReaperState(targetsToKill);
    }
}