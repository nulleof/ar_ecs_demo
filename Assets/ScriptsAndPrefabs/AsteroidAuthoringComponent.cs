using System;
using Unity.Entities;

namespace ScriptsAndPrefabs {

	[GenerateAuthoringComponent]
	[Serializable]
	public struct AsteroidAuthoringComponent : IComponentData {

		public Entity prefab;

	}

}
