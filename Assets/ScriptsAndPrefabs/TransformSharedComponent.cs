using Unity.Entities;
using UnityEngine;

namespace ScriptsAndPrefabs {

	[GenerateAuthoringComponent]
	public struct TransformSharedComponent : ISharedComponentData, IComponentData {

		public Transform transform;

	}

}