using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ScriptsAndPrefabs {
	public class DebugRays_S : SystemBase {

		protected override void OnCreate() {
			
			RequireSingletonForUpdate<DebugSettings_C>();
			
		}

		protected override void OnUpdate() {

			var settings = GetSingleton<DebugSettings_C>();

			if (settings.ShowGunMuzzleRays == false) {

				return;

			}
			
			Entities.WithAll<Rotation>()
				.WithAll<DebugRays_AC>()
				.ForEach((in LocalToWorld localToWorld) => {
					
					Debug.DrawRay(localToWorld.Position, localToWorld.Forward, Color.blue);
					Debug.DrawRay(localToWorld.Position, localToWorld.Right, Color.red);
					Debug.DrawRay(localToWorld.Position, localToWorld.Up, Color.green);
					
				}).Run();
			
		}

	}
}