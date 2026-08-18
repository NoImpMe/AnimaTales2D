using System.Collections.Generic;

public class ShopList
{
    public List<ShopItemData> defaultItems;
    public List<ShopItemData> randomItems;

    public List<ShopItemData> GetAllItems()
    {
        int totalCapacity = (defaultItems?.Count ?? 0) + (randomItems?.Count ?? 0);
        List<ShopItemData> all = new(totalCapacity);
    
        if (defaultItems != null) all.AddRange(defaultItems);
        if (randomItems != null) all.AddRange(randomItems);
        return all;
    }

    public static ShopList CreateForVillage(string villageID)
    {
        return new ShopList
        {
            defaultItems = DefaultShopItems.GetDefaultItems(),
            randomItems = VillageShopGenerator.GenerateRandomItemsForVillage(villageID, 2)
        };
    }
}