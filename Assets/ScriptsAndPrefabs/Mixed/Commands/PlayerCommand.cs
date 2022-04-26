using Unity.NetCode;
using UnityEngine;

namespace ScriptsAndPrefabs.Mixed.Commands {

	[GhostComponent(PrefabType = GhostPrefabType.AllPredicted)]
	public struct PlayerCommand : ICommandData {

		public uint Tick { get; set; }

		public Vector2 movement;
		public bool shoot;
		public bool selfDestruct;
		public Vector2 mouse;
		public bool brake;

	}

}
