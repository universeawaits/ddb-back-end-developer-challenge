using FluentAssertions;
using HeroStats.Application.Hero;
using HeroStats.Infrastructure.DataAccess.Concrete;
using HeroStats.Infrastructure.Hero.Repository;
using NUnit.Framework;

namespace HeroStats.Application.Tests.Hero;

public class HealRequestHandlerTests
{
    private const string HeroName = "nils";
    private HeroRepository _repository;
    private HealRequestHandler _handler;

    [SetUp]
    public void Setup()
    {
        var hero = new Domain.Hero.Hero { Name = HeroName, HitPoints = 1000 };
        var db = new InMemoryDatabaseContext { Database = { [typeof(Domain.Hero.Hero)] = new List<object>() } };
        db.Database[typeof(Domain.Hero.Hero)].Add(hero);

        _repository = new HeroRepository(db);
        _handler = new HealRequestHandler(_repository);
    }

    [Test]
    public async Task Handle_ExceedsMaxHp_DoenstAccumulateAboveMaximum()
    {
        // Arrange
        var hero = _repository.Get(HeroName);
        hero.SetCurrentPersistent(hero.HitPoints - 1);

        // Act
        await _handler.Handle(new() { Hero = HeroName, Points = 2 }, CancellationToken.None);

        // Assert
        var updated = _repository.Get(HeroName);
        updated.CurrentPersistentHp.Should().Be(hero.HitPoints);
        updated.TemporaryHp.Should().Be(0u);
    }
}
