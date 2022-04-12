using ScriptsAndPrefabs.Client.Components;
using ScriptsAndPrefabs.Player;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

namespace ScriptsAndPrefabs.Client.Systems {

	[UpdateInWorld(TargetWorld.Client)]
	[UpdateInGroup(typeof(GhostSimulationSystemGroup))]
	[UpdateAfter(typeof(GhostSpawnClassificationSystem))]
	public partial class PlayerGhostSpawnClassification_S : SystemBase {

		private BeginSimulationEntityCommandBufferSystem beginSimECB;

		private Entity prefabCamera;

		protected override void OnCreate() {

			this.beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
			
			RequireSingletonForUpdate<NetworkIdComponent>();
			RequireSingletonForUpdate<Camera_AC>();

		}
		
		private struct GhostPlayerState : ISystemStateComponentData { }

		protected override void OnUpdate() {

			if (this.prefabCamera == Entity.Null) {

				this.prefabCamera = GetSingleton<Camera_AC>().prefab;

			}

			var commandBuffer = this.beginSimECB.CreateCommandBuffer().AsParallelWriter();

			var camera = this.prefabCamera;
			var playerEntity = GetSingletonEntity<NetworkIdComponent>();

			// command target is NOT readOnly
			var commandTargetFromEntity = GetComponentDataFromEntity<CommandTargetComponent>(false);

			Entities.WithAll<PlayerTag, PredictedGhostComponent>()
				.WithNone<GhostPlayerState>()
				.WithNativeDisableParallelForRestriction(commandTargetFromEntity) // allows modification of Native container in multiple threads 
				.ForEach((Entity entity, int entityInQueryIndex) => {

					var state = commandTargetFromEntity[playerEntity];
					state.targetEntity = entity;
					commandTargetFromEntity[playerEntity] = state;
					
					commandBuffer.AddComponent(entityInQueryIndex, entity, new GhostPlayerState());

					var cameraEntity = commandBuffer.Instantiate(entityInQueryIndex, camera);
					
					commandBuffer.AddComponent(entityInQueryIndex, cameraEntity, new Parent() {Value = entity});
					commandBuffer.AddComponent(entityInQueryIndex, cameraEntity, new LocalToParent());

				}).ScheduleParallel();
			
			this.beginSimECB.AddJobHandleForProducer(Dependency);

		}

	}

}
