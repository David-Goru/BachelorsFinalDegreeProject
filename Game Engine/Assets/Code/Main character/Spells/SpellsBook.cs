using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsBook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bookUI;

    [Header("Debug")]
    [SerializeField] private Dictionary<int, GameObject> pages;
    [SerializeField] private int currentPage = 0;
    [SerializeField] private bool open = false;
    [SerializeField] private List<int> unlockedSpells;

    public bool Open { get => open; set => open = value; }

    private void Start()
    {
        pages = new Dictionary<int, GameObject>();
        foreach (Transform page in bookUI.transform.Find("Pages"))
        {
            pages.Add(int.Parse(page.name), page.gameObject);
        }
        unlockedSpells = new List<int>();
    }

    public void ChangeSpellsBookState()
    {
        if (open) CloseSpellsBook();
        else OpenSpellsBook();
    }

    public void OpenSpellsBook()
    {
        setPage(0);
        open = true;
        bookUI.SetActive(true);
    }

    public void CloseSpellsBook()
    {
        bookUI.SetActive(false);
        open = false;
    }

    public void NextPage()
    {
        if (currentPage < (pages.Count - 1)) setPage(currentPage + 1);
    }

    public void PreviousPage()
    {
        if (currentPage > 0) setPage(currentPage - 1);
    }

    private void setPage(int newPage)
    {
        pages[currentPage].SetActive(false);
        pages[newPage].SetActive(true);

        currentPage = newPage;
    }

    public void UnlockSpellInfo(int spellNumber)
    {
        if (unlockedSpells.Contains(spellNumber)) return;
        unlockedSpells.Add(spellNumber);
        pages[spellNumber].transform.Find("Info").gameObject.SetActive(true);
    }
}