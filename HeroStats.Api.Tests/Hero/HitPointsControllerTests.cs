using FluentAssertions;
using HeroStats.Api.Hero;
using HeroStats.Application.Hero;
using HeroStats.Domain.Action.Damage;
using HeroStats.Infrastructure.DataAccess.Concrete;
using HeroStats.Infrastructure.Hero.Repository;
using MediatR;
using Moq;
using NUnit.Framework;

namespace HeroStats.Api.Tests.Hero;

public class HitPointsControllerTests
{
    private const string HeroName = "nils";
    private HeroRepository _repository;
    private HitPointsController _controller;

    [SetUp]
    public void Setup()
    {
        var mediator = new Mock<IMediator>();

        var hero = new Domain.Hero.Hero { Name = HeroName, HitPoints = 1000 };
        var db = new InMemoryDatabaseContext { Database = { [typeof(Domain.Hero.Hero)] = new List<object>() } };
        db.Database[typeof(Domain.Hero.Hero)].Add(hero);

        _repository = new HeroRepository(db);
        var damageHandler = new DamageRequestHandler(_repository);
        var healHandler = new HealRequestHandler(_repository);
        var tempHealHandler = new TemporaryHealRequestHandler(_repository);
        mediator.Setup(x => x.Send(It.IsAny<DamageRequest>(), CancellationToken.None))
            .Returns<DamageRequest, CancellationToken>((x, y) => damageHandler.Handle(x, y));
        mediator.Setup(x => x.Send(It.IsAny<HealRequest>(), CancellationToken.None))
            .Returns<HealRequest, CancellationToken>((x, y) => healHandler.Handle(x, y));
        mediator.Setup(x => x.Send(It.IsAny<TemporaryHealRequest>(), CancellationToken.None))
            .Returns<TemporaryHealRequest, CancellationToken>((x, y) => tempHealHandler.Handle(x, y));

        _controller = new HitPointsController(mediator.Object);
    }

    [Test]
    public async Task Damage_ForAKnownHero_AppliedCorrectly()
    {
        // Arrange
        var hero = _repository.Get(HeroName);
        hero.SetCurrentPersistent(hero.HitPoints);
        hero.Defenses[DamageType.Piercing] = DefenseType.Resistance;
        
        // Act
        await _controller.Damage(new() { Hero = HeroName, Amount = 7, Type = DamageType.Piercing });
        
        // Assert
        var updated = _repository.Get(HeroName);
        updated.CurrentPersistentHp.Should().Be(996u);
    }

    [Test]
    public async Task Healing_ForAUnknownHero_Healed()
    {
        // Arrange
        var hero = _repository.Get(HeroName);
        hero.SetCurrentPersistent(hero.HitPoints - 3);
        
        // Act
        await _controller.Healing(new() { Hero = HeroName, Points = 2 });

        // Assert
        var updated = _repository.Get(HeroName);
        updated.CurrentPersistentHp.Should().Be(hero.HitPoints - 1);
    }

    [Test]
    public async Task TemporaryHealing_ForAUnknownHero_Healed()
    {
        // Arrange
        var hero = _repository.Get(HeroName);
        hero.SetCurrentPersistent(hero.HitPoints - 3);
        hero.TemporaryHp = 5;
        
        // Act
        await _controller.TemporaryHealing(new() { Hero = HeroName, Points = 6 });

        // Assert
        var updated = _repository.Get(HeroName);
        updated.CurrentPersistentHp.Should().Be(hero.HitPoints - 3);
        updated.TemporaryHp.Should().Be(6u);
    }
}
