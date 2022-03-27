using AOT;
using ScriptsAndPrefabs.Server.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine;

namespace ScriptsAndPrefabs.Mixed.Commands {

	public struct SendServerGameLoadedRpc : IComponentData, IRpcCommandSerializer<SendServerGameLoadedRpc> {


		public void Serialize(ref DataStreamWriter writer,
			in RpcSerializerState state,
			in SendServerGameLoadedRpc data) { }

		public void Deserialize(ref DataStreamReader reader,
			in RpcDeserializerState state,
			ref SendServerGameLoadedRpc data) { }

		[BurstCompile]
		[MonoPInvokeCallback(typeof(RpcExecutor.ExecuteDelegate))]
		private static void InvokeExecute(ref RpcExecutor.Parameters parameters) {

			var rpcData = default(SendServerGameLoadedRpc);

			rpcData.Deserialize(ref parameters.Reader, parameters.DeserializerState, ref rpcData);

			parameters.CommandBuffer.AddComponent(parameters.JobIndex, parameters.Connection,
				new PlayerSpawningState_C());
			parameters.CommandBuffer.AddComponent(parameters.JobIndex, parameters.Connection,
				default(NetworkStreamInGame));
			parameters.CommandBuffer.AddComponent(parameters.JobIndex, parameters.Connection,
				default(GhostConnectionPosition));

			//iOS will crash if Debug.Log is used within an RPC so we will remove this in the ARFoundation section
			Debug.Log("Server acted on confirmed game load");

		}

		private static PortableFunctionPointer<RpcExecutor.ExecuteDelegate> InvokeExecuteFunctionPointer =
			new PortableFunctionPointer<RpcExecutor.ExecuteDelegate>(SendServerGameLoadedRpc.InvokeExecute);

		public PortableFunctionPointer<RpcExecutor.ExecuteDelegate> CompileExecute() {

			return SendServerGameLoadedRpc.InvokeExecuteFunctionPointer;

		}

	}

	public partial class
		SendServerGameLoadedRpcCommandRequest_S : RpcCommandRequestSystem<SendServerGameLoadedRpc,
			SendServerGameLoadedRpc> {

		protected struct SendRpc : IJobEntityBatch {

			public SendRpcData data;

			public void Execute(ArchetypeChunk batchInChunk, int batchIndex) {

				this.data.Execute(batchInChunk, batchIndex);

			}

		}

		protected override void OnUpdate() {

			var sendJob = new SendRpc() {data = InitJobData()};
			ScheduleJobData(sendJob);

		}

	}

}
