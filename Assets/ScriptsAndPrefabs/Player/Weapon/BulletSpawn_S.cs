using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
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
				.WithAll<Muzzle_AC>()
				.WithAll<MuzzleFire_C>()
				.ForEach((Entity e, int nativeThreadIndex, in LocalToWorld localToWorld) => {

					var gunDirectionV = math.normalize(localToWorld.Forward);

					// spawn bullet
					var bulletE = commandBuffer.Instantiate(nativeThreadIndex, bulletPrefab);
					commandBuffer.SetComponent(nativeThreadIndex, bulletE, new Translation() {
						Value = localToWorld.Position,
					});
					commandBuffer.SetComponent(nativeThreadIndex, bulletE, new Rotation() {
						Value = quaternion.LookRotation(gunDirectionV, localToWorld.Up),
					});
					commandBuffer.SetComponent(nativeThreadIndex, bulletE, new PhysicsVelocity() {
						Linear = gunDirectionV *
						        playerSettings.bulletVelocity,
					});

					commandBuffer.RemoveComponent<MuzzleFire_C>(nativeThreadIndex, e);

				}).ScheduleParallel();

			this.beginSimECB.AddJobHandleForProducer(Dependency);

		}

	}

}