using ScriptsAndPrefabs.Mixed.Commands;
using Unity.Entities;
using UnityEngine;

namespace ScriptsAndPrefabs.Mixed.Components {

	public class PlayerCommandBuffer_AC : MonoBehaviour, IConvertGameObjectToEntity {
		
		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

			dstManager.AddBuffer<PlayerCommand>(entity);

		}

	}

}
