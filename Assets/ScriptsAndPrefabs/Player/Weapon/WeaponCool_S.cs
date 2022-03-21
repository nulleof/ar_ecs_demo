using Unity.Entities;

namespace ScriptsAndPrefabs.Player.Weapon {

	public partial class WeaponCool_S : SystemBase {

		protected override void OnUpdate() {

			var deltaTime = Time.DeltaTime;

			Entities
				.ForEach((ref Weapon_AC weaponC) => { weaponC.cooldownLeft -= deltaTime; })
				.ScheduleParallel();

		}

	}

}