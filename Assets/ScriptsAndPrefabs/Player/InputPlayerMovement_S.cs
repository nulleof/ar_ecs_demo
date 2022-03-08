using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScriptsAndPrefabs.Player {

	public class InputPlayerMovement_S : SystemBase {

		private PlayerInputControl playerInputControl;

		protected override void OnCreate() {

			this.playerInputControl = new PlayerInputControl();
			this.playerInputControl.PlayerInput.Enable();

			RequireSingletonForUpdate<GameSettings_C>();
			RequireSingletonForUpdate<PlayerSettings_AC>();

		}


		protected override void OnUpdate() {

			var settings = GetSingleton<GameSettings_C>();
			var playerSettings = GetSingleton<PlayerSettings_AC>();

			var moveVector = this.playerInputControl.PlayerInput.PlayerMovement.ReadValue<Vector2>();
			var brakeActive = this.playerInputControl.PlayerInput.StopFlying.phase == InputActionPhase.Performed;

			var look = float2.zero;

			var shouldLook = 
				playerSettings.useRightClickLook == false ||
				(playerSettings.useRightClickLook == true &&
				 this.playerInputControl.PlayerInput.PlayerEnableLook.phase == InputActionPhase.Performed);
			
			if (shouldLook == true) {

				look = this.playerInputControl.PlayerInput.PlayerLook.ReadValue<Vector2>();

			}

			var deltaTime = Time.DeltaTime;

			Entities
				.WithAll<PlayerTag>()
				.ForEach((Entity e, int nativeThreadIndex, ref Velocity_AC velocity, ref Rotation rot, in LocalToWorld localToWorld) => {

					if (brakeActive) {

						velocity.value = float3.zero;

					}
					else if (moveVector != Vector2.zero) {

						var moveProjectV = new float3(moveVector.x, 0, moveVector.y);

						velocity.value += (math.mul(localToWorld.Rotation, moveProjectV).xyz) * settings.playerForce * deltaTime;

					}

					if (look.x != 0f || look.y != 0f) {

						var lookH = playerSettings.sensibilityHor * look.x;
						var lookV = playerSettings.sensibilityVert * look.y;

						var forward = math.normalize(localToWorld.Forward);
						var right = math.normalize(localToWorld.Right);
						var up = math.normalize(localToWorld.Up);

						var targetV = forward + (right * lookH) + (up * lookV);

						rot.Value = quaternion.LookRotation(targetV, up);

					}

				}).ScheduleParallel();

		}

		// private float ClampAngleX(float angleAroundX)

	}

}