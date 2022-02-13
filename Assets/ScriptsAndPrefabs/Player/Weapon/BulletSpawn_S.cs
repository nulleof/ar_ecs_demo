using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ScriptsAndPrefabs.Player.Weapon {

	public class BulletSpawn_S : SystemBase {

		private BeginSimulationEntityCommandBufferSystem beginSimECB;

		protected override void OnCreate() {

			this.beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
			RequireSingletonForUpdate<PlayerSettings_AC>();

		}

		protected override void OnUpdate() {

			var bulletPrefab = GetSingleton<Bullet_AC>().prefab;
			var commandBuffer = this.beginSimECB.CreateCommandBuffer().AsParallelWriter();

			var playerSettings = GetSingleton<PlayerSettings_AC>();

			Entities
				.WithAll<Weapon_C>()
				.WithAll<WeaponFire_C>()
				.ForEach((Entity e, int nativeThreadIndex, ref Weapon_C weaponC, in LocalToWorld localToWorld) => {

					if (weaponC.cooldownLeft <= 0) {

						// spawn bullet
						var bulletE = commandBuffer.Instantiate(nativeThreadIndex, bulletPrefab);
						commandBuffer.SetComponent(nativeThreadIndex, bulletE, new Translation() {
							Value = localToWorld.Position + weaponC.spawnerLocalPos,
						});
						commandBuffer.SetComponent(nativeThreadIndex, bulletE, new Velocity_AC() {
							value = math.mul(math.mul(localToWorld.Rotation, weaponC.spawnerLocalRot), new float3(0, 0, 1f)) *
							        playerSettings.bulletVelocity,
						});

						weaponC.cooldownLeft = playerSettings.weaponCooldown;

					}

					commandBuffer.RemoveComponent<WeaponFire_C>(nativeThreadIndex, e);

				}).ScheduleParallel();

			this.beginSimECB.AddJobHandleForProducer(Dependency);

		}

	}

}