using System;
using Unity.Assertions;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
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

	public class DynamicBufferTriggerEvent_A : MonoBehaviour, IConvertGameObjectToEntity {

		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
			
		}
		
	}
	
}