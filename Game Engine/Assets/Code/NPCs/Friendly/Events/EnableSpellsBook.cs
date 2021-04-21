using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSpellsBook : IEvent
{
    [Header("References")]
    [SerializeField] private SpellsBook spellsBook;

    public override void Run()
    {
        spellsBook.EnableSpellsBook();
    }
}