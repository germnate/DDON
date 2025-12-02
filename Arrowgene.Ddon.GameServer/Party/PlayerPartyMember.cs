using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.GameServer.Party;

public class PlayerPartyMember : PartyMember
{
    public PlayerPartyMember(GameClient client, DdonGameServer server)
    {
        Client = client;
        QuestState = new SoloQuestStateManager(this, server);
    }

    public GameClient Client { get; set; }

    public SoloQuestStateManager QuestState { get; set; }

    public override CDataPartyMember CDataPartyMember
    {
        get
        {
            var cdata = base.CDataPartyMember;
            cdata.CharacterListElement = Client.Character.CDataCharacterListElement;
            return cdata;
        }
    }

    public S2CContextGetPartyPlayerContextNtc GetPartyContext()
    {
        CDataPartyPlayerContext partyPlayerContext = new()
        {
            Base = Client.Character.CDataContextBase,
            PlayerInfo = Client.Character.CDataContextPlayerInfo,
            ResistInfo = Client.Character.CDataContextResist,
            EditInfo = Client.Character.EditInfo
        };

        S2CContextGetPartyPlayerContextNtc partyPlayerContextNtc = new()
        {
            CharacterId = Client.Character.CharacterId,
            Context = partyPlayerContext
        };

        partyPlayerContextNtc.Context.Base.MemberIndex = MemberIndex;
        return partyPlayerContextNtc;
    }
}
