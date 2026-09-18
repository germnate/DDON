using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Shared.Model;

/// <summary>
/// Defines the quest gate for a server-owned support pawn.
/// </summary>
public sealed class ServerSupportPawnDefinition
{
    /// <summary>
    /// The pawn identifier.
    /// </summary>
    public uint PawnId { get; init; }

    /// <summary>
    /// The quest that must be completed before the pawn is available.
    /// </summary>
    public QuestId RequiredQuestId { get; init; }

    /// <summary>
    /// Whether the pawn is available when its quest requirement is satisfied.
    /// </summary>
    public bool Enabled { get; init; }
}
