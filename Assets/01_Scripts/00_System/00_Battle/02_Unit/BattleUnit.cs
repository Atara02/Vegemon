using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    protected Animator m_anim = null;
    protected SpriteRenderer m_rend = null;

    protected Vector2 m_initPoint;

    public string m_name;

    public int m_maxHp;
    public int m_curHp;

    public int m_atk;
    public int m_def;

    public int m_spd;
    public int m_luk;

    public List<BuffBase> m_buffs = new();

    public float Speed => 0.25f * m_spd;
    public bool IsDead => m_curHp <= 0;

    public event System.Action OnUnitAttack;
    public event System.Action OnUnitQuitAttack;
    public event System.Action OnUnitDead;

    public event System.Action<float> OnUpdateHp;

    protected virtual void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_rend = GetComponent<SpriteRenderer>();
    }

    public void Init()
    {
        PlayIdleAnimation();

        m_curHp = m_maxHp;
        OnUpdateHp?.Invoke((float)m_curHp / m_maxHp);
    }
    public void SetStat(int hp, int atk, int def, int spd, int luk)
    {
        m_maxHp = hp;
        m_atk = atk;
        m_def = def;
        m_spd = spd;
        m_luk = luk;
    }

    protected bool CanControlRenderer
    {
        get
        {
            if (m_rend == null)
            {
                m_rend = GetComponent<SpriteRenderer>();
                return m_rend != null;
            }
            return m_rend != null;
        }
    }
    public virtual void SpawnUnit(TweenCallback callback)
    {
        SetPosition(m_initPoint);
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
    public void PlayIdleAnimation()
    {
        if (CanControlAnimtion)
        {
            m_anim.SetBool("Dead", false);
            m_anim.SetBool("Move", false);
            m_anim.Play("Idle");
        }
    }
    public void PlayMoveAnimation()
    {
        if (CanControlAnimtion)
        {
            m_anim.SetBool("Move", true);
        }
    }
    public void PlayAttackAnimation()
    {
        if (CanControlAnimtion)
        {
            m_anim.SetTrigger("Attack");
        }
    }
    public void PlayHitAnimation()
    {
        if (CanControlAnimtion)
        {
            m_anim.SetTrigger("Hit");
        }
    }
    public void PlayDeadAnimation()
    {
        if (CanControlAnimtion)
        {
            m_anim.SetBool("Dead", true);
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
    void QuitDeathEvent()
    {
        if (CanControlRenderer)
        {
            m_rend.DOFade(0f, 0.5f).OnComplete(() =>
            {
                OnUnitDead?.Invoke();
                OnUnitDead = null;
            });
        }
    }


    public void SetPosition(Vector2 position)
    {
        transform.position = position;
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
    int Critical(int damage)
    {
        int def = 5;
        int add = 1 * m_luk;

        float final = (def + add) * 0.01f;
        if (Random.value <= final)
        {
            damage = Mathf.RoundToInt(damage * 1.5f);
            foreach (var buff in m_buffs)
            {
                buff.OnCritical(ref damage);
            }
        }
        return damage;
    }
    public int FinalDamage()
    {
        int damage = m_atk;
        foreach (var buff in m_buffs)
        {
            buff.OnAttack(ref damage);
        }
        return Critical(damage);
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
        int defense = m_def;
        foreach (var buff in m_buffs)
        {
            buff.OnTakeDamage(ref defense);
        }
        return defense;
    }
    bool Dodge()
    {
        bool dodge = false;

        int def = 5;
        int add = 1 * m_luk;
        float final = (def + add) * 0.01f;
        if (Random.value <= final)
        {
            foreach (var buff in m_buffs)
            {
                buff.OnDodge();
            }
            return true;
        }

        //실패
        foreach (var buff in m_buffs)
        {
            buff.OnFailDodge(ref dodge);
        }
        return dodge;
    }
    public void TakeDamage(DamageData damage)
    {
        if (Dodge()) { return; }
        PlayHitAnimation();
        m_curHp -= damage.m_damage;
        Debug.Log(
            $"{damage.m_attacker.m_name} -> " +
            $"{m_name} " +
            $"{damage.m_damage} 데미지"
        );
        //Dead
        if (m_curHp <= 0)
        {
            PlayDeadAnimation();
        }
        else
        {
            foreach (var buff in m_buffs)
            {
                buff.OnDamaged(damage);
            }
        }
        OnUpdateHp?.Invoke((float)m_curHp / m_maxHp);
    }
}
