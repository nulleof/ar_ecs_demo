using Unity.Entities;
using Unity.Jobs;
using Unity.Rendering;
using UnityEngine;

namespace ScriptsAndPrefabs.Physics {
	
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	[UpdateAfter(typeof(TriggerEventConversion_S))]
	public class ChangeMaterialAndDestroy_S : SystemBase {

		private TriggerEventConversion_S triggerConversionS;
		private EndFixedStepSimulationEntityCommandBufferSystem endFixedStepCBS;
		private EntityQueryMask nonTriggerMask;

		protected override void OnCreate() {

			this.triggerConversionS = World.GetOrCreateSystem<TriggerEventConversion_S>();
			this.endFixedStepCBS = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();

			this.nonTriggerMask = EntityManager.GetEntityQueryMask(
				GetEntityQuery(new EntityQueryDesc() {
					None = new ComponentType[] {
						typeof(StatefulTriggerEvent),
					}
				})
			);

		}

		protected override void OnUpdate() {
			
			Dependency = JobHandle.CombineDependencies(this.triggerConversionS.OutDependency, Dependency);
			var commandBuffer = this.endFixedStepCBS.CreateCommandBuffer();

			var nonTriggerMask = this.nonTriggerMask;
			
			Entities.WithName("ChangeMaterialOnTriggerEnter")
				.WithoutBurst()
				.ForEach((Entity e, ref DynamicBuffer<StatefulTriggerEvent> bufferEvents) => {

					for (int i = 0; i < bufferEvents.Length; ++i) {

						var triggerEvent = bufferEvents[i];
						var otherEntity = triggerEvent.GetOtherEntity(e);

						if (triggerEvent.State == EventOverlapState.Stay || !nonTriggerMask.Matches(otherEntity)) {
							
							continue;
							
						}

						if (triggerEvent.State == EventOverlapState.Enter) {

							var volumeRenderMesh = EntityManager.GetSharedComponentData<RenderMesh>(e);
							var overlappingRenderMesh = EntityManager.GetSharedComponentData<RenderMesh>(otherEntity);

							overlappingRenderMesh.material = volumeRenderMesh.material;
							
							commandBuffer.SetSharedComponent(otherEntity, overlappingRenderMesh);

						}
						else { // Exit
							
							commandBuffer.AddComponent(otherEntity, new DestroyTag());
							
						}

					}
					
				}).Run();
			
			this.endFixedStepCBS.AddJobHandleForProducer(Dependency);

		}
		
	}
	
}
