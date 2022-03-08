using Unity.Entities;

namespace ScriptsAndPrefabs.Player.Weapon {
	public class WeaponTriggerFire_S : SystemBase {
		
		private BeginSimulationEntityCommandBufferSystem beginSimECB;
		
		protected override void OnCreate() {

			this.beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
			RequireSingletonForUpdate<PlayerSettings_AC>();

		}

		protected override void OnUpdate() {

			var commandBuffer = this.beginSimECB.CreateCommandBuffer().AsParallelWriter();
			var playerSettings = GetSingleton<PlayerSettings_AC>();

			Entities
				.WithAll<WeaponFire_C>()
				.ForEach((Entity e, int nativeThreadIndex, ref Weapon_AC weaponC) => {

					if (weaponC.cooldownLeft <= 0) {

						var muzzleE = weaponC.muzzle;
						
						commandBuffer.AddComponent<MuzzleFire_C>(nativeThreadIndex, muzzleE, new MuzzleFire_C());

						weaponC.cooldownLeft = playerSettings.weaponCooldown;

					}

					commandBuffer.RemoveComponent<WeaponFire_C>(nativeThreadIndex, e);

				}).ScheduleParallel();

			this.beginSimECB.AddJobHandleForProducer(Dependency);
			
		}
	}
}