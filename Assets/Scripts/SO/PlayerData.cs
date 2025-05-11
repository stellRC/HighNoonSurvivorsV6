using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Player Prefab")]
    public GameObject playerPrefab;

    [Header("Player Data")]
    public string playerName;
    public float maxHealth = 1;

    [Header("Enemy Movement")]
    public float moveSpeed;
    public float dashSpeed;

    [Header("Player Combat")]
    public float attackRate;

    public float attackRange = 2;
    public int attackDamage = 1;
}
