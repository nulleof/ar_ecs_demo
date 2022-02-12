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

			if (this.playerInputControl.PlayerInput.PlayerEnableLook.phase == InputActionPhase.Performed) {

				look = this.playerInputControl.PlayerInput.PlayerLook.ReadValue<Vector2>();

			}

			var deltaTime = Time.DeltaTime;

			Entities
				.WithAll<PlayerTag>()
				.ForEach((Entity e, int nativeThreadIndex, ref Rotation rot, ref Velocity_AC velocity) => {

					if (brakeActive) {

						velocity.value = float3.zero;

					}
					else if (moveVector != Vector2.zero) {

						var moveProjectV = new float3(moveVector.x, 0, moveVector.y);

						velocity.value += (math.mul(rot.Value, moveProjectV).xyz) * settings.playerForce * deltaTime;

					}

					if (look.x != 0f || look.y != 0f) {

						var lookSpeedH = playerSettings.sensibilityHor;
						var lookSpeedV = playerSettings.sensibilityVert;

						Quaternion currQuaternion = rot.Value;
						float yaw = currQuaternion.eulerAngles.y;
						float pitch = currQuaternion.eulerAngles.x;

						yaw += lookSpeedV * look.x;
						pitch -= lookSpeedH * look.y;

						var newQuat = Quaternion.identity;
						newQuat.eulerAngles = new Vector3(pitch, yaw, 0);
						rot.Value = newQuat;

					}

				}).ScheduleParallel();

		}

		// private float ClampAngleX(float angleAroundX)

	}

}