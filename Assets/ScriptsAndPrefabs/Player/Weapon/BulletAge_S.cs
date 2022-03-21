using Unity.Entities;

namespace ScriptsAndPrefabs.Player.Weapon {

	[UpdateInGroup(typeof(SimulationSystemGroup))]
	public partial class BulletAge_S : SystemBase {

		private BeginSimulationEntityCommandBufferSystem beginSimECB;

		protected override void OnCreate() {

			this.beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

		}

		protected override void OnUpdate() {

			var commandBuffer = this.beginSimECB.CreateCommandBuffer().AsParallelWriter();
			var deltaTime = Time.DeltaTime;
			
			Entities
				.ForEach((Entity e, int nativeThreadIndex, ref BulletAge_C bulletAge) => {

					bulletAge.age += deltaTime;

					if (bulletAge.age >= bulletAge.maxAge) {
						
						commandBuffer.DestroyEntity(nativeThreadIndex, e);
						
					}
					
				}).ScheduleParallel();
			
			this.beginSimECB.AddJobHandleForProducer(Dependency);

		}

	}

}