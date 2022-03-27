using System;
using Unity.Entities;

namespace ScriptsAndPrefabs.Server.Components {

	[Serializable]
	public struct PlayerSpawningState_C : IComponentData {

		public int isSpawning;

	}

}
