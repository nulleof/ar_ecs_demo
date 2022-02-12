using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace ScriptsAndPrefabs.Player {

	[UpdateInGroup(typeof(UpdatePresentationSystemGroup))]
	public class InputPlayerDespawn_S : SystemBase {

		private EntityQuery playerQuery;
		private PlayerInputControl playerInputControl;

		protected override void OnCreate() {

			this.playerQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerTag>());

			this.playerInputControl = new PlayerInputControl();
			this.playerInputControl.PlayerInput.Enable();

		}

		protected override void OnUpdate() {

			var shouldDespawn = this.playerInputControl.PlayerInput.DespawnPlayer.triggered;
			var playerCount = this.playerQuery.CalculateEntityCountWithoutFiltering();

			if (playerCount >= 1 && shouldDespawn) {

				EntityManager.DestroyEntity(this.playerQuery);

			}

		}

	}

}