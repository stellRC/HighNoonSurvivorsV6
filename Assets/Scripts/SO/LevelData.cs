using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Level Data")]
    public int levelNumber = 1;

    [Header("Objective Data")]
    public int maxBrawlerCount = 5;
    public int maxGunmanCount = 10;
    public int maxProjectileCount = 15;
    public float maxHourCount = 12;

    public float specialAttackRate = 2;
}
