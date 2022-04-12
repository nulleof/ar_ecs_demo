using Unity.Entities;

namespace ScriptsAndPrefabs.Client.Components {

	[GenerateAuthoringComponent]
	public struct Camera_AC : IComponentData {

		public Entity prefab;

	}

}
