using Unity.Entities;

namespace ScriptsAndPrefabs.Player.Weapon {

	public class WeaponCool_S : SystemBase {

		protected override void OnUpdate() {

			var deltaTime = Time.DeltaTime;

			Entities
				.WithAll<Weapon_C>()
				.ForEach((ref Weapon_C weaponC) => { weaponC.cooldownLeft -= deltaTime; })
				.ScheduleParallel();

		}

	}

}