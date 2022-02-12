using System;
using Unity.Entities;

namespace ScriptsAndPrefabs.AsteroidField {

	[GenerateAuthoringComponent]
	[Serializable]
	public struct AsteroidAuthoringComponent : IComponentData {

		public Entity prefab;

	}

}
