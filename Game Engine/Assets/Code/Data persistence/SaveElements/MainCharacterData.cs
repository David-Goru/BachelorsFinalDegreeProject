using UnityEngine;

[System.Serializable]
public class MainCharacterData : SaveElement
{
    [SerializeField] private float xPosition;
    [SerializeField] private float yPosition;
    [SerializeField] private float zPosition;
    [SerializeField] private float yRotation;
    [SerializeField] private int gold;
    [SerializeField] private int currentHealth;
    [SerializeField] private int[] spellsUnlocked;
    [SerializeField] private bool spellsBookUnlocked;

    public override SaveElement Save()
    {
        Name = "Main Character";
        Transform mainCharacter = GameObject.FindGameObjectWithTag("Player").transform;
        xPosition = mainCharacter.position.x;
        yPosition = mainCharacter.position.y;
        zPosition = mainCharacter.position.z;
        yRotation = mainCharacter.eulerAngles.y;
        gold = mainCharacter.GetComponent<MainCharacter>().Gold;
        currentHealth = mainCharacter.GetComponent<MainCharacter>().CurrentHealth;
        spellsUnlocked = GameObject.Find("UI scripts").GetComponent<SpellsBook>().UnlockedSpells;
        spellsBookUnlocked = GameObject.Find("UI scripts").GetComponent<SpellsBook>().IsUnlocked;

        return this;
    }

    public override bool Load()
    {
        try
        {
            GameObject mainCharacter = GameObject.FindGameObjectWithTag("Player");
            mainCharacter.transform.position = new Vector3(xPosition, yPosition, zPosition);
            mainCharacter.transform.rotation = Quaternion.Euler(0.0f, yRotation, 0.0f);
            mainCharacter.GetComponent<MainCharacter>().Load(currentHealth, gold);
            SpellsBook spellsBook = GameObject.Find("UI scripts").GetComponent<SpellsBook>();
            foreach (int spell in spellsUnlocked)
            {
                spellsBook.UnlockSpellInfo(spell);
            }
            if (spellsBookUnlocked) GameObject.Find("UI scripts").GetComponent<SpellsBook>().EnableSpellsBook();
        }
        catch (UnityException e)
        {
            // Add to text file?
            Debug.Log("Main character data error: " + e);

            return false;
        }

        return true;
    }
}