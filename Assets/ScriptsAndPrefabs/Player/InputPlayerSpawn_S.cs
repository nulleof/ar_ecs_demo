using Unity.Entities;
using Unity.Rendering;
using UnityEngine.InputSystem;

namespace ScriptsAndPrefabs.Player {

	[UpdateInGroup(typeof(StructuralChangePresentationSystemGroup))]
	public class InputPlayerSpawn_S : SystemBase {

		private Entity playerPrefab;

		private EntityQuery playerQuery;
		private PlayerInputControl playerInputControl;

		protected override void OnCreate() {

			this.playerQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerTag>());

			this.playerInputControl = new PlayerInputControl();
			this.playerInputControl.PlayerInput.Enable();

			RequireSingletonForUpdate<PlayerSettings_AC>();

		}

		protected override void OnUpdate() {
			if (this.playerPrefab == Entity.Null) {

				this.playerPrefab = GetSingleton<Player_AC>().prefab;
				// return;

			}

			var playerSettings = GetSingleton<PlayerSettings_AC>();

			var shouldSpawn = this.playerInputControl.PlayerInput.SpawnPlayer.phase == InputActionPhase.Performed;

			var playerCount = this.playerQuery.CalculateEntityCountWithoutFiltering();

			if (playerCount < 1 && (shouldSpawn == true || playerSettings.spawnPlayerOnStart == true)) {

				EntityManager.Instantiate(this.playerPrefab);

			}

		}

	}

}