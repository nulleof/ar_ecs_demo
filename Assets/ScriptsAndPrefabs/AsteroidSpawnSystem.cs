using System.Diagnostics;
using System.Numerics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace ScriptsAndPrefabs {

	public class AsteroidSpawnSystem : SystemBase {

		private EntityQuery asteroidQuery;
		private BeginSimulationEntityCommandBufferSystem beginSimECB;
		private EntityQuery gameSettingsQuery;
		private Entity asteroidPrefab;

		protected override void OnCreate() {

			this.asteroidQuery = GetEntityQuery(ComponentType.ReadWrite<AsteroidTag>());
			this.beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
			this.gameSettingsQuery = GetEntityQuery(ComponentType.ReadOnly<GameSettingsComponent>());
		
			RequireForUpdate(gameSettingsQuery);

		}

		protected override void OnUpdate() {

			if (this.asteroidPrefab == Entity.Null) {

				this.asteroidPrefab = GetSingleton<AsteroidAuthoringComponent>().prefab;

				// ECS funny business
				return;

			}

			var settings = GetSingleton<GameSettingsComponent>();
			var commandBuffer = beginSimECB.CreateCommandBuffer();
			var count = this.asteroidQuery.CalculateEntityCountWithoutFiltering();
			// ECS funny business
			var asteroidPrefab = this.asteroidPrefab;
			var rand = new Random((uint) Stopwatch.GetTimestamp());

			SpawnJob job = new SpawnJob {
				commandBuffer = commandBuffer,
				count = count,
				settings = settings,
				prefab = asteroidPrefab,
				rand = rand,
			};

		
			this.beginSimECB.AddJobHandleForProducer(job.Schedule());

		}

	}

	struct SpawnJob : IJob {

		public EntityCommandBuffer commandBuffer;
		public int count;
		public Random rand;
		public GameSettingsComponent settings;
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
				var vel = new VelocityComponent {
					value = randomVelocity,
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