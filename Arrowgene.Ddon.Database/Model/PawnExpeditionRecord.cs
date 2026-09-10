using System;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Model
{
    public class PawnExpeditionRecord
    {
        public uint CharacterId { get; set; }
        public PawnExpeditionStatus Status { get; set; } = PawnExpeditionStatus.Tired;
        public uint AreaId { get; set; }
        public uint SpotId { get; set; }
        public bool IsHotSpot { get; set; }
        public bool IsGoldenSally { get; set; }
        public byte SallyCount { get; set; }
        public DateTime? SallyStartTime { get; set; }
    }
}
