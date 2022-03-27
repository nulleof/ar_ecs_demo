using System;
using Unity.Entities;

namespace ScriptsAndPrefabs.Player.Weapon {

	[Serializable]
	[GenerateAuthoringComponent]
	public struct Bullet_AC : IComponentData {

		public Entity prefab;

	}

}
