using HeroStats.Domain.Action.Damage;
using MediatR;

namespace HeroStats.Application.RequestPipeline;

public class DamageRequest : IRequest
{
    public DamageType Type { get; }
    public uint Amount { get; }

    public DamageRequest(DamageType type, uint amount) => (Type, Amount) = (type, amount);
}
