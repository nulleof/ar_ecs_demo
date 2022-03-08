using System;
using Unity.Entities;

namespace ScriptsAndPrefabs {
	
	[Serializable]
	public struct DebugSettings_C : IComponentData {

		public bool ShowGunMuzzleRays;

	}
	
}