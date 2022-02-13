using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ScriptsAndPrefabs.Player.Weapon {

	[GenerateAuthoringComponent]
	public struct Weapon_C : IComponentData {

		public float cooldownLeft;
		public float3 spawnerLocalPos;
		public quaternion spawnerLocalRot;

	}

}