using System;
using Unity.Entities;
using Unity.Mathematics;

namespace ScriptsAndPrefabs {

	[GenerateAuthoringComponent]
	[Serializable]
	public struct VelocityComponent : IComponentData {

		public float3 value;

	}

}
