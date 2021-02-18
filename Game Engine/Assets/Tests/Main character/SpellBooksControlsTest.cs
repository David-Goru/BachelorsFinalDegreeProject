using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBooksControlsTest : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpellsBook spellsBook;

    void Update()
    {
        if (spellsBook.Open)
        {
            if (Input.GetButtonDown("Spells book")) spellsBook.CloseSpellsBook();
            else if (Input.GetButtonDown("Next page")) spellsBook.NextPage();
            else if (Input.GetButtonDown("Previous page")) spellsBook.PreviousPage();
        }
        else if (Input.GetButtonDown("Spells book")) spellsBook.OpenSpellsBook();
    }
}