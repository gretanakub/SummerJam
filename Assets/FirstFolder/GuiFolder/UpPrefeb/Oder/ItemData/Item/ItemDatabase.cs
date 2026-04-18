using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Game/FirstFolder/GuiFolder/UpPrefeb/Oder/ItemData/Item")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;

    private Dictionary<ItemType, Sprite> itemDict;

    public void Init()
    {
        itemDict = new Dictionary<ItemType, Sprite>();

        foreach (var item in items)
        {
            itemDict[item.type] = item.icon;
        }
    }

    public Sprite GetSprite(ItemType type)
    {
        if (itemDict == null)
            Init();

        if (itemDict.ContainsKey(type))
            return itemDict[type];

        Debug.LogWarning("Item not found: " + type);
        return null;
    }
}