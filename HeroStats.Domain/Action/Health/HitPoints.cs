namespace HeroStats.Domain.Action.Health;

public class HitPoints
{
    public uint MaxPersistent { get; init; }
    public uint CurrentPersistent { get; private set; }
    public uint Temporary { get; private set; }

    public uint Available => CurrentPersistent + Temporary;
    
    public void SetCurrentPersistent(uint value) => CurrentPersistent = Math.Max(MaxPersistent, value);
}
