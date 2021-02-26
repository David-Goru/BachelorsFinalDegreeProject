using System.Collections;
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

    [Header("References")]
    [SerializeField] private GameObject model;
    [SerializeField] private List<ItemPool> lootPools;

    // Getters and setters
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int Damage { get => damage; set => damage = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float ForgetRange { get => forgetRange; set => forgetRange = value; }
    public float AttackRate { get => attackRate; set => attackRate = value; }
    public float WalkingSpeed { get => walkingSpeed; set => walkingSpeed = value; }
    public float RunningSpeed { get => runningSpeed; set => runningSpeed = value; }
    public GameObject Model { get => model; set => model = value; }
    public float WanderingArea { get => wanderingArea; set => wanderingArea = value; }
    public float MinTimeBetweenBehaviours { get => minTimeBetweenBehaviours; set => minTimeBetweenBehaviours = value; }
    public float MaxTimeBetweenBehaviours { get => maxTimeBetweenBehaviours; set => maxTimeBetweenBehaviours = value; }
}