
using UnityEngine;

[System.Serializable]
public class PropertyData
{
    int m_soul;
    int[] m_seed;

    public PropertyData(int soul = 0)
    {
        m_soul = soul;
        m_seed = new int[10];
    }
    public void SetSoul(int soul) { m_soul += soul; }
    public void SetSeed(int id, int seed)
    {
        if (id < 0 || id >= m_seed.Length) { return; }
        m_seed[id] += seed;
    }

    public int GetSoul => m_soul;
    public int? GetSeed(int id)
    {
        if (m_seed.Length == 0 || id >= m_seed.Length) { return null; }
        return m_seed[id];
    }
}
public class InventoryManager : Singleton<InventoryManager>
{
    public string m_dataKey = "Property";
    public PropertyData m_property = null;

    public event System.Action<int> onLoadSoul;
    public event System.Action<int, int> onLoadSeed;

    private void Start()
    {
        LoadProperty();
    }
    public void SaveProperty()
    {
        SaveAndLoader.SaveData(m_property, m_dataKey);
    }
    public void LoadProperty()
    {
        m_property = new PropertyData();
        m_property = SaveAndLoader.LoadData(m_property, m_dataKey);

        onLoadSoul?.Invoke(m_property.GetSoul);
        for (int i = 0; i < 10; i++)
        {
            int? count = m_property.GetSeed(i);
            if (count.HasValue) { onLoadSeed?.Invoke(i, count.Value); }
        }
    }

    public void AddSoul(int soul)
    {
        soul = soul < 0 ? 0 : soul;
        m_property.SetSoul(soul);
    }
    public void DelSoul(int soul)
    {
        soul = soul < 0 ? 0 : soul;
        m_property.SetSoul(soul);
    }

    public void AddSeed(string type, int seed)
    {
        seed = seed < 0 ? 0 : seed;
        m_property.SetSeed(SeedType(type), seed);
    }
    public void DelSeed(string type, int seed)
    {
        seed = seed < 0 ? 0 : seed;
        m_property.SetSeed(SeedType(type), seed);
    }

    int SeedType(string type)
    {
        type = type.ToUpper();
        switch (type)
        {
            case "FIRE": return 0;
            case "WATER": return 1;
            case "EARTH": return 2;
            case "AIR": return 3;
            case "LIGHT": return 4;
            case "DARK": return 5;
            default:
                Debug.Log($"<Error!> 타입외 씨앗입니다!");
                return -1;

        }
    }
}
