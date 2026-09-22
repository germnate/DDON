using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Model;
using Xunit;

namespace Arrowgene.Ddon.Test.GameServer.Characters;

public class PawnExpeditionManagerTest
{
    [Fact]
    public void DefaultRecord_ShouldNotBePersisted()
    {
        PawnExpeditionRecord record = PawnExpeditionManager.CreateDefaultRecord(42);

        Assert.Equal(42u, record.CharacterId);
        Assert.Equal(0u, record.PawnId);
        Assert.Equal(PawnExpeditionStatus.Tired, record.Status);
        Assert.Equal(1, record.SallyCount);
        Assert.False(PawnExpeditionManager.CanPersistRecord(record));
    }
}
