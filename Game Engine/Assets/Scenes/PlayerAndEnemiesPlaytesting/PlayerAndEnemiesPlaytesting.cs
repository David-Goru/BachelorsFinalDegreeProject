using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAndEnemiesPlaytesting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpellsBook spellsBook = null;
    [SerializeField] private Text healthUI = null;
    [SerializeField] private Text enemyUI = null;
    [SerializeField] private Text inventoryUI = null;

    [Header("Debug")]
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private int[] stats;

    public static PlayerAndEnemiesPlaytesting Instance;

    private void Start()
    {
        Instance = this;

        // Unlock all spells info
        spellsBook.UnlockSpellInfo(1);
        spellsBook.UnlockSpellInfo(2);
        spellsBook.UnlockSpellInfo(3);

        // Set base stats
        stats = new int[8];
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth <= 0) healthUI.text = "You should be already dead. But, don't worry!";
        else healthUI.text = string.Format("Current health: {0}", currentHealth);
    }

    public void UpdateStat(string statName, int amount)
    {
        switch (statName)
        {
            case "Wild pig":
                stats[0] += amount;
                break;
            case "Butterfly":
                stats[1] += amount;
                break;
            case "Slime":
                stats[2] += amount;
                break;
            case "Dark essence":
                stats[3] += amount;
                break;
            case "Horn":
                stats[4] += amount;
                break;
            case "Tusk":
                stats[5] += amount;
                break;
            case "Butterfly wing":
                stats[6] += amount;
                break;
            case "Slime liquid":
                stats[7] += amount;
                break;
        }

        enemyUI.text = string.Format("Enemies killed\n{0} wild pigs, {1} butterflies, {2} slimes", stats[0], stats[1], stats[2]);
        inventoryUI.text = string.Format("Inventory\n\n{0} dark essences\n\n{1} horns\n\n{2} tusks\n\n{3} butterfly wings\n\n{4} slime liquids", stats[3], stats[4], stats[5], stats[6], stats[7]);
    }

    public void CloseTheGame()
    {
        Application.Quit();
    }
}