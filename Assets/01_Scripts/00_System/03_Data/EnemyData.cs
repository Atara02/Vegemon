using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_", menuName = "Scriptable Object/Enemy", order = int.MinValue)]
public class EnemyData : ScriptableObject
{
    [Header("<기본 능력치>")]
    public int m_hp;
    public int m_atk;
    public int m_def;

    [Header("<특수 능력치>")]
    public float m_spd;
    public float m_block;

    [Header("<드랍>")]
    public int m_drop;
}
