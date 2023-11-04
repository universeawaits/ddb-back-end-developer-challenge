using HeroStats.Api.Hero.Dto;
using HeroStats.Application.Hero;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HeroStats.Api.Hero;

[AllowAnonymous]
[ApiController]
[Route("/api/hp")]
public class HitPointsController : ControllerBase
{
    private readonly IMediator _mediator;

    public HitPointsController(IMediator mediator) => _mediator = mediator;

    [HttpDelete]
    public async Task<ActionResult> Damage([FromBody] DamageRequestDto dto)
    {
        var request = new DamageRequest { Amount = dto.Amount, Type = dto.Type, Hero = dto.Hero };
        await _mediator.Send(request);

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> Healing([FromBody] HealthRequestDto dto)
    {
        var request = new HealRequest { Hero = dto.Hero, Points = dto.Points };
        await _mediator.Send(request);

        return Ok();
    }

    [HttpPost]
    [Route("temporary")]
    public async Task<ActionResult> TemporaryHealing([FromBody] HealthRequestDto dto)
    {
        var request = new TemporaryHealRequest { Hero = dto.Hero, Points = dto.Points };
        await _mediator.Send(request);

        return Ok();
    }
}
