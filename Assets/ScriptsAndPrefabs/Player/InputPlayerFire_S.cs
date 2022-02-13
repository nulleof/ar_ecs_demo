using ScriptsAndPrefabs.Player.Weapon;
using Unity.Entities;
using UnityEngine.InputSystem;

namespace ScriptsAndPrefabs.Player {

	public class InputPlayerFire_S : SystemBase {

		private PlayerInputControl playerInputControl;
		private BeginSimulationEntityCommandBufferSystem beginSimECB;

		protected override void OnCreate() {

			this.beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
			this.playerInputControl = new PlayerInputControl();
			this.playerInputControl.PlayerInput.Enable();

		}

		protected override void OnUpdate() {

			var fire = this.playerInputControl.PlayerInput.Fire.phase == InputActionPhase.Performed;
			var commandBuffer = this.beginSimECB.CreateCommandBuffer().AsParallelWriter();

			Entities
				.WithAll<PlayerTag>()
				.WithAll<PlayerWeaponHandler_AC>()
				.ForEach((Entity e, int nativeThreadIndex, in PlayerWeaponHandler_AC gunHandler) => {

					if (fire == false) return;

					var gunEntity = gunHandler.weapon;

					commandBuffer.AddComponent(nativeThreadIndex, gunEntity, new WeaponFire_C());

				}).Schedule();

			this.beginSimECB.AddJobHandleForProducer(Dependency);

		}

	}

}