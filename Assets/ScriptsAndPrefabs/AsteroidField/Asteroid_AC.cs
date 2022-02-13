using System;
using Unity.Entities;

namespace ScriptsAndPrefabs.AsteroidField {

	[GenerateAuthoringComponent]
	[Serializable]
	public struct Asteroid_AC : IComponentData {

		public Entity prefab;

	}

}