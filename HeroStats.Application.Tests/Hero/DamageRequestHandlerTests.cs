using FluentAssertions;
using HeroStats.Application.Hero;
using HeroStats.Domain.Action.Damage;
using HeroStats.Infrastructure.DataAccess.Concrete;
using HeroStats.Infrastructure.Hero.Repository;
using NUnit.Framework;

namespace HeroStats.Application.Tests.Hero;

public class DamageRequestHandlerTests
{
    private const string HeroName = "nils";
    private HeroRepository _repository;
    private DamageRequestHandler _handler;

    [SetUp]
    public void Setup()
    {
        var hero = new Domain.Hero.Hero { Name = HeroName, HitPoints = 1000 };
        var db = new InMemoryDatabaseContext { Database = { [typeof(Domain.Hero.Hero)] = new List<object>() } };
        db.Database[typeof(Domain.Hero.Hero)].Add(hero);

        _repository = new HeroRepository(db);
        _handler = new DamageRequestHandler(_repository);
    }

    [Test]
    [TestCase(10u, 10u, 0u)]
    [TestCase(10u, 11u, 0u)]
    [TestCase(10u, 9u, 1u)]
    public async Task Handle_WithOnlyPersistentHp_DamageAppliedCorrectly(uint hp, uint damage, uint expectedHp)
    {
        // Arrange
        var hero = _repository.Get(HeroName);
        hero.SetCurrentPersistent(hp);

        // Act
        await _handler.Handle(new() { Hero = HeroName, Amount = damage }, CancellationToken.None);

        // Assert
        var updated = _repository.Get(HeroName);
        updated.CurrentPersistentHp.Should().Be(expectedHp);
        updated.TemporaryHp.Should().Be(0u);
    }

    [Test]
    [TestCase(10u, 10u, 0u)]
    [TestCase(10u, 11u, 0u)]
    [TestCase(10u, 9u, 1u)]
    public async Task Handle_WithOnlyTemporaryHp_DamageAppliedCorrectly(uint hp, uint damage, uint expectedHp)
    {
        // Arrange
        var hero = _repository.Get(HeroName);
        hero.TemporaryHp = hp;

        // Act
        await _handler.Handle(new() { Hero = HeroName, Amount = damage }, CancellationToken.None);

        // Assert
        var updated = _repository.Get(HeroName);
        updated.CurrentPersistentHp.Should().Be(0u);
        updated.TemporaryHp.Should().Be(expectedHp);
    }

    [Test]
    [TestCase(10u, 10u, 0u, 10u, 10u)]
    [TestCase(10u, 10u, 1u, 10u, 9u)]
    [TestCase(10u, 10u, 11u, 9u, 0u)]
    [TestCase(10u, 10u, 21u, 0u, 0u)]
    [TestCase(10u, 8u, 8u, 10u, 0u)]
    [TestCase(10u, 0u, 8u, 2u, 0u)]
    [TestCase(10u, 0u, 11u, 0u, 0u)]
    [TestCase(0u, 10u, 8u, 0u, 2u)]
    [TestCase(4u, 10u, 11u, 3u, 0u)]
    [TestCase(0u, 10u, 11u, 0u, 0u)]
    public async Task Handle_WithBothTemporaryAndPersistentHp_DamageAppliedCorrectly(uint persistentHp, uint tempHp,
        uint damage, uint expectedPersistedHp, uint expectedTempHp)
    {
        // Arrange
        var hero = _repository.Get(HeroName);
        hero.SetCurrentPersistent(persistentHp);
        hero.TemporaryHp = tempHp;

        // Act
        await _handler.Handle(new() { Hero = HeroName, Amount = damage }, CancellationToken.None);

        // Assert
        var updated = _repository.Get(HeroName);
        updated.CurrentPersistentHp.Should().Be(expectedPersistedHp);
        updated.TemporaryHp.Should().Be(expectedTempHp);
    }
    
    [Test]
    [TestCase(10u, 5u, DefenseType.Immunity, 500u, 10u, 5u)]
    [TestCase(10u, 5u, DefenseType.Resistance, 7u, 10u, 1u)]
    [TestCase(10u, 5u, DefenseType.Resistance, 13u, 8u, 0u)]
    [TestCase(0u, 8u, DefenseType.Immunity, 9u, 0u, 8u)]
    [TestCase(8u, 0u, DefenseType.Immunity, 9u, 8u, 0u)]
    public async Task Handle_WithDefense_DamageAppliedCorrectly(uint persistentHp, uint tempHp,
        DefenseType defense, uint damage, uint expectedPersistedHp, uint expectedTempHp)
    {
        // Arrange
        var hero = _repository.Get(HeroName);
        hero.SetCurrentPersistent(persistentHp);
        hero.TemporaryHp = tempHp;
        hero.Defenses[DamageType.Piercing] = defense;

        // Act
        await _handler.Handle(new() { Hero = HeroName, Amount = damage, Type = DamageType.Piercing }, CancellationToken.None);

        // Assert
        var updated = _repository.Get(HeroName);
        updated.CurrentPersistentHp.Should().Be(expectedPersistedHp);
        updated.TemporaryHp.Should().Be(expectedTempHp);
    }
}
