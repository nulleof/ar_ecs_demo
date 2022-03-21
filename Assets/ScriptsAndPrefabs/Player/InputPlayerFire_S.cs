using ScriptsAndPrefabs.Player.Weapon;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScriptsAndPrefabs.Player {

	public partial class InputPlayerFire_S : SystemBase {

		private PlayerInputControl playerInputControl;
		private BeginSimulationEntityCommandBufferSystem beginSimECB;

		protected override void OnCreate() {

			this.beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
			this.playerInputControl = new PlayerInputControl();
			this.playerInputControl.PlayerInput.Enable();

		}

		protected override void OnUpdate() {

			var fire = this.playerInputControl.PlayerInput.Fire.phase == InputActionPhase.Performed;
			
			if (fire == false) return;

			var commandBuffer = this.beginSimECB.CreateCommandBuffer().AsParallelWriter();

			Entities
				.WithAll<PlayerTag>()
				.ForEach((Entity e, int nativeThreadIndex, in PlayerWeaponHandler_AC gunHandler) => {

					var gunEntity = gunHandler.weapon;

					commandBuffer.AddComponent(nativeThreadIndex, gunEntity, new WeaponFire_C());

				}).ScheduleParallel();

			this.beginSimECB.AddJobHandleForProducer(Dependency);

		}

	}

}