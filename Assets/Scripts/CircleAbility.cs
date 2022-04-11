using UnityEngine;

public abstract class CircleAbility : ScriptableObject
{
    public abstract void Do(Circle owner, GameManager gameManager);
}
