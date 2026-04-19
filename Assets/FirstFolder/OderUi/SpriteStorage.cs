using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteStorage : MonoBehaviour
{
    [System.Serializable]
    public class SpriteData
    {
        public string name;
        public Sprite sprite;
    }

    public List<SpriteData> spriteList = new List<SpriteData>();

    private Dictionary<string, Sprite> spriteDict;

    void Awake()
    {
        spriteDict = new Dictionary<string, Sprite>();

        foreach (var data in spriteList)
        {
            if (data == null)
            {
                Debug.LogWarning("SpriteData is NULL");
                continue;
            }

            if (string.IsNullOrEmpty(data.name))
            {
                Debug.LogWarning("Sprite name is EMPTY");
                continue;
            }

            if (data.sprite == null)
            {
                Debug.LogWarning("Sprite is NULL: " + data.name);
                continue;
            }

            if (!spriteDict.ContainsKey(data.name))
            {
                spriteDict.Add(data.name, data.sprite);
            }
        }
    }

    public Sprite GetSprite(string name)
    {
        if (spriteDict.TryGetValue(name, out Sprite sprite))
        {
            return sprite;
        }

        Debug.LogWarning("Sprite not found: " + name);
        return null;
    }

    // Just for testing, you can remove this later

}
//SpawnOder( MenuSprite , ????????????{int} , ????{float}(??????????? Default = 15) ,Sprite{1},Sprite{2},Sprite{3},...{?????????????????? ????????})