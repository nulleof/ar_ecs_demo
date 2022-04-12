using ScriptsAndPrefabs.Mixed.Commands;
using ScriptsAndPrefabs.Player;
using Unity.Entities;
using Unity.NetCode;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScriptsAndPrefabs.Client.Systems {

	[UpdateInGroup(typeof(GhostInputSystemGroup))]
	public partial class InputPlayerSpawn_S : SystemBase {

		private BeginSimulationEntityCommandBufferSystem beginSimECB;
		private PlayerInputControl playerInputControl;

		protected override void OnCreate() {

			this.beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

			this.playerInputControl = new PlayerInputControl();
			this.playerInputControl.PlayerInput.Enable();

			RequireSingletonForUpdate<NetworkStreamInGame>();

		}

		protected override void OnUpdate() {

			var shouldSpawn = this.playerInputControl.PlayerInput.SpawnPlayer.phase == InputActionPhase.Performed;

			if (shouldSpawn == false) return;

			var commandBuffer = this.beginSimECB.CreateCommandBuffer().AsParallelWriter();

			Entities.WithAll<NetworkIdComponent>()
				.WithNone<NetworkStreamDisconnected>()
				.ForEach((Entity entity, int nativeThreadIndex, in CommandTargetComponent commandTargetComponent) => {

					if (commandTargetComponent.targetEntity == Entity.Null) {

						var req = commandBuffer.CreateEntity(nativeThreadIndex);
						commandBuffer.AddComponent<PlayerSpawnRequestRPC>(nativeThreadIndex, req);

						commandBuffer.AddComponent(nativeThreadIndex, req,
							new SendRpcCommandRequestComponent() {TargetConnection = entity});

					}

				}).Schedule();

			this.beginSimECB.AddJobHandleForProducer(Dependency);

		}

	}

}
