using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterSpells : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MainCharacter mainCharacter;
    [SerializeField] private Transform characterModel;
    [SerializeField] private List<Spell> spells;
    [SerializeField] private Transform cameraSkillshotPoint;

    [Header("Debug")]
    [SerializeField] private List<Spell> runningSpells;
    [SerializeField] private int currentStep = 0;
    [SerializeField] private float currentStepTime = 0.0f;

    private void Start()
    {
        runningSpells = new List<Spell>();
        currentStep = 0;
        currentStepTime = 0.0f;
    }

    private void Update()
    {
        if (!Input.anyKey || Input.GetButton("Crouch") && runningSpells.Count == 0)
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
            if (runningSpells.Count != 0) startSpell(runningSpells[0]);
            return;
        }

        if (mainCharacter.Movement.enabled) // Whoops
        {
            resetSpells();
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
            else if (Input.GetButtonDown(nextInput.ButtonName) && nextInput.MeetsAllConditions()) nextStep(spell);
            else if (currentInput.MeetsAllConditions()) runningSpells.Add(spell);
        }
    }

    private void checkIfCanStart(Spell spell)
    {
        PlayerInput firstInput = spell.GetInputAtStep(0);
        if (Input.GetButtonDown(firstInput.ButtonName) && firstInput.MeetsAllConditions()) runningSpells.Add(spell);
    }

    private void startSpell(Spell spell)
    {
        PlayerInput firstInput = spell.GetInputAtStep(0);
        mainCharacter.Movement.enabled = false;
        mainCharacter.SetState(MainCharacterState.USINGSPELLS);

        // If skillshot, make character look at player target
        if (spell.IsSkillshot) characterModel.transform.LookAt(new Vector3(cameraSkillshotPoint.position.x, characterModel.transform.position.y, cameraSkillshotPoint.position.z));

        // Start animation
        mainCharacter.Animator.SetTrigger("StartSpells");
        mainCharacter.Animator.SetInteger("SpellAnimID", firstInput.AnimationID);

        // Spawn projectile if needed
        if (firstInput.SpawnProjectile) spell.Projectile.Spawn(characterModel);
    }

    private void resetSpells()
    {
        if (currentStep == 0 && currentStepTime == 0) return;

        // Steps
        currentStep = 0;
        currentStepTime = 0;

        // Animations
        mainCharacter.Animator.SetInteger("SpellAnimID", 0);
        mainCharacter.Animator.SetTrigger("StopSpells");

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

        // Steps
        currentStep = 0;
        currentStepTime = 0;

        // Animations
        mainCharacter.Animator.SetInteger("SpellAnimID", 0);

        // Spells list
        runningSpells.Clear();

        // Update state
        StartCoroutine(unsetRunningSpells());
    }

    private void nextStep(Spell spell)
    {
        PlayerInput currentInput = spell.GetInputAtStep(currentStep);
        currentInput.DoAllActions();

        // Steps
        currentStep++;
        currentStepTime = 0;
        currentInput = spell.GetInputAtStep(currentStep);

        // Projectiles
        if (currentInput.SpawnProjectile) spell.Projectile.Spawn(characterModel);
        if (currentInput.NextProjectileState) spell.Projectile.NextState();
        if (currentInput.DetonateProjectile) spell.Projectile.Detonate();

        // Animations
        mainCharacter.Animator.SetInteger("SpellAnimID", currentInput.AnimationID);

        // Check recently added running spells and see if they can go to the next step
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
        yield return new WaitUntil(() => mainCharacter.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle 1"));
        mainCharacter.Movement.enabled = true;
    }
}