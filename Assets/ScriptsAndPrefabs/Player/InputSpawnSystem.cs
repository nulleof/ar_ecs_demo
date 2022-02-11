using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class InputSpawnSystem : SystemBase {

    private Entity playerPrefab;

    private EntityQuery playerQuery;

    protected override void OnCreate() {

        this.playerQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerTag>());

    }

    protected override void OnUpdate()
    {
        if (this.playerPrefab == Entity.Null) {

            this.playerPrefab = GetSingleton<PlayerAuthoringComponent>().prefab;
            // return;

        }

        var fire = Input.GetKey("space");

        var playerCount = this.playerQuery.CalculateEntityCountWithoutFiltering();

        if (playerCount < 1 && fire) {

            EntityManager.Instantiate(this.playerPrefab);

        }
        
    }
}
