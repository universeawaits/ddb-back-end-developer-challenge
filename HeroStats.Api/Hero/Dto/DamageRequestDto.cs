using HeroStats.Domain.Action.Damage;

namespace HeroStats.Api.Hero.Dto;

public class DamageRequestDto
{
    public string Hero { get; init; }
    public DamageType Type { get; init; }
    public uint Amount { get; init; }
}
