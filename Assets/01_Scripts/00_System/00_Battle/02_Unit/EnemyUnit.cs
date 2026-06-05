using DG.Tweening;
using UnityEngine;

public class EnemyUnit : BattleUnit
{
    private Vector2 m_spawnPoint;

    protected override void Awake()
    {
        base.Awake();
        m_initPoint = transform.position;
        m_spawnPoint = transform.position;

        Vector2 center = Camera.main.transform.position;
        float height = Camera.main.orthographicSize * 2f;
        float width = height * ((float)Screen.width / Screen.height);
        int sign = m_initPoint.x <= center.x ? -1 : 1;
        m_spawnPoint.x += sign * (width * 0.5f);
    }

    public override void SpawnUnit(TweenCallback callback)
    {
        SetPosition(m_spawnPoint);
        if (CanControlRenderer)
        {
            Color alpha = m_rend.color;
            alpha.a = 1;
            m_rend.color = alpha;
        }
        PlayIdleAnimation();
        PlayMoveAnimation();
        transform.DOMove(m_initPoint, 0.5f).OnComplete(callback);
    }
}
