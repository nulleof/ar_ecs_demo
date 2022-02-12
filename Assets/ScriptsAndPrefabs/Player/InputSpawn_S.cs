using Unity.Entities;
using Unity.Rendering;

namespace ScriptsAndPrefabs.Player {

	[UpdateInGroup(typeof(StructuralChangePresentationSystemGroup))]
	public class InputSpawn_S : SystemBase {

		private Entity playerPrefab;

		private EntityQuery playerQuery;
		private PlayerInputControl playerInputControl;

		protected override void OnCreate() {

			this.playerQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerTag>());

			this.playerInputControl = new PlayerInputControl();
			this.playerInputControl.PlayerInput.Enable();

		}

		protected override void OnUpdate() {
			if (this.playerPrefab == Entity.Null) {

				this.playerPrefab = GetSingleton<PlayerAuthoringComponent>().prefab;
				// return;

			}

			var shouldSpawn = this.playerInputControl.PlayerInput.SpawnPlayer.triggered;

			var playerCount = this.playerQuery.CalculateEntityCountWithoutFiltering();

			if (playerCount < 1 && shouldSpawn) {

				EntityManager.Instantiate(this.playerPrefab);

			}

		}

	}

}