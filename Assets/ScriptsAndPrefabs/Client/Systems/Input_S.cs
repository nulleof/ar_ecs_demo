using ScriptsAndPrefabs.Mixed.Commands;
using ScriptsAndPrefabs.Player;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScriptsAndPrefabs.Client.Systems {

	[UpdateInGroup(typeof(GhostInputSystemGroup))]
	public partial class Input_S : SystemBase {

		private BeginSimulationEntityCommandBufferSystem beginSimECB;
		private ClientSimulationSystemGroup clientSimG;

		private PlayerInputControl playerInputControl;

		protected override void OnCreate() {

			this.beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
			this.clientSimG = World.GetOrCreateSystem<ClientSimulationSystemGroup>();

			this.playerInputControl = new PlayerInputControl();
			
			RequireSingletonForUpdate<NetworkStreamInGame>();
			RequireSingletonForUpdate<PlayerSettings_AC>();

		}

		protected override void OnUpdate() {

			bool isThinClient = HasSingleton<ThinClientComponent>();
			var playerSettings = GetSingleton<PlayerSettings_AC>();

			if (HasSingleton<CommandTargetComponent>() &&
			    GetSingleton<CommandTargetComponent>().targetEntity == Entity.Null) {

				if (isThinClient == true) {
					
					// No ghost are spawned. So create placeholder struct to store commands
					var ent = EntityManager.CreateEntity();
					EntityManager.AddBuffer<PlayerCommand>(ent);
					SetSingleton(new CommandTargetComponent() { targetEntity = ent });

				}

			}
			
			Vector2 movement = Vector2.zero;
			bool selfDestruct = false;
			bool shoot = false;
			bool brake = false;
			bool spawn = false;
			Vector2 mouse = Vector2.zero;

			if (isThinClient == false) {

				movement = this.playerInputControl.PlayerInput.PlayerMovement.ReadValue<Vector2>();
				selfDestruct = this.playerInputControl.PlayerInput.DespawnPlayer.phase == InputActionPhase.Performed;
				shoot = this.playerInputControl.PlayerInput.Fire.phase == InputActionPhase.Performed;
				brake = this.playerInputControl.PlayerInput.StopFlying.phase == InputActionPhase.Performed;
				spawn = this.playerInputControl.PlayerInput.SpawnPlayer.phase == InputActionPhase.Performed;

				var shouldLook = playerSettings.useRightClickLook == false
								 || (playerSettings.useRightClickLook == true
									 && this.playerInputControl.PlayerInput
										 .PlayerEnableLook.phase ==
									 InputActionPhase.Performed
								 );

				if (shouldLook == true) {

					mouse = this.playerInputControl.PlayerInput.PlayerLook.ReadValue<Vector2>();

				}

			}
			else { // Generate some random input for thin client

				var secondsElapsed = Time.ElapsedTime;

				if (secondsElapsed % 10 < 1) {

					spawn = true;

				}

				switch (secondsElapsed % 4) {
					
					case var n when n < 1:
						movement = Vector2.left;
						break;
					
					case var n when n < 2:
						movement = Vector2.up;
						break;
					
					case var n when n < 3:
						movement =  Vector2.right;
						break;
					
					case var n when n < 4:
						movement = Vector2.down;
						break;
					
					default:
						shoot = true;

						break;
					
				}

			}

			var commandBuffer = this.beginSimECB.CreateCommandBuffer().AsParallelWriter();
			var inputFromEntity = GetBufferFromEntity<PlayerCommand>();
			var inputTargetTick = this.clientSimG.ServerTick;

			Entities
				.WithAll<NetworkIdComponent>()
				.WithNone<NetworkStreamDisconnected>()
				.ForEach((Entity entity, int nativeThreadIndex, in CommandTargetComponent commandTarget) => {

					if (isThinClient == true && spawn == true) {

						var ent = commandBuffer.CreateEntity(nativeThreadIndex);
						commandBuffer.AddComponent<PlayerSpawnRequestRPC>(nativeThreadIndex, ent);
						commandBuffer.AddComponent(nativeThreadIndex, ent, new SendRpcCommandRequestComponent() {
							TargetConnection = entity,
						});

					}

					if (commandTarget.targetEntity == Entity.Null) {

						if (shoot == true) {
							
							var ent = commandBuffer.CreateEntity(nativeThreadIndex);
							commandBuffer.AddComponent<PlayerSpawnRequestRPC>(nativeThreadIndex, ent);
							commandBuffer.AddComponent(nativeThreadIndex, ent, new SendRpcCommandRequestComponent() {
								TargetConnection = entity,
							});
							
						}
						
					}
					else {

						if (inputFromEntity.HasComponent(commandTarget.targetEntity)) {

							var input = inputFromEntity[commandTarget.targetEntity];
							
							input.AddCommandData(new PlayerCommand() {
								brake = brake,
								mouse = mouse,
								movement = movement,
								selfDestruct = selfDestruct,
								shoot = shoot,
								Tick = inputTargetTick,
							});

						}
						
					}
					
				}).Schedule();
			
			this.beginSimECB.AddJobHandleForProducer(Dependency);

		}

	}

}
