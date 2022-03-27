using Unity.NetCode;

namespace ScriptsAndPrefabs.Mixed.Commands {

	public struct SendClientGameRpc : IRpcCommand {

		public int levelWidth;
		public int levelHeight;
		public int levelDepth;
		public float playerForce;
		public float bulletVelocity;
		public float asteroidVelocity;
		public int numAsteroids;

	}

}
