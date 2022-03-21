using System;
using Unity.Assertions;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

namespace ScriptsAndPrefabs.Physics {

	public enum EventOverlapState : byte {
		Enter,
		Stay,
		Exit
	}

	public struct StatefulTriggerEvent : IBufferElementData, IComparable<StatefulTriggerEvent> {

		private EntityPair entities;
		private BodyIndexPair bodyIndices;
		private ColliderKeyPair colliderKeys;

		public EventOverlapState State;

		public Entity EntityA => entities.EntityA;
		public Entity EntityB => entities.EntityB;
		public int BodyIndexA => bodyIndices.BodyIndexA;
		public int BodyIndexB => bodyIndices.BodyIndexB;
		public ColliderKey ColliderKeyA => colliderKeys.ColliderKeyA;
		public ColliderKey ColliderKeyB => colliderKeys.ColliderKeyB;

		public StatefulTriggerEvent(
			Entity entityA, Entity entityB,
			int bodyIndexA, int bodyIndexB,
			ColliderKey colliderKeyA, ColliderKey colliderKeyB
		) {

			this.entities = new EntityPair() {
				EntityA = entityA,
				EntityB = entityB,
			};
			this.bodyIndices = new BodyIndexPair() {
				BodyIndexA = bodyIndexA,
				BodyIndexB = bodyIndexB,
			};
			this.colliderKeys = new ColliderKeyPair() {
				ColliderKeyA = colliderKeyA,
				ColliderKeyB = colliderKeyB,
			};
			
			this.State = default;

		}

		public int CompareTo(StatefulTriggerEvent other) {

			var compare = this.entities.EntityA.CompareTo(other.EntityB);
			if (compare != 0) {

				return compare;
				
			}

			compare = this.entities.EntityB.CompareTo(other.EntityA);
			if (compare != 0) {

				return compare;

			}

			if (this.ColliderKeyA.Value != other.ColliderKeyB.Value) {

				return this.ColliderKeyA.Value < other.ColliderKeyB.Value ? 1 : -1;

			}

			if (this.ColliderKeyB.Value != other.ColliderKeyA.Value) {
			
				return this.ColliderKeyB.Value < other.ColliderKeyA.Value ? 1 : -1;
				
			}

			return 0;

		}

		public Entity GetOtherEntity(Entity entity) {
			
			Assert.IsTrue(entity != this.EntityA || entity != this.EntityB);


			var otherEntityDef = math.@select(

				new int2(this.entities.EntityA.Index, this.entities.EntityA.Version),
				new int2(this.entities.EntityB.Index, this.entities.EntityB.Version),
				entity == EntityA
			);

			return new Entity() {
				Index = otherEntityDef[0],
				Version = otherEntityDef[1]
			};

		}

	}
	
	public struct ExcludeFromTriggerEventConversion : IComponentData {}

	public class DynamicBufferTriggerEvent_A : MonoBehaviour, IConvertGameObjectToEntity {

		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

			dstManager.AddBuffer<StatefulTriggerEvent>(entity);

		}
		
	}

	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	[UpdateAfter(typeof(StepPhysicsWorld))]
	[UpdateBefore(typeof(EndFramePhysicsSystem))]
	public partial class TriggerEventConversion_S : SystemBase {

		public JobHandle OutDependency => Dependency;

		private StepPhysicsWorld stepPhysicsWorld = default;
		private BuildPhysicsWorld buildPhysicsWorld = default;
		private EndFramePhysicsSystem endFramePhysicsSystem = default;

		private EntityQuery query = default;

		private NativeList<StatefulTriggerEvent> prevFrameTriggerEvents;
		private NativeList<StatefulTriggerEvent> curFrameTriggerEvents;

		protected override void OnCreate() {

			this.stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
			this.buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
			this.endFramePhysicsSystem = World.GetOrCreateSystem<EndFramePhysicsSystem>();

			this.query = GetEntityQuery(new EntityQueryDesc() {
				All = new ComponentType[] {
					typeof(StatefulTriggerEvent),
				},
				None = new ComponentType[] {
					typeof(ExcludeFromTriggerEventConversion),
				},
			});

			this.prevFrameTriggerEvents = new NativeList<StatefulTriggerEvent>(Allocator.Persistent);
			this.curFrameTriggerEvents = new NativeList<StatefulTriggerEvent>(Allocator.Persistent);

		}

		protected override void OnDestroy() {

			this.prevFrameTriggerEvents.Dispose();
			this.curFrameTriggerEvents.Dispose();

		}

		private void PushPrevFrameEvents() {
			
			(this.prevFrameTriggerEvents, this.curFrameTriggerEvents) = (this.curFrameTriggerEvents, this.prevFrameTriggerEvents);
			this.curFrameTriggerEvents.Clear();

		}

		protected override void OnUpdate() {

			if (this.query.CalculateEntityCount() == 0) return;
			
			Dependency = JobHandle.CombineDependencies(this.stepPhysicsWorld.FinalSimulationJobHandle, Dependency);

			Entities.WithName("ClearTriggerEventDynamicBufferParallelJob")
				.WithNone<ExcludeFromTriggerEventConversion>()
				.ForEach((ref DynamicBuffer<StatefulTriggerEvent> buffer) => {
					
					buffer.Clear();
					
				})
				.ScheduleParallel();

			this.PushPrevFrameEvents();

			var curFrameEvents = this.curFrameTriggerEvents;
			var prevFrameEvents = this.prevFrameTriggerEvents;
			
			var triggerEventBuffer = GetBufferFromEntity<StatefulTriggerEvent>();
			var physicsWorld = this.buildPhysicsWorld.PhysicsWorld;

			var collectTriggerEventJob = new CollectTriggerEventJob() {

				triggerEvents = curFrameEvents,

			};

			var collectJobHandle =
				collectTriggerEventJob.Schedule(this.stepPhysicsWorld.Simulation, Dependency);

			NativeHashSet<Entity> entitiesWithBuffersMsp = new NativeHashSet<Entity>(0, Allocator.TempJob);

			var collectTriggerBuffersHandle = Entities.WithName("CollectTriggerBuffersJob")
				.WithNone<ExcludeFromTriggerEventConversion>()
				.WithAll<StatefulTriggerEvent>()
				.ForEach((Entity e) => {

					entitiesWithBuffersMsp.Add(e);

				}).Schedule(Dependency);
			
			Dependency = JobHandle.CombineDependencies(collectJobHandle, collectTriggerBuffersHandle);

			Job.WithName("ConvertTriggerEventStreamToDynamicBuffersJob")
				.WithCode(() => {

					curFrameEvents.Sort();

					var triggerEventsWithState = new NativeList<StatefulTriggerEvent>(curFrameEvents.Length, Allocator.Temp);
					TriggerEventConversion_S.UpdateTriggerEventState(prevFrameEvents, curFrameEvents, triggerEventsWithState);
					TriggerEventConversion_S.AddTriggerEventsToDynamicBuffers(triggerEventsWithState, ref triggerEventBuffer, entitiesWithBuffersMsp);
					
				}).Schedule();
			
			this.endFramePhysicsSystem.RegisterPhysicsRuntimeSystemReadOnly();
				// AddInputDependency(Dependency);
			entitiesWithBuffersMsp.Dispose(Dependency);

		}

		public static void UpdateTriggerEventState(
			NativeList<StatefulTriggerEvent> prevTriggerEvents,
			NativeList<StatefulTriggerEvent> currentTriggerEvents,
			NativeList<StatefulTriggerEvent> resultTriggerEvents) {

			int i = 0;
			int j = 0;

			while (i < prevTriggerEvents.Length && j < currentTriggerEvents.Length) {

				var prevEvent = prevTriggerEvents[i];
				var currEvent = currentTriggerEvents[j];

				int cmpResult = currEvent.CompareTo(prevEvent);
				
				// events equal. Appears in prev and current frame. STAY
				if (cmpResult == 0) {

					currEvent.State = EventOverlapState.Stay;
					resultTriggerEvents.Add(currEvent);

					++i;
					++j;

				} else if (cmpResult < 0) {
					
					// event appears in current frame but not previous. ENTER
					currEvent.State = EventOverlapState.Enter;
					resultTriggerEvents.Add(currEvent);

					++i;

				}
				else {
					
					// event appeared in previous frame but not current. Exit
					prevEvent.State = EventOverlapState.Exit;
					resultTriggerEvents.Add(prevEvent);

					++j;

				}

			}

			if (i == currentTriggerEvents.Length) {
				
				// all left previous events exit
				while (j < prevTriggerEvents.Length) {

					var prevEvent = prevTriggerEvents[j];
					prevEvent.State = EventOverlapState.Exit;
					resultTriggerEvents.Add(prevEvent);
					++j;

				}
				
			} else if (j == prevTriggerEvents.Length) {
				
				// all left current events enter
				while (i < currentTriggerEvents.Length) {

					var currEvent = currentTriggerEvents[i];
					currEvent.State = EventOverlapState.Enter;
					resultTriggerEvents.Add(currEvent);
					++i;

				}
				
			}

		}

		public static void AddTriggerEventsToDynamicBuffers(
			NativeList<StatefulTriggerEvent> triggerEvents,
			ref BufferFromEntity<StatefulTriggerEvent> bufferFromEntity,
			NativeHashSet<Entity> entitiesWithTriggerBuffers) {

			for (int i = 0; i < triggerEvents.Length; ++i) {

				var triggerEvent = triggerEvents[i];
				if (entitiesWithTriggerBuffers.Contains(triggerEvent.EntityA)) {

					bufferFromEntity[triggerEvent.EntityA].Add(triggerEvent);

				}
				if (entitiesWithTriggerBuffers.Contains(triggerEvent.EntityB)) {

					bufferFromEntity[triggerEvent.EntityB].Add(triggerEvent);

				}

			}

		}

		private struct CollectTriggerEventJob : ITriggerEventsJob {

			public NativeList<StatefulTriggerEvent> triggerEvents;

			public void Execute(TriggerEvent triggerEvent) {
				
				this.triggerEvents.Add(new StatefulTriggerEvent(
					triggerEvent.EntityA,
					triggerEvent.EntityB,
					triggerEvent.BodyIndexA,
					triggerEvent.BodyIndexB,
					triggerEvent.ColliderKeyA,
					triggerEvent.ColliderKeyB
					));
				
			}
			
		}
		
	}
	
}