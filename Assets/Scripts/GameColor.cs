using UnityEngine;

[System.Serializable]
public struct GameColor
{
    public Color color;
    public int scoreRequired;
    public CircleAbility circleAbility;
    [Range(0, 1)] public float abilitySpawnChance;
}