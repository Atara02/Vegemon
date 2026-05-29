using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public Animator m_anim = null;

    public string m_name;

    public int m_maxHp;
    public int m_curHp;

    public int m_attack;
    public int m_defense;

    public float m_speed;

    public List<BuffBase> m_buffs = new();

    public bool IsDead => m_curHp <= 0;
    public bool IsAttack { get; set; }
    public bool IsBlocking { get; set; }

    public event Action OnUnitAttack;
    public event Action OnUnitQuitAttack;

    public void Init()
    {
        m_curHp = m_maxHp;
    }

    // =========================
    // 애니메이션 제어
    // =========================
    bool CanControlAnimtion
    {
        get
        {
            if (m_anim == null)
            {
                m_anim = GetComponent<Animator>();
            }
            return m_anim != null;
        }
    }
    public void PlayAttackAnimation()
    {
        if(CanControlAnimtion)
        {
            m_anim.SetTrigger("Attack");
        }
    }
    public void PlayBlockingAnimation()
    {
        if (CanControlAnimtion)
        {
            m_anim.SetTrigger("Block");
        }
    }
    public void PlayDeadAnimation()
    {
        if (CanControlAnimtion)
        {
            m_anim.SetTrigger("Dead");
        }
    }

    void AttackEvent()
    {
        OnUnitAttack?.Invoke();
        OnUnitAttack = null;
    }
    void QuitAttackEvent()
    {
        OnUnitQuitAttack?.Invoke();
        OnUnitQuitAttack = null;
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
    bool Blocking()
    {
        PlayBlockingAnimation();
        return false;
    }
    public void TakeDamage(DamageData damage)
    {
        if (Blocking() == false)
        {
            m_curHp -= damage.m_damage;
            Debug.Log(
                $"{damage.m_attacker.m_name} -> " +
                $"{m_name} " +
                $"{damage.m_damage} 데미지"
            );
        }
        if (m_curHp <= 0)
        {
            PlayDeadAnimation();
            return;
        }
        foreach (var buff in m_buffs)
        {
            buff.OnDamaged(damage);
        }
    }
}
