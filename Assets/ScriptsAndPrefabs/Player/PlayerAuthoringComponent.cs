using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
[GenerateAuthoringComponent]
public struct PlayerAuthoringComponent : IComponentData {

    public Entity prefab;

}
