using System.Collections.Generic;
using UnityEngine;

public static class VillageShopGenerator
{
    public static List<ShopItemData> GenerateRandomItemsForVillage(string villageID, int maxCount)
    {
        List<ShopItemData> pool = RandomShopItems.GetRandomItemPool();
        List<ShopItemData> result = new();

        int seed = villageID.GetHashCode();
        Random.InitState(seed);

        while (result.Count < maxCount && result.Count < pool.Count)
        {
            var item = pool[Random.Range(0, pool.Count)];
            if (!result.Exists(i => i.itemID == item.itemID))
            {
                result.Add(item);
            }
        }

        return result;
    }
}