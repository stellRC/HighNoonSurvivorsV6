using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Level Data")]
    public int levelNumber;

    [Header("Objective Data")]
    public int maxBrawlerCount;
    public int maxGunmanCount;
    public int maxProjectileCount;
    public float maxHourCount;
}
