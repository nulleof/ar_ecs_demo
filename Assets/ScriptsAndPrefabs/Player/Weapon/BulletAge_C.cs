using Unity.Entities;

namespace ScriptsAndPrefabs.Player.Weapon {

	[GenerateAuthoringComponent]
	public struct BulletAge_C : IComponentData {

		public float age;
		public float maxAge;

	}

}