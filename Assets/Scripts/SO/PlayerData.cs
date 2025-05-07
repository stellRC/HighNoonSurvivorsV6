using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Player Prefab")]
    public GameObject playerPrefab;

    [Header("Player Data")]
    public string playerName;
    public float maxHealth;

    [Header("Enemy Movement")]
    public float moveSpeed;
    public int movementPatternID;
    public int movementSpeedID;

    [Header("Player Combat")]
    public float attackRate;
    public float specialAttackTime;
    public float attackRange;
    public int attackDamage;
}
