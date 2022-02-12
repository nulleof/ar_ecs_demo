using System;
using Unity.Entities;

namespace ScriptsAndPrefabs.Player {

	[Serializable]
	[GenerateAuthoringComponent]
	public struct Player_AC : IComponentData {

		public Entity prefab;

	}

}