using NUnit.Framework;

public class TroopStateTests
{
    [Test]
    public void TroopFaction_WhenCreatedAsPlayer_ShouldBePlayerFaction()
    {
        Faction expectedFaction = Faction.Allies;
        Faction actualFaction = Faction.Allies;

        Assert.That(actualFaction, Is.EqualTo(expectedFaction));
    }
}