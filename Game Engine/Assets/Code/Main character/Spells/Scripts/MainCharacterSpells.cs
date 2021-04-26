using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MainCharacterSpells : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private List<Spell> spells;

    [Header("References")]
    [SerializeField] private MainCharacter mainCharacter = null;
    [SerializeField] private Transform cameraSkillshotPoint = null;
    [SerializeField] private SpellsBook spellsBook = null;

    [Header("Debug")]
    [SerializeField] private List<Spell> runningSpells = null;
    [SerializeField] private int currentStep = 0;
    [SerializeField] private float currentStepTime = 0.0f;

    private void Start()
    {
        if (spells.Count == 0)
        {
            Debug.Log("Empty spells list. Disabling MainCharacterSpells.");
            enabled = false;
        }

        // Get components
        try
        {
            mainCharacter = gameObject.GetComponent<MainCharacter>();
            cameraSkillshotPoint = GameObject.FindGameObjectWithTag("SkillshotTarget").transform;
        }
        catch (UnityException e)
        {
            Debug.Log("MainCharacterSpells references not found. Disabling script. Error: " + e);
            enabled = false;
        }

        // Set base stats
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
            else if (Input.GetButtonDown(nextInput.ButtonName) && nextInput.MeetsAllConditions())
            {
                nextStep(spell);
                return;
            }
            else if (currentInput.MeetsAllConditions())
            {
                if (!currentInput.RequiresTime || (currentInput.RequiresTime && Input.GetButton(currentInput.ButtonName))) runningSpells.Add(spell);
            }
        }
    }

    public void BlockSpells()
    {
        resetSpells();
        enabled = false;
    }

    public void UnblockSpells()
    {
        resetSpells();
        enabled = true;
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
        if (spell.IsSkillshot) mainCharacter.Model.transform.LookAt(new Vector3(cameraSkillshotPoint.position.x, mainCharacter.Model.transform.position.y, cameraSkillshotPoint.position.z));

        // Start animation
        mainCharacter.Animations.SpellAnimation(firstInput.AnimationID, true);

        // Spawn projectile if needed
        if (firstInput.SpawnProjectile) spell.Projectile.Spawn(mainCharacter.Model);
    }

    private void resetSpells()
    {
        if (currentStep == 0 && currentStepTime == 0) return;

        // Steps
        currentStep = 0;
        currentStepTime = 0;

        // Stop animation
        mainCharacter.Animations.SpellAnimation(0);

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

        // Unlock on spells book
        spellsBook.UnlockSpellInfo(spells.IndexOf(spell) + 1);

        // Steps
        currentStep = 0;
        currentStepTime = 0;
        
        // Stop animation
        mainCharacter.Animations.SpellAnimation(0);

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
        if (currentInput.SpawnProjectile) spell.Projectile.Spawn(mainCharacter.Model);
        if (currentInput.NextProjectileState) spell.Projectile.NextState();
        if (currentInput.DetonateProjectile) spell.Projectile.Detonate();

        // Animations
        mainCharacter.Animations.SpellAnimation(currentInput.AnimationID);

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
        mainCharacter.SetState(MainCharacterState.IDLE);
        yield return new WaitUntil(() => mainCharacter.Animations.SpellsFinished());
        
        yield return new WaitForSeconds(0.05f);
        mainCharacter.Movement.enabled = true;
    }
}