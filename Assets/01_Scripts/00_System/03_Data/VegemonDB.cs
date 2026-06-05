using System.Collections.Generic;
using UnityEngine;

public class VegemonDB : Singleton<VegemonDB>
{
    public VegemonDataSheet m_sheet;

    public Dictionary<int, VegemonData> m_vegemons;
    public Dictionary<int, VegemonData> m_enemies;
    protected override void Awake()
    {
        base.Awake();
        InitVegemonData();
        InitEnemyData();
    }
    #region <Init>
    void InitVegemonData()
    {
        m_vegemons = new Dictionary<int, VegemonData>();
        foreach (VegemonData vegemon in m_sheet.VegemonSheet)
        {
            int key = vegemon.m_id;
            if (!m_vegemons.ContainsKey(key))
            {
                m_vegemons.Add(key, vegemon);
            }
        }
    }
    void InitEnemyData()
    {
        m_enemies = new Dictionary<int, VegemonData>();
        foreach (VegemonData enemy in m_sheet.EnemySheet)
        {
            int key = enemy.m_id;
            if (!m_enemies.ContainsKey(key))
            {
                m_enemies.Add(key, enemy);
            }
        }
    }
    #endregion

    public VegemonData GetVegemonData(int id)
    {
        if (m_vegemons.ContainsKey(id))
        {
            return m_vegemons[id];
        }
        return null;
    }

    public VegemonData GetStageEnemy(int stage)
    {
        int id = stage - 1;
        if(m_enemies.ContainsKey(id))
        {
            return m_enemies[id];
        }
        Debug.Log($"<Error!> 에너미 정보 없음!");
        return null;
    }
}
