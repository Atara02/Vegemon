public abstract class BuffBase
{
    public BattleUnit m_owner;

    public string m_buffName;
    public int m_duration;
    public int m_remain;

    public void Init(BattleUnit owner)
    {
        m_owner = owner;
        m_remain = m_duration;
    }

    // =========================
    // 생성
    // =========================
    public virtual void OnApply()
    {
    }

    // =========================
    // 제거
    // =========================
    public virtual void OnRemove()
    {
    }

    // =========================
    // 공격 직전
    // =========================
    public virtual void OnAttack(ref int damage)
    {
    }


    public virtual void OnCritical(ref int damage)
    {

    }

    // =========================
    // 피격 직전
    // =========================
    public virtual void OnDodge()    //블로킹 성공 시
    {

    }
    public virtual void OnFailDodge(ref bool dodge)    //블로킹 실패 시
    {

    }
    public virtual void OnTakeDamage(ref int damage)
    {
    }

    // =========================
    // 피격 후
    // =========================
    public virtual void OnDamaged(DamageData damage)
    {
    }
    // =========================
    // 사망 시
    // =========================
    public virtual void OnDead(DamageData damage)
    {
    }
}