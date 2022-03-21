using Unity.Entities;
using UnityEngine;

namespace ScriptsAndPrefabs.Environment {
	
	public struct CursorLock_C : IComponentData {}

	[UpdateInGroup(typeof(InitializationSystemGroup))]
	public partial class LockCursor_S : SystemBase {

		protected override void OnCreate() {

			var e = EntityManager.CreateEntity(typeof(CursorLock_C));

			RequireSingletonForUpdate<CursorLock_C>();

		}

		protected override void OnUpdate() {
			
			EntityManager.DestroyEntity(GetSingletonEntity<CursorLock_C>());
			
			Cursor.lockState = CursorLockMode.Locked;
			
		}

	}

}