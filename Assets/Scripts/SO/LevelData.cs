using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Level Data")]
    public int LevelNumber = 1;
    public float GameDaySeconds = .5f;

    [Header("Objective Data")]
    public int MaxBrawlerCount = 5;
    public int MaxGunmanCount = 10;
    public int MaxRollerCount = 15;
    public float MaxHourCount = 12;

    public float SpecialAttackRate = 2;
}
