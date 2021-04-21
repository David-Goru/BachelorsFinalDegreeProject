using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsBook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bookUI;
    [SerializeField] private GameObject openButton;

    [Header("Debug")]
    [SerializeField] private Dictionary<int, GameObject> pages;
    [SerializeField] private int currentPage = 0;
    [SerializeField] private bool open = false;
    [SerializeField] private List<int> unlockedSpells;

    public bool Open { get => open; set => open = value; }

    private void Awake()
    {
        pages = new Dictionary<int, GameObject>();
        foreach (Transform page in bookUI.transform.Find("Pages"))
        {
            pages.Add(int.Parse(page.name), page.gameObject);
        }
        unlockedSpells = new List<int>();
        enabled = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Spells book")) OpenSpellsBook();
    }

    public void EnableSpellsBook()
    {
        enabled = true;
        openButton.SetActive(true);
    }

    public void ChangeSpellsBookState()
    {
        if (open) CloseSpellsBook();
        else OpenSpellsBook();
    }

    public void OpenSpellsBook()
    {
        Time.timeScale = 0;
        setPage(0);
        open = true;
        bookUI.SetActive(true);
    }

    public void CloseSpellsBook()
    {
        Time.timeScale = 1;
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