using System;
using Unity.Entities;
using UnityEngine;

namespace ScriptsAndPrefabs.Player {

	[GenerateAuthoringComponent]
	[Serializable]
	public struct PlayerSettings_AC : IComponentData {

		[Header("Mouse sensibility")] public float sensibilityHor;
		public float sensibilityVert;

		public float bulletVelocity;
		public float weaponCooldown;

		[Header("Look mouse type")] public bool useRightClickLook;

	}

}