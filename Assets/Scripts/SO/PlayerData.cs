using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Player Prefab")]
    public GameObject PlayerPrefab;

    [Header("Player Data")]
    public string PlayerName;
    public float MaxHealth = 1;

    [Header("Enemy Movement")]
    public float MoveSpeed;
    public float DashSpeed;

    [Header("Player Combat")]
    public float AttackRange = 2;
    public float SpecialAttackRange = 7;
    public int AttackDamage = 1;
}
