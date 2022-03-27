using ScriptsAndPrefabs.Mixed.Components;
using Unity.Entities;
using UnityEngine;

namespace ScriptsAndPrefabs {

	[DisallowMultipleComponent]
	public class SetGameSettings_S : MonoBehaviour, IConvertGameObjectToEntity {

		public float asteroidVelocity = 10f;
		public float playerForce = 50f;
		public float bulletVelocity = 500f;

		public int numAsteroids = 200;
		public int levelWidth = 2048;
		public int levelHeight = 2048;
		public int levelDepth = 2048;

		[Header("Debug")]
		public bool drawRaysMuzzles = false;

		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

			var settings = default(GameSettings_C);

			settings.asteroidVelocity = this.asteroidVelocity;
			settings.playerForce = this.playerForce;
			settings.bulletVelocity = this.bulletVelocity;

			settings.numAsteroids = this.numAsteroids;
			settings.levelWidth = this.levelWidth;
			settings.levelHeight = this.levelHeight;
			settings.levelDepth = this.levelDepth;

			dstManager.AddComponentData(entity, settings);

			var debugSettings = default(DebugSettings_C);
			debugSettings.ShowGunMuzzleRays = this.drawRaysMuzzles;

			dstManager.AddComponentData(entity, debugSettings);

		}

	}

}
