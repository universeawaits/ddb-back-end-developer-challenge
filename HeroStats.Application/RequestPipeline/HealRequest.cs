using MediatR;

namespace HeroStats.Application.RequestPipeline;

public class HealRequest : IRequest
{
    public string Hero { get; }
    public uint Points { get; }

    public HealRequest(string hero, uint points) => (Hero, Points) = (hero, points);
}
