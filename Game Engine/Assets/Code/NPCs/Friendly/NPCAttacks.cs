using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttacks : MonoBehaviour, IEntity
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Attacks per minute")] private float attackSpeed = 0.0f;
    [SerializeField] [Tooltip("Damage that every attack deals")] private int damage = 0;
    [SerializeField] [Tooltip("Spell that the character will use")] private Projectile spell;
    [SerializeField] [Tooltip("Starting objective to attack")] private GameObject objective = null;

    [Header("References")]
    [SerializeField] private NPC npc = null;

    [Header("Debug")]
    [SerializeField] private float timer = 0;
    [SerializeField] private bool onFight = false;

    private void Start()
    {
        if (attackSpeed == 0 || spell == null)
        {
            Debug.Log("NPCAttacks attributes not defined for " + name);
            enabled = false;
            return;
        }

        try
        {
            npc = transform.GetComponent<NPC>();
            transform.LookAt(objective.transform);
            npc.SetState(NPCState.IDLE);
            timer = 60 / attackSpeed;
        }
        catch (UnityException e)
        {
            Debug.Log("NPCAttacks references not found. Disabling script. Error: " + e);
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        if (npc.CurrentState != NPCState.IDLE && npc.CurrentState != NPCState.ATTACK) return;

        transform.LookAt(objective.transform);
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 60 / attackSpeed;
            npc.SetState(NPCState.ATTACK);
            StartCoroutine(spawnProjectile());
        }
    }

    public void UpdateObjective(GameObject newObjective)
    {
        objective = newObjective;
        transform.LookAt(objective.transform);
        timer = 60 / attackSpeed;
        onFight = true;
    }

    private IEnumerator spawnProjectile()
    {
        yield return new WaitForSeconds(0.5f);

        spell.Spawn(transform);
        spell.Detonate();

        if (onFight && objective.GetComponent<IEntity>() != null) objective.GetComponent<IEntity>().ReceiveDamage(damage);
    }

    public void ReceiveDamage(int damageAmount, bool finalBattle = false)
    {
        if (onFight) Debug.Log(name + " received " + damageAmount);
    }
}