using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine;

namespace ScriptsAndPrefabs.MultiplayerSetup {

	[UpdateInWorld(TargetWorld.Server)]
	public partial class ServerConnectionControl_S : SystemBase {

		public struct InitializeServer_C : IComponentData { }

		public struct ServerData_C : IComponentData {

			public ushort gamePort;

		}

		protected override void OnCreate() {

			RequireSingletonForUpdate<InitializeServer_C>();

		}

		protected override void OnUpdate() {

			var serverDataEntity = GetSingletonEntity<ServerData_C>();
			var serverData = EntityManager.GetComponentData<ServerData_C>(serverDataEntity);
			var gamePort = serverData.gamePort;

			EntityManager.DestroyEntity(GetSingletonEntity<InitializeServer_C>());

			var grid = EntityManager.CreateEntity();

			EntityManager.AddComponentData(grid, new GhostDistanceImportance() {
				ScaleImportanceByDistance = GhostDistanceImportance.DefaultScaleFunctionPointer,
				TileSize = new int3(80, 80, 80),
				TileCenter = new int3(0, 0, 0),
				TileBorderWidth = new float3(1f, 1f, 1f),
			});

			NetworkEndPoint ep = NetworkEndPoint.AnyIpv4;
			ep.Port = gamePort;
			World.GetExistingSystem<NetworkStreamReceiveSystem>().Listen(ep);
			Debug.Log($"Server is listening on port: {gamePort}");

		}

	}

}
