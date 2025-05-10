using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Objects")]
    public GameObject enemyPrefab;
    public GameObject projectilePrefab;

    [Header("Enemy Data")]
    public string EnemyName;
    public float maxHealth = 1;

    [Header("Enemy Movement")]
    public float moveSpeed;
    public float distanceBetween;
    public float attackDistance;
    public float minimumDistance;
    public int movementPatternID;
    public int movementSpeedID;

    [Header("Enemy Combat")]
    public bool isBoss;
}
