using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers.Base {

	public class Managers : BaseManager<Managers>, IBaseManager {

		private List<IManager> list = new List<IManager>();

		public void OnCreateManager(IManager manager) {
			
			this.list.Add(manager);
			
		}

		public override void OnCreate() {
			
			this.list.Clear();
			this.transform.SetSiblingIndex(0);
			
		}

		private void Update() {

			var deltaTime = Time.deltaTime;

			for (int i = 0; i < Managers.instance.list.Count; ++i) {

				var manager = Managers.instance.list[i];
				manager.UpdateTick(deltaTime);

			}
			
		}

		public static void Cleanup() {
			
			Managers.instance.Cleanup_INTERNAL();
			
		}

		public void Cleanup_INTERNAL() {

			for (int i = 0; i < this.list.Count; ++i) {
				
				GameObject.Destroy((this.list[i] as MonoBehaviour).gameObject);
				
			}
			
			this.list.Clear();
			
		}

	}

}
