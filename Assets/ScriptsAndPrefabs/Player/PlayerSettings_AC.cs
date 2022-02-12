using System;
using Unity.Entities;
using UnityEngine;

namespace ScriptsAndPrefabs.Player {

	[GenerateAuthoringComponent]
	[Serializable]
	public struct PlayerSettings_AC : IComponentData {

		[Header("Mouse sensibility")] public float sensibilityHor;
		public float sensibilityVert;

		[Header("Mouse vertical clamp angle")] public float maxAngle;
		public float minAngle;

		[Header("Spawn player on start")] public bool spawnPlayerOnStart;

	}

}