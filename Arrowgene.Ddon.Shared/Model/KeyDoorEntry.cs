using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model
{
    /// <summary>
    /// Describes a "key enemy" door: when every required enemy of the trigger enemy group is
    /// killed, the server pushes the listed object-manager (OM) states to the party so the door
    /// opens. Vanilla relies on the client mapping an enemy group to a door internally; this
    /// asset lets the server drive the same OM state explicitly for stages where that mapping is
    /// unavailable or where the key enemy group has to be re-created server side.
    /// See KeyDoors.json.
    /// </summary>
    public class KeyDoorEntry
    {
        /// <summary>Free-form note describing the door. Ignored by the server.</summary>
        public string Comment { get; set; }

        /// <summary>Stage the trigger enemy group lives in.</summary>
        public uint StageId { get; set; }

        /// <summary>Layer of the trigger enemy group.</summary>
        public uint LayerNo { get; set; }

        /// <summary>Enemy group whose destruction opens the door.</summary>
        public uint GroupId { get; set; }

        /// <summary>OM states pushed to the party once the trigger group is destroyed.</summary>
        public List<KeyDoorOmState> Opens { get; set; } = new List<KeyDoorOmState>();
    }

    /// <summary>
    /// A single object-manager state change applied when a <see cref="KeyDoorEntry"/> triggers.
    /// The values correspond to the arguments accepted by the `/updateom` chat command.
    /// </summary>
    public class KeyDoorOmState
    {
        /// <summary>Stage the OM lives in. Defaults to the trigger group's stage when zero.</summary>
        public uint StageId { get; set; }

        /// <summary>Layout group id of the OM (the door object), not the enemy group id.</summary>
        public uint GroupId { get; set; }

        /// <summary>Position id of the OM within its layout group.</summary>
        public uint PosId { get; set; }

        /// <summary>Raw <c>SeasonDungeonOmState</c> value. 8 (DoorUnlocked) opens a door.</summary>
        public byte State { get; set; } = 8;
    }
}
