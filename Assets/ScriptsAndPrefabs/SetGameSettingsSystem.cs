using Unity.Entities;
using UnityEngine;

namespace ScriptsAndPrefabs {

	[DisallowMultipleComponent]
	public class SetGameSettingsSystem : MonoBehaviour, IConvertGameObjectToEntity {

		public float asteroidVelocity = 10f;
		public float playerForce = 50f;
		public float bulletVelocity = 500f;

		public int numAsteroids = 200;
		public int levelWidth = 2048;
		public int levelHeight = 2048;
		public int levelDepth = 2048;


		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

			var settings = default(GameSettingsComponent);

			settings.asteroidVelocity = this.asteroidVelocity;
			settings.playerForce = this.playerForce;
			settings.bulletVelocity = this.bulletVelocity;

			settings.numAsteroids = this.numAsteroids;
			settings.levelWidth = this.levelWidth;
			settings.levelHeight = this.levelHeight;
			settings.levelDepth = this.levelDepth;

			dstManager.AddComponentData(entity, settings);

		}
	}

}
