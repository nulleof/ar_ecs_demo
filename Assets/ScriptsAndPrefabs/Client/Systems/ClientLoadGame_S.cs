using ScriptsAndPrefabs.Mixed.Commands;
using ScriptsAndPrefabs.Mixed.Components;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace ScriptsAndPrefabs.Client.Systems {

	[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
	[UpdateBefore(typeof(RpcSystem))]
	public partial class ClientLoadGame_S : SystemBase {

		private BeginSimulationEntityCommandBufferSystem beginSimEcb;

		protected override void OnCreate() {

			this.beginSimEcb = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

			RequireForUpdate(GetEntityQuery(
				ComponentType.ReadOnly<SendClientGameRpc>(),
				ComponentType.ReadOnly<ReceiveRpcCommandRequestComponent>()
			));

			RequireSingletonForUpdate<GameSettings_C>();

		}


		protected override void OnUpdate() {

			var commandBuffer = this.beginSimEcb.CreateCommandBuffer();
			var rpcFromEntity = GetBufferFromEntity<OutgoingRpcDataStreamBufferComponent>();
			var gameSettingsE = GetSingletonEntity<GameSettings_C>();
			var gameSettingsData = GetComponentDataFromEntity<GameSettings_C>();

			Entities.ForEach((Entity entity,
				in SendClientGameRpc request,
				in ReceiveRpcCommandRequestComponent requestSource) => {

				commandBuffer.DestroyEntity(entity);

				if (rpcFromEntity.HasComponent(requestSource.SourceConnection) == false) return;

				gameSettingsData[gameSettingsE] = new GameSettings_C() {

					levelWidth = request.levelWidth,
					levelHeight = request.levelHeight,
					levelDepth = request.levelDepth,
					playerForce = request.playerForce,
					bulletVelocity = request.bulletVelocity,
					asteroidVelocity = request.asteroidVelocity,
					numAsteroids = request.numAsteroids,

				};

				commandBuffer.AddComponent(requestSource.SourceConnection, default(NetworkStreamInGame));

				var levelReq = commandBuffer.CreateEntity();
				commandBuffer.AddComponent(levelReq, new SendServerGameLoadedRpc());
				commandBuffer.AddComponent(levelReq,
					new SendRpcCommandRequestComponent() {TargetConnection = requestSource.SourceConnection});

				Debug.Log("Client loaded game");

			}).Schedule();

			this.beginSimEcb.AddJobHandleForProducer(Dependency);

		}

	}

}
