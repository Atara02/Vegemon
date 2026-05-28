using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BattleUnit
{
    public string m_name;

    public int m_maxHp;
    public int m_curHp;

    public int m_attack;
    public int m_defense;

    public float m_speed;

    public List<BuffBase> m_buffs = new();

    public bool IsDead => m_curHp <= 0;

    public void Init()
    {
        m_curHp = m_maxHp;
    }

    // =========================
    // 버프 추가
    // =========================
    public void AddBuff(BuffBase buff)
    {
        m_buffs.Add(buff);

        buff.Init(this);
        buff.OnApply();
    }

    // =========================
    // 공격
    // =========================

    public int FinalDamage()
    {
        int damage = m_attack;
        foreach (var buff in m_buffs)
        {
            buff.OnAttack(ref damage);
        }
        return damage;
    }
    public void Attack(BattleUnit target)
    {
        int damage = FinalDamage();
        int defense = target.FinalDefense();

        DamageData data = new DamageData(this, target, Mathf.Max(1, damage - defense));
        target.TakeDamage(data);
    }

    // =========================
    // 데미지 처리
    // =========================
    public int FinalDefense()
    {
        int defense = m_defense;
        foreach (var buff in m_buffs)
        {
            buff.OnTakeDamage(ref defense);
        }
        return defense;
    }
    public void TakeDamage(DamageData damage)
    {
        m_curHp -= damage.m_damage;
        Debug.Log(
            $"{damage.m_attacker.m_name} -> " +
            $"{m_name} " +
            $"{damage.m_damage} 데미지"
        );

        if (m_curHp <= 0)
        {
            m_curHp = 0;
            foreach (var buff in m_buffs)
            {
                buff.OnDead(damage);
            }
            return;
        }

        foreach (var buff in m_buffs)
        {
            buff.OnDamaged(damage);
        }
    }
}
