using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

namespace ScriptsAndPrefabs.Mixed.Components {

	[GenerateAuthoringComponent]
	public struct Velocity_C : IComponentData {

		[GhostField]
		public float3 Linear;

	}

}
