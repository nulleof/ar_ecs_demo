using UnityEngine;

namespace ScriptsAndPrefabs.MultiplayerSetup {

	public class ClientServerInfo : MonoBehaviour {

		public bool isServer = false;
		public bool isClient = false;

		public string connectToServerIp;
		public ushort gamePort;

	}

}
