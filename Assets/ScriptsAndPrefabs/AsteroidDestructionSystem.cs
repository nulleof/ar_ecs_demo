using Unity.Entities;

namespace ScriptsAndPrefabs {

	[UpdateInGroup(typeof(LateSimulationSystemGroup))]
	public class AsteroidDestructionSystem : SystemBase {

		private EndSimulationEntityCommandBufferSystem endSimECB;

		protected override void OnCreate() {
			
			this.endSimECB = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
			
		}


		protected override void OnUpdate() {

			var commandBuffer = this.endSimECB.CreateCommandBuffer().AsParallelWriter();

			Entities.WithAll<DestroyTag, AsteroidTag>().ForEach((int nativeThreadIndex, Entity e) => {

				commandBuffer.DestroyEntity(nativeThreadIndex, e);

			}).ScheduleParallel();
			
			this.endSimECB.AddJobHandleForProducer(Dependency);

		}

	}

}