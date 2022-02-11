using Cinemachine;
using Unity.Entities;

namespace ScriptsAndPrefabs {

	[GenerateAuthoringComponent]
	public struct CameraSharedComponent : ISharedComponentData, IComponentData {

		public CinemachineVirtualCamera virtualCamera;

	}

}