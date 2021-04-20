using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfo", menuName = "Enemies/EnemyInfo", order = 0)]
public class EnemyInfo : ScriptableObject
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Health when spawned")] private int maxHealth = 0;
    [SerializeField] [Tooltip("Damage that deals per attack")] private int damage = 0;
    [SerializeField] [Tooltip("Distance where the enemy can start attacking")] private float attackRange = 0.0f;
    [SerializeField] [Tooltip("Distance where the enemy forgets who was chasing")] private float forgetRange = 0.0f;
    [SerializeField] [Tooltip("Attacks per second")] private float attackRate = 0.0f;
    [SerializeField] [Tooltip("Meters per second")] private float walkingSpeed = 0.0f;
    [SerializeField] [Tooltip("Meters per second")] private float runningSpeed = 0.0f;
    [SerializeField] [Tooltip("Maximum X and Y difference per wander action")] private float wanderingArea = 0.0f;
    [SerializeField] [Tooltip("Minimum time the enemy needs for starting a new behaviour")] private float minTimeBetweenBehaviours = 0.0f;
    [SerializeField] [Tooltip("Maximum time the enemy needs for starting a new behaviour")] private float maxTimeBetweenBehaviours = 0.0f;
    [SerializeField] [Tooltip("The enemy projectile - if any")] private GameObject projectile = null;

    [Header("References")]
    [SerializeField] private GameObject model;
    [SerializeField] private List<ItemPool> lootPools;

    [Header("Debug")]
    [SerializeField] private int amountKilled = 0;

    // Getters
    public int MaxHealth { get => maxHealth; }
    public int Damage { get => damage; }
    public float AttackRange { get => attackRange; }
    public float ForgetRange { get => forgetRange; }
    public float AttackRate { get => attackRate; }
    public float WalkingSpeed { get => walkingSpeed; }
    public float RunningSpeed { get => runningSpeed; }
    public GameObject Model { get => model; }
    public float WanderingArea { get => wanderingArea; }
    public float MinTimeBetweenBehaviours { get => minTimeBetweenBehaviours; }
    public float MaxTimeBetweenBehaviours { get => maxTimeBetweenBehaviours; }
    public GameObject Projectile { get => projectile; }
    public List<ItemPool> LootPools { get => lootPools; }

    // Getters and setters
    public int AmountKilled { get => amountKilled; set => amountKilled = value; }
}