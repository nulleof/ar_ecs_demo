using ScriptsAndPrefabs.Mixed.Commands;
using ScriptsAndPrefabs.Mixed.Components;
using ScriptsAndPrefabs.Player;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace ScriptsAndPrefabs.Mixed.Systems {

	[UpdateInWorld(TargetWorld.ClientAndServer)]
	[UpdateInGroup(typeof(GhostPredictionSystemGroup))]
	public partial class InputResponseMovement_S : SystemBase {

		private GhostPredictionSystemGroup predictionSystemGroup;

		protected override void OnCreate() {

			this.predictionSystemGroup = World.GetOrCreateSystem<GhostPredictionSystemGroup>();

		}

		protected override void OnUpdate() {

			var deltaTime = this.predictionSystemGroup.Time.DeltaTime;
			var currentTick = this.predictionSystemGroup.PredictingTick;

			var settings = GetSingleton<GameSettings_C>();
			var playerSettings = GetSingleton<PlayerSettings_AC>();

			var inputFromEntity = GetBufferFromEntity<PlayerCommand>(true);

			Entities
				.WithReadOnly(inputFromEntity)
				.WithAll<PlayerTag, PlayerCommand>()
				.ForEach((Entity entity, int nativeThreadIndex, ref Rotation rot, ref Velocity_C velocity,
					in GhostOwnerComponent ghostOwner,
					in LocalToWorld localToWorld,
					in PredictedGhostComponent prediction) => {

					if (GhostPredictionSystemGroup.ShouldPredict(currentTick, prediction) == false) {
					
						return;
					
					}

					var input = inputFromEntity[entity];

					PlayerCommand inputData;
					if (input.GetDataAtTick(currentTick, out inputData) == false) {

						inputData.shoot = false;

					}
					
					var moveVector = inputData.movement;
					var look = inputData.mouse;
					if (inputData.brake == true) {

						velocity.Linear = float3.zero;

					}
					else if (moveVector != Vector2.zero) {

						var moveProjectV = new float3(moveVector.x, 0, moveVector.y);

						velocity.Linear += (math.mul(localToWorld.Rotation, moveProjectV).xyz) * settings.playerForce * deltaTime;

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

	}

}
