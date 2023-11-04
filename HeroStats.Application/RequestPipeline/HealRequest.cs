using MediatR;

namespace HeroStats.Application.RequestPipeline;

public class HealRequest : IRequest
{
    public string Hero { get; init; }
    public uint Points { get; init; }
}
