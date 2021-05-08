using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMerchant : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Items to be sold/bought")] private List<MerchantItem> items = null;
    [SerializeField] private GameObject merchantItemButton = null;
    [SerializeField] private GameObject playerItemButton = null;

    [Header("References")]
    [SerializeField] private GameObject player = null;
    [SerializeField] private Transform merchantUI = null;
    [SerializeField] private Transform merchantInventoryUI = null;
    [SerializeField] private Transform playerInventoryUI = null;

    [Header("Debug")]
    [SerializeField] private int sellGold = 0;
    [SerializeField] private int sellAllGold = 0;
    [SerializeField] private int buyGold = 0;

    private void Start()
    {
        try
        {
            player = GameObject.FindGameObjectWithTag("Player");
            merchantUI = GameObject.Find("UI").transform.Find("Merchant");
            merchantInventoryUI = merchantUI.Find("Merchant inventory").Find("Scroll View").Find("Viewport").Find("Content");
            playerInventoryUI = merchantUI.Find("Player inventory").Find("Scroll View").Find("Viewport").Find("Content");

            // Set up buttons
            merchantUI.Find("Buy").GetComponent<Button>().onClick.AddListener(() => buy());
            merchantUI.Find("Sell selected").GetComponent<Button>().onClick.AddListener(() => sellSelected());
            merchantUI.Find("Sell all").GetComponent<Button>().onClick.AddListener(() => sellAll());
            merchantUI.Find("Close").GetComponent<Button>().onClick.AddListener(() => Close());

            // Set up items
            foreach (MerchantItem item in items)
            {
                if (item.Buying)
                {
                    GameObject merchantItem = Instantiate(merchantItemButton);
                    merchantItem.name = item.Item.name;
                    merchantItem.transform.SetParent(merchantInventoryUI);
                    merchantItem.transform.Find("Image").GetComponent<Image>().sprite = item.Item.ItemIcon;
                    merchantItem.transform.Find("Remove").GetComponent<Button>().onClick.AddListener(() => removeAmountMerchantItem(merchantItem));
                    merchantItem.transform.Find("Add").GetComponent<Button>().onClick.AddListener(() => addAmountMerchantItem(merchantItem));
                    merchantItem.transform.Find("Price").GetComponent<Text>().text = string.Format("{0}g", item.Price);
                    merchantItem.transform.Find("Amount").Find("Text").GetComponent<Text>().text = "0";
                }
                else
                {
                    GameObject playerItem = Instantiate(playerItemButton);
                    playerItem.name = item.Item.name;
                    playerItem.transform.SetParent(playerInventoryUI);
                    playerItem.transform.Find("Image").GetComponent<Image>().sprite = item.Item.ItemIcon;
                    playerItem.transform.Find("Remove").GetComponent<Button>().onClick.AddListener(() => removeAmountPlayerItem(playerItem));
                    playerItem.transform.Find("Add").GetComponent<Button>().onClick.AddListener(() => addAmountPlayerItem(playerItem));
                    playerItem.transform.Find("Price").GetComponent<Text>().text = string.Format("{0}g", item.Price);
                    playerItem.transform.Find("Amount selected").Find("Text").GetComponent<Text>().text = "0";
                    playerItem.transform.Find("Current amount").GetComponent<Text>().text = "0";
                }
            }
        }
        catch(UnityException e) { Debug.Log("NPCMerchant references not found. Disabling script. Error: " + e); }
    }

    public void Open()
    {
        player.transform.GetComponent<MainCharacter>().CharacterCamera.ChangeState(false);
        List<MainCharacterItem> playerItems = player.GetComponent<MainCharacterInventory>().MainCharacterItems;
        sellGold = 0; 
        sellAllGold = 0;
        buyGold = 0;
        foreach (MerchantItem item in items)
        {
            item.Amount = 0;
            if (item.Buying)
            {
                merchantInventoryUI.Find(item.Item.name).Find("Amount").Find("Text").GetComponent<Text>().text = "0";
                continue;
            }

            playerInventoryUI.Find(item.Item.name).Find("Amount selected").Find("Text").GetComponent<Text>().text = "0";

            MainCharacterItem playerItem = playerItems.Find(x => x.Item == item.Item);
            if (playerItem != null)
            {
                playerInventoryUI.Find(item.Item.name).Find("Current amount").GetComponent<Text>().text = playerItem.Amount.ToString();
                playerInventoryUI.Find(item.Item.name).gameObject.SetActive(true);
                sellAllGold += playerItem.Amount * item.Price;
                item.Amount = 0;
                item.MaxAmount = playerItem.Amount;
            }
            else playerInventoryUI.Find(item.Item.name).gameObject.SetActive(false);
        }
        merchantUI.gameObject.SetActive(true);

        merchantUI.Find("Sell all").Find("Text").GetComponent<Text>().text = sellAllGold == 0 ? "SELL ALL" : "SELL ALL (" + sellAllGold + "g)";
        merchantUI.Find("Money").Find("Text").GetComponent<Text>().text = player.GetComponent<MainCharacter>().Gold + "g";
        UI.Instance.UnlockMouse();
    }

    public void Close()
    {
        player.transform.GetComponent<MainCharacter>().CharacterCamera.ChangeState(true);
        merchantUI.gameObject.SetActive(false); 
        UI.Instance.LockMouse();
    }

    private void buy()
    {
        if (buyGold == 0 || player.GetComponent<MainCharacter>().Gold < buyGold) return;

        foreach (MerchantItem item in items)
        {
            if (item.Buying && item.Amount > 0)
            {
                player.GetComponent<MainCharacterInventory>().AddItem(item.Item, item.Amount);
                item.Amount = 0;
                merchantInventoryUI.Find(item.Item.name).transform.Find("Amount").Find("Text").GetComponent<Text>().text = item.Amount.ToString();
            }
        }

        player.GetComponent<MainCharacter>().Gold -= buyGold;
        merchantUI.Find("Money").Find("Text").GetComponent<Text>().text = player.GetComponent<MainCharacter>().Gold + "g";

        buyGold = 0;
        merchantUI.Find("Buy").Find("Text").GetComponent<Text>().text = "BUY";
    }

    private void sellSelected()
    {
        if (sellGold == 0) return;

        List<MainCharacterItem> playerItems = player.GetComponent<MainCharacterInventory>().MainCharacterItems;
        foreach (MerchantItem item in items)
        {
            if (!item.Buying && item.Amount > 0)
            {
                if (!playerItems.Exists(x => x.Item == item.Item)) continue;

                player.GetComponent<MainCharacterInventory>().RemoveItem(item.Item, item.Amount);
                item.MaxAmount -= item.Amount;
                item.Amount = 0;

                if (item.MaxAmount == 0) playerInventoryUI.Find(item.Item.name).gameObject.SetActive(false);
                else
                {
                    playerInventoryUI.Find(item.Item.name).transform.Find("Amount selected").Find("Text").GetComponent<Text>().text = "0";
                    playerInventoryUI.Find(item.Item.name).transform.Find("Current amount").GetComponent<Text>().text = item.MaxAmount.ToString();
                }
            }
        }

        player.GetComponent<MainCharacter>().Gold += sellGold;
        merchantUI.Find("Money").Find("Text").GetComponent<Text>().text = player.GetComponent<MainCharacter>().Gold + "g";

        sellGold = 0;
        merchantUI.Find("Sell selected").Find("Text").GetComponent<Text>().text = "SELL SELECTED";

        sellAllGold = 0;
        foreach (MerchantItem item in items)
        {
            if (item.Buying) continue;

            MainCharacterItem playerItem = playerItems.Find(x => x.Item == item.Item);
            if (playerItem != null) sellAllGold += playerItem.Amount * item.Price;
        }
        merchantUI.Find("Sell all").Find("Text").GetComponent<Text>().text = sellAllGold == 0 ? "SELL ALL" : "SELL ALL (" + sellAllGold + "g)";
    }

    private void sellAll()
    {
        if (sellAllGold == 0) return;

        foreach (MainCharacterItem item in player.GetComponent<MainCharacterInventory>().MainCharacterItems.ToArray())
        {
            if (!items.Exists(x => x.Item == item.Item && x.Buying == false)) continue;

            playerInventoryUI.Find(item.Item.name).gameObject.SetActive(false);
            player.GetComponent<MainCharacterInventory>().RemoveItem(item.Item, item.Amount);
        }

        player.GetComponent<MainCharacter>().Gold += sellAllGold;
        merchantUI.Find("Money").Find("Text").GetComponent<Text>().text = player.GetComponent<MainCharacter>().Gold + "g";

        sellAllGold = 0;
        merchantUI.Find("Sell all").Find("Text").GetComponent<Text>().text = "SELL ALL";
        sellGold = 0;
        merchantUI.Find("Sell selected").Find("Text").GetComponent<Text>().text = "SELL SELECTED";
    }

    private void removeAmountPlayerItem(GameObject button)
    {
        MerchantItem item = items.Find(x => x.Item.name == button.name);
        if (item.Amount == 0) return;
        item.Amount--;
        button.transform.Find("Amount selected").Find("Text").GetComponent<Text>().text = item.Amount.ToString();

        sellGold -= item.Price;
        merchantUI.Find("Sell selected").Find("Text").GetComponent<Text>().text = sellGold == 0 ? "SELL SELECTED" : "SELL SELECTED (" + sellGold + "g)";
    }

    private void addAmountPlayerItem(GameObject button)
    {
        MerchantItem item = items.Find(x => x.Item.name == button.name);
        if (item.Amount >= item.MaxAmount) return;
        item.Amount++;
        button.transform.Find("Amount selected").Find("Text").GetComponent<Text>().text = item.Amount.ToString();

        sellGold += item.Price;
        merchantUI.Find("Sell selected").Find("Text").GetComponent<Text>().text = "SELL SELECTED (" + sellGold + "g)";
    }

    private void removeAmountMerchantItem(GameObject button)
    {
        MerchantItem item = items.Find(x => x.Item.name == button.name);
        if (item.Amount == 0) return;
        item.Amount--;
        button.transform.Find("Amount").Find("Text").GetComponent<Text>().text = item.Amount.ToString();

        buyGold -= item.Price;
        merchantUI.Find("Buy").Find("Text").GetComponent<Text>().text = buyGold == 0 ? "BUY" : "BUY (" + buyGold + "g)";
    }

    private void addAmountMerchantItem(GameObject button)
    {
        MerchantItem item = items.Find(x => x.Item.name == button.name);
        if (item.Amount >= item.MaxAmount) return;
        item.Amount++;
        button.transform.Find("Amount").Find("Text").GetComponent<Text>().text = item.Amount.ToString();

        buyGold += item.Price;
        merchantUI.Find("Buy").Find("Text").GetComponent<Text>().text = "BUY (" + buyGold + "g)";
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerInteraction") && Input.GetButton("R") && !merchantUI.gameObject.activeSelf) Open();
    }
}