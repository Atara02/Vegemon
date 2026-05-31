using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public GameManager m_gm;
    public InventoryManager m_im;
    public EnemyData m_defStatus;
    public int m_stage = 1;

    public int m_maxCount;
    public int m_killCount;

    public int m_getSoul;   //장비 제작
    public int m_getSeed;   //재배

    public event System.Action<int> onUpdateKillCount;
    public event System.Action<int> onUpdateSoulAmount;
    public event System.Action<int> onUpdateSeedAmount;

    protected override void Awake()
    {
        base.Awake();

        if(m_gm == null) { 
            m_gm = FindFirstObjectByType<GameManager>();
        }
        m_gm.onGamePause += BattlePause;
        m_gm.onGameClear += StageClear;
        m_gm.onGameOver += BattleOver;

        m_killCount = 0;
        m_getSoul = 0;
        m_getSeed = 0;
        m_defStatus = ResourceLoader.LoadData<EnemyData>("00_Data/00_Enemy/Enemy_" + m_stage.ToString());
    }

    private void Start()
    {
        if(m_im == null) { m_im = InventoryManager.Instance; }
    }

    public int CalculateEnemyHp()
    {
        int hpBonusMin = m_killCount;
        int hpBonusMax = m_killCount * 2;
        int finalHp = m_defStatus.m_hp + Random.Range(hpBonusMin, hpBonusMax + 1);
        return CheckEliteBonus(finalHp);
    }
    public int CalculateEnemyAtk()
    {
        int atkBonusMin = m_killCount / 5;
        int atkBonusMax = m_killCount / 3;
        int finalAtk = m_defStatus.m_atk + Random.Range(atkBonusMin, atkBonusMax + 1);
        return CheckEliteBonus(finalAtk);
    }
    public int CalculateEnemyDef()
    {
        int defBonusMin = m_killCount / 10;
        int defBonusMax = m_killCount / 5;
        int finalDef = m_defStatus.m_def + Random.Range(defBonusMin, defBonusMax + 1);
        return CheckEliteBonus(finalDef);
    }

    int CheckEliteBonus(int baseStat)
    {
        int index = m_killCount + 1;
        if(index == m_maxCount)//보스
        {
            //3 ~ 5배 더 강하게
            return Mathf.RoundToInt(baseStat * Random.Range(30, 51) / 10f);
        }
        else if (index % 5 == 0)//엘리트
        {
            //1.5 ~ 3배 더 강하게
            return Mathf.RoundToInt(baseStat * Random.Range(15, 31) / 10f);
        }
        else
        {
            return baseStat;
        }
    }

    public void KillMob()
    {
        m_killCount = Mathf.Min(m_killCount + 1, m_maxCount);

        int dropBonusMin = m_killCount / 5;
        int dropBonusMax = m_killCount / 3;
        int finalSoul = m_defStatus.m_drop + Random.Range(dropBonusMin, dropBonusMax + 1);
        m_getSoul += finalSoul;

        if(m_killCount % 5 == 0)
        {
            m_getSeed++;
        }
        if (m_killCount == m_maxCount)
        {
            m_gm.GameClear();
        }
    }


    void StageClear()
    {
        UpdateCount();
        UpdateInventory();
    }
    void BattleOver()
    {
        UpdateCount();
        UpdateInventory();
    }
    void BattlePause()
    {
        UpdateCount();
    }

    void UpdateInventory()
    {
        if (m_im)
        {
            m_im.AddSoul(m_getSoul);
            m_im.AddSeed(m_getSeed);
            m_im.SaveProperty();
        }
    }
    void UpdateCount()
    {
        //Update UI
        onUpdateKillCount?.Invoke(m_killCount);
        onUpdateSoulAmount?.Invoke(m_getSoul);
        onUpdateSeedAmount?.Invoke(m_getSeed);
    }
}
