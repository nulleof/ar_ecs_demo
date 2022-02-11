using Cinemachine;
using ScriptsAndPrefabs;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

[DisallowMultipleComponent]
public class CameraAuthoringComponent : MonoBehaviour, IConvertGameObjectToEntity {

	public CinemachineVirtualCamera virtualCamera;


	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

		dstManager.AddSharedComponentData(entity, new CameraSharedComponent() {
			virtualCamera = this.virtualCamera,
		});

	}

}