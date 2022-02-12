using System;
using Unity.Entities;

namespace ScriptsAndPrefabs.Player {

    [Serializable]
    [GenerateAuthoringComponent]
    public struct PlayerAuthoringComponent : IComponentData {

        public Entity prefab;

    }

}
