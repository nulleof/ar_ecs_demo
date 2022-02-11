using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ScriptsAndPrefabs {

	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	[UpdateBefore(typeof(EndFixedStepSimulationEntityCommandBufferSystem))]
	public class AsteroidsOutOfBoundsSystem : SystemBase {

		private EndFixedStepSimulationEntityCommandBufferSystem endFixedStepSimECB;

		protected override void OnCreate() {

			this.endFixedStepSimECB = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
			RequireSingletonForUpdate<GameSettingsComponent>();

		}

		protected override void OnUpdate() {

			var commandBuffer = this.endFixedStepSimECB.CreateCommandBuffer().AsParallelWriter();
			var settings = GetSingleton<GameSettingsComponent>();

			Entities.WithAll<AsteroidTag>().ForEach((Entity e, int nativeThreadIndex, in Translation position) => {

				if (Mathf.Abs(position.Value.x) > settings.levelWidth / 2
				    || Mathf.Abs(position.Value.y) > settings.levelHeight / 2
				    || Mathf.Abs(position.Value.y) > settings.levelHeight / 2) {

					commandBuffer.AddComponent(nativeThreadIndex, e, new DestroyTag());

				}

			}).ScheduleParallel();

			this.endFixedStepSimECB.AddJobHandleForProducer(Dependency);

		}

	}

}