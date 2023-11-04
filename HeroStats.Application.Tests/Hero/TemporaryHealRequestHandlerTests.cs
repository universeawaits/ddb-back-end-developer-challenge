using FluentAssertions;
using HeroStats.Application.Hero;
using HeroStats.Infrastructure.DataAccess.Concrete;
using HeroStats.Infrastructure.Hero.Repository;
using NUnit.Framework;

namespace HeroStats.Application.Tests.Hero;

public class TemporaryHealRequestHandlerTests
{
    private const string HeroName = "nils";
    private HeroRepository _repository;
    private TemporaryHealRequestHandler _handler;

    [SetUp]
    public void Setup()
    {
        var hero = new Domain.Hero.Hero { Name = HeroName, HitPoints = 1000 };
        var db = new InMemoryDatabaseContext { Database = { [typeof(Domain.Hero.Hero)] = new List<object>() } };
        db.Database[typeof(Domain.Hero.Hero)].Add(hero);

        _repository = new HeroRepository(db);
        _handler = new TemporaryHealRequestHandler(_repository);
    }

    [Test]
    public async Task Handle_SeveralTimes_DoenstAccumulateButChoosesMaximum()
    {
        // Arrange
        var hero = _repository.Get(HeroName);
        hero.SetCurrentPersistent(hero.HitPoints - 1);
        
        await _handler.Handle(new() { Hero = HeroName, Points = 2 }, CancellationToken.None);
        await _handler.Handle(new() { Hero = HeroName, Points = 10 }, CancellationToken.None);
        await _handler.Handle(new() { Hero = HeroName, Points = 4 }, CancellationToken.None);

        // Act
        await _handler.Handle(new() { Hero = HeroName, Points = 8 }, CancellationToken.None);

        // Assert
        var updated = _repository.Get(HeroName);
        updated.CurrentPersistentHp.Should().Be(hero.HitPoints - 1);
        updated.TemporaryHp.Should().Be(10u);
    }
}
