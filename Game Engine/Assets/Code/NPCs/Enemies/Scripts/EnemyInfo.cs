using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfo", menuName = "Enemies/EnemyInfo", order = 0)]
public class EnemyInfo : ScriptableObject
{
    [Header("Attributes")]
    [SerializeField] private float visionRange = 0.0f;
    [SerializeField] private float listeningRadius = 0.0f;
    [SerializeField] private int health = 0;
    [SerializeField] private int damage = 0;
    [SerializeField] private float attackRange = 0.0f;
    [Tooltip("Attacks per second")] [SerializeField] private float attackRate = 0.0f;
    [Tooltip("Meters per second")] [SerializeField] private float walkingSpeed = 0.0f;
    [Tooltip("Meters per second")] [SerializeField] private float runningSpeed = 0.0f;
    [SerializeField] private float wanderingArea = 0.0f;
    [SerializeField] private float minTimeBetweenBehaviours = 0.0f;
    [SerializeField] private float maxTimeBetweenBehaviours = 0.0f;

    [Header("References")]
    [SerializeField] private GameObject model;
    [SerializeField] private List<ItemPool> lootPools;

    public float VisionRange { get => visionRange; set => visionRange = value; }
    public float ListeningRadius { get => listeningRadius; set => listeningRadius = value; }
    public int Health { get => health; set => health = value; }
    public int Damage { get => damage; set => damage = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float AttackRate { get => attackRate; set => attackRate = value; }
    public float WalkingSpeed { get => walkingSpeed; set => walkingSpeed = value; }
    public float RunningSpeed { get => runningSpeed; set => runningSpeed = value; }
    public GameObject Model { get => model; set => model = value; }
    public float WanderingArea { get => wanderingArea; set => wanderingArea = value; }
    public float MinTimeBetweenBehaviours { get => minTimeBetweenBehaviours; set => minTimeBetweenBehaviours = value; }
    public float MaxTimeBetweenBehaviours { get => maxTimeBetweenBehaviours; set => maxTimeBetweenBehaviours = value; }
}