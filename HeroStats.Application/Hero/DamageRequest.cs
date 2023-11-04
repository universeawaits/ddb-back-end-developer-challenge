using HeroStats.Domain.Action.Damage;
using MediatR;

namespace HeroStats.Application.Hero;

public class DamageRequest : IRequest
{
    public string Hero { get; init; }
    public DamageType Type { get; init; }
    public uint Amount { get; init; }
}
