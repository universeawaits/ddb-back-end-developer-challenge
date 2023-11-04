using FluentAssertions;
using NUnit.Framework;

namespace HeroStats.Domain.Tests.Hero;

public class HeroTests
{
    [Test]
    [TestCase(10u, 0u, 10u, 10u)]
    [TestCase(10u, 0u, 4u, 4u)]
    [TestCase(10u, 0u, 11u, 10u)]
    [TestCase(10u, 10u, 0u, 0u)]
    [TestCase(10u, 10u, 4u, 4u)]
    [TestCase(10u, 10u, 11u, 10u)]
    [TestCase(10u, 4u, 5u, 5u)]
    [TestCase(10u, 4u, 0u, 0u)]
    [TestCase(10u, 4u, 11u, 10u)]
    public void SetCurrentPersistent_GivenInitialMaxAndCurrent_ChangesAccordingly(uint max, uint current, uint called, uint expected)
    {
        // Arrange
        var hero = new Domain.Hero.Hero { HitPoints = max };
        hero.SetCurrentPersistent(current);

        // Act
        hero.SetCurrentPersistent(called);

        // Assert
        hero.CurrentPersistentHp.Should().Be(expected);
    }
}
