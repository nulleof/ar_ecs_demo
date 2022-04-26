using ScriptsAndPrefabs.Mixed.Components;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

namespace ScriptsAndPrefabs.Mixed.Systems {

	[UpdateInWorld(TargetWorld.ClientAndServer)]
	[UpdateInGroup(typeof(GhostPredictionSystemGroup))]
	public partial class Movement_S : SystemBase {

		private GhostPredictionSystemGroup predictionSystemGroup;

		protected override void OnCreate() {

			this.predictionSystemGroup = World.GetOrCreateSystem<GhostPredictionSystemGroup>();

		}

		protected override void OnUpdate() {

			var deltaTime = this.predictionSystemGroup.Time.DeltaTime;
			var currentTick = this.predictionSystemGroup.PredictingTick;

			Entities.ForEach((ref Translation position, in Velocity_C velocity, in PredictedGhostComponent prediction) => {

				if (GhostPredictionSystemGroup.ShouldPredict(currentTick, prediction) == false) {
					
					return;
					
				}

				position.Value.xyz += velocity.Linear * deltaTime;

			}).ScheduleParallel();

		}

	}

}
