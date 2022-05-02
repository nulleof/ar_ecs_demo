using UnityEngine;

namespace Managers.Base {
	
	public interface IManager {

		public void OnCreate();
		public void Initialize();

		public void UpdateTick(float deltaTime);

	}

	public interface IBaseManager {

		public void OnCreateManager(IManager manager);

	}

	public class BaseManager<T> : MonoBehaviour, IManager where T : MonoBehaviour, IManager {

		protected static T _instance;

		public static T instance {

			get {
				
				BaseManager<T>.Register();

				return BaseManager<T>._instance;

			}
			
		}

		public static T1 Create<T1>() where T1 : IManager {

			var name = typeof(T1).FullName;

			var go = new GameObject($"[M]{name}", typeof(T));
			GameObject.DontDestroyOnLoad(go);

			var instance = go.GetComponent<T1>();

			if (typeof(T1) != typeof(Managers)) {

				var managers = Managers.instance;
				go.transform.SetParent(managers.transform);
				(managers as IBaseManager).OnCreateManager(instance);

			}
			
			(instance as IManager).OnCreate();

			return instance;

		}

		public virtual void OnCreate() {

		}

		public void Initialize() {

		}

		public void UpdateTick(float deltaTime) {

		}

		public static void Register() {

			if (BaseManager<T>._instance == null) {

				BaseManager<T>._instance = BaseManager<T>.Create<T>();

			}
			
		}

	}

}
