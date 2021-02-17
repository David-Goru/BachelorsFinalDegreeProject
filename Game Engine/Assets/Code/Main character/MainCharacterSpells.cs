using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterSpells : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Spell> spells;
    [SerializeField] private Animator animator;

    [Header("Debug")]
    [SerializeField] private List<Spell> runningSpells;
    [SerializeField] private int currentStep = 0;
    [SerializeField] private float currentStepTime = 0.0f;
    [SerializeField] private bool isRunningSpells = false;

    [HideInInspector] public bool IsRunningSpells { get => isRunningSpells; set => isRunningSpells = value; }

    private void Start()
    {
        runningSpells = new List<Spell>();
        currentStep = 0;
        currentStepTime = 0.0f;
    }

    private void Update()
    {
        if (!Input.anyKey)
        {
            resetSpells();
            return;
        }

        if (runningSpells.Count == 0)
        {
            resetSpells();
            foreach (Spell spell in spells)
            {
                checkIfCanStart(spell);
            }
            return;
        }

        // Step time
        currentStepTime += Time.deltaTime;

        // Spells lists
        List<Spell> oldRunningSpells = runningSpells;
        runningSpells = new List<Spell>();

        foreach (Spell spell in oldRunningSpells)
        {
            PlayerInput currentInput = spell.GetInputAtStep(currentStep);
            if (currentInput.RequiresTime)
            {
                if (currentInput.TimeAmount > currentStepTime)
                {
                    if (Input.GetButton(currentInput.ButtonName) && currentInput.MeetsAllConditions())
                    {
                        runningSpells.Add(spell);
                    }
                    continue;
                }
            }

            PlayerInput nextInput = spell.GetInputAtStep(currentStep + 1);
            if (nextInput == null)
            {
                finishSpell(spell);
                return;
            }
            else if (Input.GetButtonDown(nextInput.ButtonName) && nextInput.MeetsAllConditions())
            {
                nextStep(spell);
            }
            else if (currentInput.MeetsAllConditions())
            {
                runningSpells.Add(spell);
            }
        }
    }

    private void checkIfCanStart(Spell spell)
    {
        PlayerInput firstInput = spell.GetInputAtStep(0);
        if (Input.GetButtonDown(firstInput.ButtonName))
        {
            if (firstInput.MeetsAllConditions())
            {
                runningSpells.Add(spell);
                isRunningSpells = true;

                // Start animation
                animator.SetTrigger("StartSpells");
                animator.SetInteger("SpellAnimID", firstInput.AnimationID);
                animator.SetFloat("IdleTime", 0);
            }
        }
    }

    private void resetSpells()
    {
        if (currentStep == 0 && currentStepTime == 0) return;

        // Steps
        currentStep = 0;
        currentStepTime = 0;

        // Animations
        animator.SetInteger("SpellAnimID", 0);
        animator.SetTrigger("StopSpells");

        // Remove any possible projectile spawned
        foreach (Spell s in spells)
        {
            s.Projectile.Stop();
        }

        // Spells list
        runningSpells.Clear();

        // Update state
        StartCoroutine(unsetRunningSpells());
    }

    private void finishSpell(Spell spell)
    {
        PlayerInput currentInput = spell.GetInputAtStep(currentStep);
        currentInput.DoAllActions();

        if (currentInput.SpawnProjectile) spell.Projectile.Spawn(transform);
        if (currentInput.DetonateProjectile) spell.Projectile.Detonate();

        // Steps
        currentStep = 0;
        currentStepTime = 0;

        // Animations
        animator.SetInteger("SpellAnimID", 0);

        // Spells list
        runningSpells.Clear();

        // Update state
        StartCoroutine(unsetRunningSpells());
    }

    private void nextStep(Spell spell)
    {
        PlayerInput currentInput = spell.GetInputAtStep(currentStep);
        currentInput.DoAllActions();

        if (currentInput.SpawnProjectile) spell.Projectile.Spawn(transform);
        if (currentInput.DetonateProjectile) spell.Projectile.Detonate();

        // Steps
        currentStep++;
        currentStepTime = 0;
        currentInput = spell.GetInputAtStep(currentStep);

        // Animations
        animator.SetInteger("SpellAnimID", currentInput.AnimationID);

        // Check running spells and see if they can go to the next step
        foreach (Spell s in runningSpells.ToArray())
        {
            PlayerInput nextInput = s.GetInputAtStep(currentStep);
            if (!Input.GetButtonDown(nextInput.ButtonName) || !nextInput.MeetsAllConditions())
            {
                runningSpells.Remove(s);
            }
        }

        // Spells list
        runningSpells.Add(spell);
    }

    private IEnumerator unsetRunningSpells()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Quick water drop 1") == false);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Quick water drop 2") == false);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Floral flame 1") == false);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Floral flame 2") == false);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Floral flame 3") == false);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Floral flame 4") == false);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Floral flame 5") == false);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Lethal orb 1") == false);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Lethal orb 2") == false);

        isRunningSpells = false;
    }
}