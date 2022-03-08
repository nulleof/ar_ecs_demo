using System;
using ScriptsAndPrefabs.Player.Weapon;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[Serializable]
public class Weapon_AC : MonoBehaviour, IConvertGameObjectToEntity {

	public Transform spawnerTransform;

	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

		dstManager.AddComponentData<Weapon_C>(entity, new Weapon_C() {
			cooldownLeft = 0,
			spawnerLocalPos = spawnerTransform.localPosition,
			localForward = math.rotate(spawnerTransform.localRotation, math.forward()),
			localUp = math.rotate(spawnerTransform.localRotation, math.up()),
		});

	}

}