using System;
using Unity.Entities;

namespace ScriptsAndPrefabs.Mixed.Components {

	[Serializable]
	public struct GameSettings_C : IComponentData {

		public float asteroidVelocity;
		public float playerForce;
		public float bulletVelocity;
		public int numAsteroids;
		public int levelWidth;
		public int levelHeight;
		public int levelDepth;

	}

}