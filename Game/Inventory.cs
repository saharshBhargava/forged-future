using System.Collections.Generic;

class Inventory
{
    private static List<Collectable> inventory;
    private int numCoins;

    public Inventory()
    {
        inventory = new();
    }

    public List<Collectable> GetInventoryList()
    {
        return inventory;
    }

    public void AddToInventory(Collectable collectable)
    {
        inventory.Add(collectable); 
        if (collectable is Coin)
        {
            numCoins++;
        }
    }

    public void RemoveFromInventory(Collectable collectable)
    {
        inventory.Remove(collectable);
    }

    public void ClearInventory()
    {
        inventory = new();
    }

    public Collectable GetInventoryAtIndex(int index)
    {
        if (inventory.Count > 0 && inventory.Count > index) 
        {
            return inventory[index];
        }
        return null;
    }
}