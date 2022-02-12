using System;
using Unity.Entities;
using Unity.Mathematics;

namespace ScriptsAndPrefabs {

	[GenerateAuthoringComponent]
	[Serializable]
	public struct Velocity_AC : IComponentData {

		public float3 value;

	}

}