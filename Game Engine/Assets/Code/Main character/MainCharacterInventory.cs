using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCharacterInventory : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private GameObject elementUI = null;

    [Header("References")]
    [SerializeField] private Transform inventoryUI = null;

    [Header("Debug")]
    [SerializeField] private List<MainCharacterItem> mainCharacterItems = null;
    [SerializeField] private bool open = false;

    public List<MainCharacterItem> MainCharacterItems { get => mainCharacterItems; set => mainCharacterItems = value; }

    private void Start()
    {
        if (elementUI == null) Debug.Log("MainCharacterInventory element UI not defined.");

        try
        {
            inventoryUI = GameObject.Find("UI").transform.Find("Inventory").Find("Scroll View").Find("Viewport").Find("Inventory content");
        }
        catch (UnityException e) { Debug.Log("MainCharacterInventory references not found. Error: " + e); }

        if (!Menu.LoadingGame) mainCharacterItems = new List<MainCharacterItem>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory")) ChangeInventoryState();
        else if (Input.GetButtonDown("Close UI")) CloseInventory();
    }

    public void SetUpDataLoaded()
    {
        foreach (MainCharacterItem characterItem in mainCharacterItems)
        {
            AddElementToUI(characterItem.Item);
            characterItem.UpdateUI(inventoryUI);
        }
    }

    public void ChangeInventoryState()
    {
        if (open) CloseInventory();
        else OpenInventory();
    }

    public void OpenInventory()
    {
        if (open) return;
        transform.GetComponent<MainCharacter>().CharacterCamera.ChangeState(false);
        Time.timeScale = 0;
        open = true;
        inventoryUI.parent.parent.parent.gameObject.SetActive(true);
        UI.Instance.UnlockMouse();
    }

    public void CloseInventory()
    {
        if (!open) return;
        transform.GetComponent<MainCharacter>().CharacterCamera.ChangeState(true);
        Time.timeScale = 1;
        open = false;
        inventoryUI.parent.parent.parent.gameObject.SetActive(false);
        UI.Instance.LockMouse();
    }

    public void AddItem(Item item, int amount)
    {
        MainCharacterItem characterItem = mainCharacterItems.Find(x => x.Item == item);
        if (characterItem != null) characterItem.AddAmount(amount);
        else
        {
            characterItem = new MainCharacterItem(item, amount);
            mainCharacterItems.Add(characterItem);
            AddElementToUI(item);
        }

        characterItem.UpdateUI(inventoryUI);
    }

    public bool RemoveItem(Item item, int amount)
    {
        MainCharacterItem characterItem = mainCharacterItems.Find(x => x.Item == item);
        if (characterItem != null)
        {
            bool removed = characterItem.RemoveAmount(amount);
            if (removed && characterItem.Amount == 0)
            {
                characterItem.RemoveFromUI(inventoryUI);
                mainCharacterItems.Remove(characterItem);
            }
            else characterItem.UpdateUI(inventoryUI);

            return removed;
        }
        return false;
    }

    public void AddElementToUI(Item item)
    {
        GameObject element = Instantiate(elementUI);
        element.transform.SetParent(inventoryUI, false);
        element.transform.Find("Icon").GetComponent<Image>().sprite = item.ItemIcon;
        if (item is HealItem)
        {
            element.GetComponent<Button>().onClick.AddListener(() => GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacter>().Heal(((HealItem)item).HealAmount));
            element.GetComponent<Button>().onClick.AddListener(() => RemoveItem(item, 1));
        }
        element.name = item.name;
    }
}

[System.Serializable]
public class MainCharacterItem
{
    [System.NonSerialized] private Item item;
    private string itemName;
    private int amount;

    public Item Item { get => item; set => item = value; }
    public int Amount { get => amount; set => amount = value; }

    public void SaveName()
    {
        itemName = item.name;
    }

    public void LoadItem()
    {
        item = LoadGame.Instance.Items.Find(x => x.name == itemName);
    }

    public MainCharacterItem(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int amount)
    {
        this.amount += amount;
    }

    public bool RemoveAmount(int amount)
    {
        if (amount > this.amount) return false;

        this.amount -= amount;
        return true;
    }

    public void UpdateUI(Transform ui)
    {
        ui.Find(item.name).Find("Amount").GetComponent<Text>().text = amount.ToString();
    }

    public void RemoveFromUI(Transform ui)
    {
        Object.Destroy(ui.Find(item.name).gameObject);
    }
}