using System;

public abstract class CombatStatModifier
{
    public bool MarkedForRemoval { get; private set; }
    public event Action<CombatStatModifier> OnDisposed = delegate { };
    
    private readonly CountdownTimer timer;

    protected CombatStatModifier(float duration)
    {
        if(duration <= 0) return;
        
        timer = new CountdownTimer(duration);
        timer.OnTimerStop += () => MarkedForRemoval = true;
        timer.Start();
    }
    
    public abstract void Handle(object sender, Query query);

    public void Update(float deltaTime) => timer?.Tick(deltaTime);
    public void Dispose()
    {
        MarkedForRemoval = true;
        OnDisposed.Invoke(this);
    }
}