using System.Diagnostics;
using ScriptsAndPrefabs.Mixed.Commands;
using ScriptsAndPrefabs.Mixed.Components;
using ScriptsAndPrefabs.Player;
using ScriptsAndPrefabs.Server.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace ScriptsAndPrefabs.Server.Systems {

	public struct PlayerSpawnInProgressTag : IComponentData { }

	[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
	public partial class PlayerSpawn_S : SystemBase {

		private BeginSimulationEntityCommandBufferSystem beginSimECB;
		private Entity playerEntity;
		private Random rand;

		protected override void OnCreate() {
			
			this.beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

			this.rand = new Random((uint) Stopwatch.GetTimestamp());
			
			RequireSingletonForUpdate<GameSettings_C>();
			
		}

		protected override void OnUpdate() {

			if (this.playerEntity == Entity.Null) {

				this.playerEntity = GetSingleton<Player_AC>().prefab;

			}

			var commandBuffer = this.beginSimECB.CreateCommandBuffer();
			var gameSettings = GetSingleton<GameSettings_C>();

			var playerStateFromEntity = GetComponentDataFromEntity<PlayerSpawningState_C>();
			var commandTargetFromEntity = GetComponentDataFromEntity<CommandTargetComponent>();
			var networkIdFromEntity = GetComponentDataFromEntity<NetworkIdComponent>();

			var rand = this.rand;
			var playerPrefab = this.playerEntity;

			Entities.ForEach((Entity entity,
				in PlayerSpawnRequestRPC request,
				in ReceiveRpcCommandRequestComponent requestSource) => {
				
				commandBuffer.DestroyEntity(entity);

				if (playerStateFromEntity.HasComponent(requestSource.SourceConnection) == false
				    || commandTargetFromEntity.HasComponent(requestSource.SourceConnection) == false
				    || commandTargetFromEntity[requestSource.SourceConnection].targetEntity != Entity.Null
				    || playerStateFromEntity[requestSource.SourceConnection].isSpawning != 0) {

					return;

				}

				var player = commandBuffer.Instantiate(playerPrefab);

				var width = gameSettings.levelWidth * 0.2f;
				var height = gameSettings.levelHeight * 0.2f;
				var depth = gameSettings.levelDepth * 0.2f;

				var pos = new Translation() {
					Value = new float3(
						rand.NextFloat(-width, width),
						rand.NextFloat(-height, height),
						rand.NextFloat(-depth, depth)
					),
				};

				var rot = new Rotation() {
					Value = Quaternion.identity,
				};
				
				commandBuffer.SetComponent(player, pos);
				commandBuffer.SetComponent(player, rot);
				
				commandBuffer.SetComponent(player, new GhostOwnerComponent() {
					NetworkId = networkIdFromEntity[requestSource.SourceConnection].Value,
				});
				commandBuffer.SetComponent(player, new PlayerEntity_C() {
					playerEntity = requestSource.SourceConnection,
				});
				
				commandBuffer.AddComponent(player, new PlayerSpawnInProgressTag());

				playerStateFromEntity[requestSource.SourceConnection] = new PlayerSpawningState_C() {isSpawning = 1};

			}).Schedule();
			
			this.beginSimECB.AddJobHandleForProducer(Dependency);

		}

	}

	[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
	[UpdateBefore(typeof(GhostSendSystem))]
	public partial class PlayerCompleteSpawn_S : SystemBase {

		private BeginSimulationEntityCommandBufferSystem beginSimECB;

		protected override void OnCreate() {

			this.beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

		}

		protected override void OnUpdate() {

			var commandBuffer = this.beginSimECB.CreateCommandBuffer();
			
			var playerStateFromEntity = GetComponentDataFromEntity<PlayerSpawningState_C>();
			var commandTargetFromEntity = GetComponentDataFromEntity<CommandTargetComponent>();
			var connectionFromEntity = GetComponentDataFromEntity<NetworkStreamConnection>();

			Entities.WithAll<PlayerSpawnInProgressTag>()
				.ForEach((Entity entity, in PlayerEntity_C player) => {
					
					// Ensure there was no disconnect
					if (playerStateFromEntity.HasComponent(player.playerEntity) == false
					    || connectionFromEntity[player.playerEntity].Value.IsCreated == false) {
						
						commandBuffer.DestroyEntity(entity);
						return;

					}
					
					commandBuffer.RemoveComponent<PlayerSpawnInProgressTag>(entity);

					commandTargetFromEntity[player.playerEntity] = new CommandTargetComponent() {targetEntity = entity};
					playerStateFromEntity[player.playerEntity] = new PlayerSpawningState_C() {isSpawning = 0};

				}).Schedule();
			
			this.beginSimECB.AddJobHandleForProducer(Dependency);

		}

	}

}
