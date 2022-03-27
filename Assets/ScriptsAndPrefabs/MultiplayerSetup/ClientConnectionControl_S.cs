using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine;

namespace ScriptsAndPrefabs.MultiplayerSetup {

	[UpdateInWorld(TargetWorld.Client)]
	public partial class ClientConnectionControl_S : SystemBase {

		public struct InitializeClient_C : IComponentData { }
		
		public struct ClientData_C : IComponentData {

			public FixedString64Bytes connectToServerIp;
			public ushort gamePort;

		}

		protected override void OnCreate() {

			RequireSingletonForUpdate<InitializeClient_C>();

		}

		protected override void OnUpdate() {

			var clientDataEntity = GetSingletonEntity<ClientData_C>();
			var clientData = EntityManager.GetComponentData<ClientData_C>(clientDataEntity);
			var gamePort = clientData.gamePort;
			var connectToServerIp = clientData.connectToServerIp.Value;

			EntityManager.DestroyEntity(GetSingletonEntity<InitializeClient_C>());

			NetworkEndPoint ep = NetworkEndPoint.Parse(connectToServerIp, gamePort);
			World.GetExistingSystem<NetworkStreamReceiveSystem>().Connect(ep);
			Debug.Log($"Client connecting to ip: {connectToServerIp}, port: {gamePort}");

		}

	}

}
