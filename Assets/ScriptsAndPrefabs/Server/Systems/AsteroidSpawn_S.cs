using System.Diagnostics;
using ScriptsAndPrefabs.AsteroidField;
using ScriptsAndPrefabs.Mixed.Components;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Physics;
using Unity.Transforms;

namespace ScriptsAndPrefabs.Server.Systems {

	[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
	public partial class AsteroidSpawn_S : SystemBase {

		private EntityQuery asteroidQuery;
		private BeginSimulationEntityCommandBufferSystem beginSimECB;
		private EntityQuery gameSettingsQuery;
		private Entity asteroidPrefab;

		private EntityQuery connectionGroup;

		protected override void OnCreate() {

			this.asteroidQuery = GetEntityQuery(ComponentType.ReadWrite<AsteroidTag>());
			this.beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
			this.gameSettingsQuery = GetEntityQuery(ComponentType.ReadOnly<GameSettings_C>());

			RequireForUpdate(gameSettingsQuery);

			// Don't spawn asteroids if there is no clients to save cpu
			this.connectionGroup = GetEntityQuery(ComponentType.ReadWrite<NetworkStreamConnection>());

		}

		protected override void OnUpdate() {

			if (this.connectionGroup.IsEmptyIgnoreFilter == true) {
				
				// there is no connected clients. Destroy all asteroids
				EntityManager.DestroyEntity(this.asteroidQuery);
				return;

			}

			if (this.asteroidPrefab == Entity.Null) {

				this.asteroidPrefab = GetSingleton<Asteroid_AC>().prefab;

			}

			var settings = GetSingleton<GameSettings_C>();
			var commandBuffer = beginSimECB.CreateCommandBuffer();
			var count = this.asteroidQuery.CalculateEntityCountWithoutFiltering();
			var rand = new Random((uint) Stopwatch.GetTimestamp());

			SpawnJob job = new SpawnJob {
				commandBuffer = commandBuffer,
				count = count,
				settings = settings,
				prefab = this.asteroidPrefab,
				rand = rand,
			};


			this.beginSimECB.AddJobHandleForProducer(job.Schedule());

		}

	}

	struct SpawnJob : IJob {

		public EntityCommandBuffer commandBuffer;
		public int count;
		public Random rand;
		public GameSettings_C settings;
		public Entity prefab;

		public void Execute() {

			for (int i = count; i < this.settings.numAsteroids; ++i) {

				var padding = 0.1f;

				var boundPosX = SpawnJob.BoundaryPosition(this.settings.levelWidth, padding);
				var boundPosY = SpawnJob.BoundaryPosition(this.settings.levelHeight, padding);
				var boundPosZ = SpawnJob.BoundaryPosition(this.settings.levelDepth, padding);

				var xPos = this.rand.NextFloat(-boundPosX, boundPosX);
				var yPos = this.rand.NextFloat(-boundPosY, boundPosY);
				var zPos = this.rand.NextFloat(-boundPosZ, boundPosZ);

				var chooseFace = this.rand.NextInt(0, 6);

				switch (chooseFace) {

					case 0:
						xPos = -boundPosX;
						break;
					case 1:
						xPos = boundPosX;
						break;
					case 2:
						yPos = -boundPosX;
						break;
					case 3:
						yPos = boundPosY;
						break;
					case 4:
						zPos = -boundPosX;
						break;
					case 5:
						zPos = boundPosZ;
						break;

				}

				var pos = new Translation {
					Value = new float3(xPos, yPos, zPos),
				};

				var e = commandBuffer.Instantiate(this.prefab);
				commandBuffer.SetComponent(e, pos);

				var randomVelocity = SpawnJob.RandomVelocity(ref rand, settings.asteroidVelocity);
				var vel = new PhysicsVelocity() {
					Linear = randomVelocity,
				};

				commandBuffer.SetComponent(e, vel);

			}

		}

		private static float BoundaryPosition(float length, float boundPadding) {

			return length / 2 - boundPadding;

		}

		private static float3 RandomVelocity(ref Random rand, float cap) {

			var randX = SpawnJob.RandomSymmetricalRange(ref rand, cap);
			var randY = SpawnJob.RandomSymmetricalRange(ref rand, cap);
			var randZ = SpawnJob.RandomSymmetricalRange(ref rand, cap);

			return new float3(randX, randY, randZ);

		}

		private static float RandomSymmetricalRange(ref Random rand, float cap) {

			return rand.NextFloat(-cap, cap);

		}

	}

}