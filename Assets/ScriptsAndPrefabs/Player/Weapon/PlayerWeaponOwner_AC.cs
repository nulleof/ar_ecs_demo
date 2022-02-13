using Unity.Entities;

namespace ScriptsAndPrefabs.Player.Weapon {

	[GenerateAuthoringComponent]
	public struct PlayerWeaponOwner_AC : IComponentData {

		public Entity player;

	}

}