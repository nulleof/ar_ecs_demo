using ScriptsAndPrefabs.AsteroidField;
using ScriptsAndPrefabs.Mixed.Components;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace ScriptsAndPrefabs.Server.Systems {

	// Update in world instead group, because UpdateInGroup can be used only once in attributes
	[UpdateInWorld(TargetWorld.Server)]
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	[UpdateBefore(typeof(EndFixedStepSimulationEntityCommandBufferSystem))]
	public partial class AsteroidsOutOfBoundsSystem : SystemBase {

		private EndFixedStepSimulationEntityCommandBufferSystem endFixedStepSimECB;

		protected override void OnCreate() {

			this.endFixedStepSimECB = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
			RequireSingletonForUpdate<GameSettings_C>();

		}

		protected override void OnUpdate() {

			var commandBuffer = this.endFixedStepSimECB.CreateCommandBuffer().AsParallelWriter();
			var settings = GetSingleton<GameSettings_C>();

			Entities.WithAll<AsteroidTag>().ForEach((Entity e, int nativeThreadIndex, in Translation position) => {

				if (Mathf.Abs(position.Value.x) > settings.levelWidth / 2
				    || Mathf.Abs(position.Value.y) > settings.levelHeight / 2
				    || Mathf.Abs(position.Value.z) > settings.levelDepth / 2) {

					commandBuffer.AddComponent(nativeThreadIndex, e, new DestroyTag());

				}

			}).ScheduleParallel();

			this.endFixedStepSimECB.AddJobHandleForProducer(Dependency);

		}

	}

}