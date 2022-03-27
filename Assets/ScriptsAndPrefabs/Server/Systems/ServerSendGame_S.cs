using ScriptsAndPrefabs.Mixed.Commands;
using ScriptsAndPrefabs.Mixed.Components;
using Unity.Entities;
using Unity.NetCode;

namespace ScriptsAndPrefabs.Server.Systems {

	public struct SentClientGameRpcTag : IComponentData { }

	[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
	[UpdateBefore(typeof(RpcSystem))]
	public partial class ServerSendGame_S : SystemBase {

		private BeginSimulationEntityCommandBufferSystem barrier;

		protected override void OnCreate() {

			this.barrier = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
			RequireSingletonForUpdate<GameSettings_C>();

		}

		protected override void OnUpdate() {

			var commandBuffer = this.barrier.CreateCommandBuffer();

			var serverData = GetSingleton<GameSettings_C>();

			Entities.WithNone<SentClientGameRpcTag>()
				.ForEach((Entity entity, in NetworkIdComponent netId) => {

					commandBuffer.AddComponent(entity, new SentClientGameRpcTag());
					var req = commandBuffer.CreateEntity();
					commandBuffer.AddComponent(req, new SendClientGameRpc() {
						levelWidth = serverData.levelWidth,
						levelHeight = serverData.levelHeight,
						levelDepth = serverData.levelDepth,
						bulletVelocity = serverData.bulletVelocity,
						playerForce = serverData.playerForce,
						asteroidVelocity = serverData.asteroidVelocity,
						numAsteroids = serverData.numAsteroids,
					});

					commandBuffer.AddComponent(req, new SendRpcCommandRequestComponent() {
						TargetConnection = entity,
					});

				}).Schedule();

			this.barrier.AddJobHandleForProducer(Dependency);

		}

	}

}
