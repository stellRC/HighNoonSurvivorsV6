using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Objects")]
    public GameObject EnemyPrefab;
    public GameObject ProjectilePrefab;

    [Header("Enemy Data")]
    public string EnemyName;
    public float MaxHealth = 1;

    [Header("Enemy Movement")]
    public float MoveSpeed = 3;
    public float AttackDistance = 5;

    [Tooltip("Stopping Min distance between enemy & player")]
    public float MinimumDistance;

    [Tooltip("AI Pattern")]
    public int MovementPatternID;

    [Tooltip("Movement Pace e.g. walking, running, sprinting")]
    public int MovementSpeedID;

    [Header("Enemy Combat")]
    public bool IsBoss;
}
