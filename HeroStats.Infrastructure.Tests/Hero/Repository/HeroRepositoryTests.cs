using FluentAssertions;
using HeroStats.Domain.Hero.DataAccess;
using HeroStats.Infrastructure.DataAccess.Concrete;
using HeroStats.Infrastructure.Hero.Repository;
using NUnit.Framework;

namespace HeroStats.Infrastructure.Tests.Hero.Repository;

public class HeroRepositoryTests
{
    [Test]
    public void Get_DoesntExist_ExceptionThrown()
    {
        // Arrange
        var hero = new Domain.Hero.Hero { Name = "nils" };
        var db = new InMemoryDatabaseContext { Database = { [typeof(Domain.Hero.Hero)] = new List<object>() } };
        db.Database[typeof(Domain.Hero.Hero)].Add(hero);

        var repo = new HeroRepository(db);

        // Act
        var get = () => repo.Get("keppel");

        // Assert
        get.Should().Throw<HeroNotFoundException>().WithMessage("Hero keppel doesn't exist");
    }
    
    [Test]
    public void Update_ForAnExistingHero_Applied()
    {
        // Arrange
        var hero = new Domain.Hero.Hero { Name = "nils", HitPoints = 10 };
        var db = new InMemoryDatabaseContext { Database = { [typeof(Domain.Hero.Hero)] = new List<object>() } };
        db.Database[typeof(Domain.Hero.Hero)].Add(hero);

        const int newHp = 8;
        var repo = new HeroRepository(db);
        hero.SetCurrentPersistent(newHp);

        // Act
        repo.Update(hero);

        // Assert
        var updated = repo.Get("nils");
        updated.CurrentPersistentHp.Should().Be(newHp);
    }
}
