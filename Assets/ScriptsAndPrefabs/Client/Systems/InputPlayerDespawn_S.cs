using ScriptsAndPrefabs.Player;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine.InputSystem;

namespace ScriptsAndPrefabs.Client.Systems {

	[UpdateInGroup(typeof(UpdatePresentationSystemGroup))]
	public partial class InputPlayerDespawn_S : SystemBase {

		private EndSimulationEntityCommandBufferSystem endSimECB;
		private PlayerInputControl playerInputControl;

		protected override void OnCreate() {

			this.endSimECB = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
			this.playerInputControl = new PlayerInputControl();
			this.playerInputControl.PlayerInput.Enable();

			var query = EntityManager.CreateEntityQuery(ComponentType.ReadOnly<PlayerTag>());
			RequireForUpdate(query);

		}

		protected override void OnUpdate() {

			var shouldDespawn = this.playerInputControl.PlayerInput.DespawnPlayer.phase == InputActionPhase.Performed;

			if (shouldDespawn == false) return;

			var commandBuffer = this.endSimECB.CreateCommandBuffer().AsParallelWriter();

			Entities
				.WithAll<PlayerTag>()
				.ForEach((Entity e, int nativeThreadIndex) => { commandBuffer.DestroyEntity(nativeThreadIndex, e); })
				.ScheduleParallel();

			this.endSimECB.AddJobHandleForProducer(Dependency);

		}

	}

}