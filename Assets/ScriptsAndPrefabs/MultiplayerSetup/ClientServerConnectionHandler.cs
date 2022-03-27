using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace ScriptsAndPrefabs.MultiplayerSetup {

	public class ClientServerConnectionHandler : MonoBehaviour {

		public ClientServerInfo clientServerInfo;

		private GameObject[] launchObjects;

		private void Awake() {

			this.launchObjects = GameObject.FindGameObjectsWithTag("LaunchObject");

			foreach (var launchObject in this.launchObjects) {

				if (launchObject.GetComponent<ServerLaunchObjectData>() != null) {

					this.clientServerInfo.isServer = true;

					foreach (var world in World.All) {

						if (world.GetExistingSystem<ServerSimulationSystemGroup>() != null) {

							var serverDataEntity = world.EntityManager.CreateEntity();
							world.EntityManager.AddComponentData(serverDataEntity,
								new ServerConnectionControl_S.ServerData_C() {
									gamePort = this.clientServerInfo.gamePort,
								});

							world.EntityManager.CreateEntity(typeof(ServerConnectionControl_S.InitializeServer_C));

						}

					}

				}


				if (launchObject.GetComponent<ClientLaunchObjectData>() != null) {

					var launchData = launchObject.GetComponent<ClientLaunchObjectData>();

					this.clientServerInfo.isClient = true;
					this.clientServerInfo.connectToServerIp = launchData.ipAddress;

					foreach (var world in World.All) {

						if (world.GetExistingSystem<ClientSimulationSystemGroup>() != null) {

							var clientDataEntity = world.EntityManager.CreateEntity();
							world.EntityManager.AddComponentData(clientDataEntity,
								new ClientConnectionControl_S.ClientData_C() {
									gamePort = this.clientServerInfo.gamePort,
									connectToServerIp = this.clientServerInfo.connectToServerIp,
								});

							world.EntityManager.CreateEntity(typeof(ClientConnectionControl_S.InitializeClient_C));

						}

					}

				}

			}

		}

	}

}
