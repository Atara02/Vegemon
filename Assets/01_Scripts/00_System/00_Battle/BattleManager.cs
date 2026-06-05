using System.Collections.Generic;
using UnityEngine;


public class BattleManager : Singleton<BattleManager>
{
    GameManager m_gm = null;
    InventoryManager m_im;
    VegemonDB m_db = null;

    private float[] m_battleSpeed = new float[4] { 1f, 1.5f, 2f, 3f };
    private int m_speedIndex = 0;

    [Header("<스테이지 정보>")]
    [SerializeField] VegemonData m_enemy = null;
    [SerializeField] int m_stage = 1;
    [SerializeField] int m_maxCount = 30;
    [SerializeField] int m_killCount = 0;

    [Header("<드랍품>")]
    [SerializeField] int m_getSoul = 0;   //장비 제작
    [SerializeField] int m_getSeed = 0;   //재배

    public event System.Action<int> onUpdateKillCount;
    public event System.Action<int> onUpdateSoulAmount;
    public event System.Action<int> onUpdateSeedAmount;

    protected override void Awake()
    {
        base.Awake();
        if (m_gm == null)
        {
            m_gm = FindFirstObjectByType<GameManager>();
            m_gm.onGamePause += BattlePause;
            m_gm.onGameClear += StageClear;
            m_gm.onGameOver += BattleOver;
        }
        if (m_db == null)
        {
            m_db = GetComponent<VegemonDB>();
            m_enemy = m_db.GetStageEnemy(m_stage);
        }

        m_maxCount = 30;
        m_killCount = 0;
        m_getSoul = 0;
        m_getSeed = 0;
    }
    private void Start()
    {
        if (m_im == null) { m_im = InventoryManager.Instance; }
        InitSpeed();
    }

    public int GetEnemyHp()
    {
        int hpBonusMin = m_killCount;
        int hpBonusMax = m_killCount * 2;
        int finalHp = m_enemy.m_hp + Random.Range(hpBonusMin, hpBonusMax + 1);
        return CheckEliteBonus(finalHp);
    }
    public int GetEnemyAtk()
    {
        int atkBonusMin = m_killCount / 5;
        int atkBonusMax = m_killCount / 3;
        int finalAtk = m_enemy.m_atk + Random.Range(atkBonusMin, atkBonusMax + 1);
        return CheckEliteBonus(finalAtk);
    }
    public int GetEnemyDef()
    {
        int defBonusMin = m_killCount / 10;
        int defBonusMax = m_killCount / 5;
        int finalDef = m_enemy.m_def + Random.Range(defBonusMin, defBonusMax + 1);
        return CheckEliteBonus(finalDef);
    }
    public int GetEnemySpd()
    {
        int spdBonusMin = m_killCount / 15;
        int spdBonusMax = m_killCount / 10;

        int finalSpd = m_enemy.m_spd + Random.Range(spdBonusMin, spdBonusMax + 1);
        return finalSpd;
    }
    public int GetEnemyLuk()
    {
        int luk = m_enemy.m_luk;
        int index = m_killCount + 1;
        if (index == m_maxCount) { luk *= 3; }
        else if (index % 5 == 0) { luk *= 2; }
        return luk;
    }

    int CheckEliteBonus(int baseStat)
    {
        int index = m_killCount + 1;
        if (index == m_maxCount)//보스
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
        int finalSoul = m_stage + Random.Range(dropBonusMin, dropBonusMax + 1);

        m_getSoul += finalSoul;

        if (m_killCount % 5 == 0)
        {
            m_getSeed++;
            if (m_killCount == m_maxCount)
            {
                m_getSeed++;
                m_gm.GameClear();
            }
        }
    }


    void StageClear()
    {
        InitSpeed();

        UpdateCount();
        UpdateInventory();
    }
    void BattleOver()
    {
        InitSpeed();

        UpdateCount();
        UpdateInventory();
    }
    void BattlePause()
    {
        Time.timeScale = 0;
        UpdateCount();
    }
    void BattlePlay()
    {
        Time.timeScale = m_battleSpeed[m_speedIndex];
    }

    void UpdateInventory()
    {
        if (m_im)
        {
            m_im.AddSoul(m_getSoul);
            m_im.AddSeed(m_enemy.m_type, m_getSeed);
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


    #region <Battle Speed>
    void InitSpeed()
    {
        m_speedIndex = 0;
        Time.timeScale = m_battleSpeed[m_speedIndex];
    }
    public int UpSpeed()
    {
        m_speedIndex++;
        if(m_speedIndex >= m_battleSpeed.Length) { m_speedIndex = 0; }
        Time.timeScale = m_battleSpeed[m_speedIndex];
        return m_speedIndex;
    }
    #endregion
}
