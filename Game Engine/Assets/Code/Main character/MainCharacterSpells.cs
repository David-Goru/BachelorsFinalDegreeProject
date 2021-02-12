using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterSpells : MonoBehaviour
{
    [SerializeField] private List<Spell> spells;
    [SerializeField] private PlayerInputParse inputParse;
    [SerializeField] private Animator animator;

    [Header("Debug")]
    [SerializeField] private List<Spell> possibleSpells;
    [SerializeField] private int currentStep = 0;
    [SerializeField] private float currentStepTime = 0.0f;

    private void Start()
    {
        possibleSpells = new List<Spell>();
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

        if (possibleSpells.Count == 0)
        {
            foreach (Spell spell in spells)
            {
                PlayerInput firstInput = spell.GetInputAtStep(0);
                if (Input.GetButtonDown(inputParse.ParseInput(firstInput.ButtonName)))
                {                    
                    if (firstInput.MeetsAllConditions())
                    {
                        possibleSpells.Add(spell);                        
                        startSpell(firstInput.AnimationID);
                    }
                }
            }

            if (possibleSpells.Count == 0) resetSpells();
        }
        else
        {
            List<Spell> newPossibleSpells = new List<Spell>();
            currentStepTime += Time.deltaTime;
            foreach (Spell spell in possibleSpells)
            {
                PlayerInput currentInput = spell.GetInputAtStep(currentStep);
                if (currentInput.RequiresTime)
                {
                    if (currentInput.TimeAmount <= currentStepTime)
                    {
                        if ((currentStep + 1) == spell.StepsAmount)
                        {
                            currentInput.DoAllActions();
                            finishSpell();
                            return;
                        }
                        else
                        {
                            PlayerInput nextInput = spell.GetInputAtStep(currentStep + 1);
                            if (Input.GetButtonDown(inputParse.ParseInput(nextInput.ButtonName)) && nextInput.MeetsAllConditions())
                            {
                                currentInput.DoAllActions();
                                newPossibleSpells.Add(spell);
                                nextStep(nextInput.AnimationID);
                                return;
                            }
                            else if (Input.GetButton(inputParse.ParseInput(currentInput.ButtonName)))
                            {
                                newPossibleSpells.Add(spell);
                            }
                        }
                    }
                    else if (currentInput.TimeAmount > currentStepTime && Input.GetButton(inputParse.ParseInput(currentInput.ButtonName)))
                    {
                        newPossibleSpells.Add(spell);
                    }
                }
                else
                {
                    if ((currentStep + 1) == spell.StepsAmount)
                    {
                        currentInput.DoAllActions();
                        finishSpell();
                        return;
                    }
                    else
                    {
                        PlayerInput nextInput = spell.GetInputAtStep(currentStep + 1);
                        if (Input.GetButtonDown(inputParse.ParseInput(nextInput.ButtonName)) && nextInput.MeetsAllConditions())
                        {
                            currentInput.DoAllActions();
                            newPossibleSpells.Add(spell);
                            nextStep(nextInput.AnimationID);
                            return;
                        }
                        else if (Input.GetButton(inputParse.ParseInput(currentInput.ButtonName)))
                        {
                            newPossibleSpells.Add(spell);
                        }
                    }
                }
            }
            possibleSpells = newPossibleSpells;
        }
    }

    private void resetSpells()
    {
        if (currentStep == 0 && currentStepTime == 0) return;
        currentStep = 0;
        currentStepTime = 0;
        possibleSpells.Clear();
        animator.SetInteger("SpellAnimID", 0);
        animator.SetTrigger("StopSpells");
    }

    private void startSpell(int animationID)
    {
        animator.SetTrigger("StartSpells");
        animator.SetInteger("SpellAnimID", animationID);
        animator.SetFloat("IdleTime", 0);
    }

    private void finishSpell()
    {
        currentStep = 0;
        currentStepTime = 0;
        possibleSpells.Clear();
        animator.SetInteger("SpellAnimID", 0);
    }

    private void nextStep(int animationID)
    {
        currentStep++;
        currentStepTime = 0;
        animator.SetInteger("SpellAnimID", animationID);
    }
}