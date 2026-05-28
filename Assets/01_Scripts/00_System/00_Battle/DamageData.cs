public class DamageData
{
    public BattleUnit m_attacker;
    public BattleUnit m_target;

    public int m_damage;

    public bool m_isCritical;

    public DamageData(BattleUnit attacker, BattleUnit target, int damage, bool isCritical = false)
    {
        m_attacker = attacker;
        m_target = target;
        m_damage = damage;
        m_isCritical = isCritical;
    }
}