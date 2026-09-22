using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Shared.Model;
using Xunit;

namespace Arrowgene.Ddon.Test.GameServer.Party
{
    public class PartyGroupTest
    {
        [Fact]
        public void TestCanPawnJoinParty_RejectsExpeditionState()
        {
            Assert.False(PartyGroup.CanPawnJoinParty(new Pawn { PawnState = PawnState.ExpeditionSally }));
            Assert.False(PartyGroup.CanPawnJoinParty(new Pawn { PawnState = PawnState.ExpeditionReturn }));
            Assert.True(PartyGroup.CanPawnJoinParty(new Pawn { PawnState = PawnState.None }));
        }
    }
}
