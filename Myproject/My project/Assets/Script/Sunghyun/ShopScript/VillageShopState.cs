using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VillageShopState
{
    public string villageID;
    
    public Dictionary<string, int> remainingCounts = new Dictionary<string, int>();
    
    public VillageShopState(string villageID, List<ShopItemData> items)
    {
        this.villageID = villageID;
        
        foreach (var item in items)
        {
            remainingCounts[item.itemID] = item.maxPurchaseCount;
        }
    }
    
    public bool PurchaseItem(string itemID)
    {
        if (!remainingCounts.ContainsKey(itemID) || remainingCounts[itemID] <= 0)
        {
            return false;
        }
        
        remainingCounts[itemID]--;
        return true;
    }
    
    public int GetRemainingCount(string itemID)
    {
        remainingCounts.TryGetValue(itemID, out int count);
        return count;
    }
    
    public bool IsPurchasable(string itemID)
    {
        return remainingCounts.TryGetValue(itemID, out int count) && count > 0;
    }
}