using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct Bullet_AC : IComponentData {

	public Entity prefab;

}
