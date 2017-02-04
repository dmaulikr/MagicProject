using UnityEngine;
using System;

public enum Potion
{
    RED, GREEN, YELLOW, BLUE
}

public class Inventory : MonoBehaviour {

    [SerializeField]
    private int maxPotions_ = 99;

    [SerializeField]
    private int[] potions_ = new int[4];
    [SerializeField]
    private int coins_;

    private bool AddPotion(Potion color, int amount = 1)
    {
        #if UNITY_EDITOR
        if (amount < 0)
            Error.ShowError("Cannot add a negative amount of potions");
        #endif

        if (potions_[(int)color] + amount > maxPotions_) return false;
        potions_[(int)color] += amount;
        return true;
    }
    private bool RemovePotion(Potion color, int amount = 1)
    {
        #if UNITY_EDITOR
        if (amount < 0)
            Error.ShowError("Cannot remove a negative amount of potions");
        #endif

        if (potions_[(int)color] < amount) return false;
        potions_[(int)color] -= amount;
        return true;
    }

    private bool AddCoins(int amount)
    {
        #if UNITY_EDITOR
        if (amount < 0)
            Error.ShowError("Cannot add a negative amount of coins");
        #endif

        coins_ += amount;
        return true;
    }
    private bool RemoveCoins(int amount)
    {
        #if UNITY_EDITOR
        if (amount < 0)
            Error.ShowError("Cannot remove a negative amount of coins");
        #endif

        if (coins_ < amount) return false;
        coins_ -= amount;
        return true;
    }

    public bool AddItem(ItemObject obj)
    {
        switch (obj.type)
        {
            case ItemType.POTION:
                AddPotion(obj.potionColor);
                break;
            case ItemType.COINS:
                AddCoins(obj.itemQuantity);
                break;
        }
        return false;
    }
}
