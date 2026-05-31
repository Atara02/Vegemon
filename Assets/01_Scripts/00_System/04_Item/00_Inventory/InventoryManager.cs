using UnityEngine;

public class PropertyData
{
    int m_soul = 0;
    int m_seed = 0;

    public PropertyData(int soul = 0, int seed = 0)
    {
        m_soul = soul;
        m_seed = seed;
    }
    public void SetSoul(int soul) { m_soul += soul; }
    public void SetSeed(int seed) { m_seed += seed; }

    public int GetSoul => m_soul;
    public int GetSeed => m_seed;
}
public class InventoryManager : Singleton<InventoryManager>
{
    public PropertyData m_property = null;

    public int m_soul = 0;
    public int m_seed = 0;

    string m_dataKey = "Property";

    public event System.Action<int> onLoadSoul;
    public event System.Action<int> onLoadSeed;
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
        onLoadSeed?.Invoke(m_property.GetSeed);

        //For Check
        m_soul = m_property.GetSoul;
        m_seed = m_property.GetSeed;
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

    public void AddSeed(int seed)
    {
        seed = seed < 0 ? 0 : seed;
        m_property.SetSeed(seed);
    }
    public void DelSeed(int seed)
    {
        seed = seed < 0 ? 0 : seed;
        m_property.SetSeed(seed);
    }
}
