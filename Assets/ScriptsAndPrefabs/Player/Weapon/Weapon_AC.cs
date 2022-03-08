using System;
using Unity.Entities;

namespace ScriptsAndPrefabs.Player.Weapon {

	[GenerateAuthoringComponent]
	[Serializable]
	public struct Weapon_AC : IComponentData {

		public float cooldownLeft;
		public Entity muzzle;

	}

}