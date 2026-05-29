using System;

public abstract class StateBase<T>
{
    protected T m_sys = default(T);
    public abstract void Init(T value);
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
