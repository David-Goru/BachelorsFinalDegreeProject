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
    [Tooltip("Attacks per second")]
    [SerializeField] private float attackRate = 0.0f;
    [SerializeField] private float movementSpeed = 0.0f;

    [Header("References")]
    [SerializeField] private GameObject model;
    [SerializeField] private List<ItemPool> lootPools;

    public GameObject Model { get => model; set => model = value; }
}