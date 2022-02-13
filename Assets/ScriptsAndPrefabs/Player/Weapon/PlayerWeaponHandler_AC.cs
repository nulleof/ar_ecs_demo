using Unity.Entities;

namespace ScriptsAndPrefabs.Player.Weapon {

	[GenerateAuthoringComponent]
	public struct PlayerWeaponHandler_AC : IComponentData {

		public Entity weapon;

	}

}