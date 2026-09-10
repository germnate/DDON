using System.Collections.Generic;

namespace Arrowgene.Ddon.Database.Model
{
    public class PawnExpeditionRewardBoxItem
    {
        public uint BoxId { get; set; }
        public uint SlotNo { get; set; }
        public uint ItemId { get; set; }
        public uint ItemNum { get; set; }
        public uint Quality { get; set; }
        public bool IsHidden { get; set; }
        public bool Claimed { get; set; }
    }

    public class PawnExpeditionRewardBox
    {
        public uint BoxId { get; set; }
        public uint CharacterId { get; set; }
        public byte MdlType { get; set; }
        public bool Claimed { get; set; }
        public List<PawnExpeditionRewardBoxItem> Items { get; set; } = new();
    }
}
