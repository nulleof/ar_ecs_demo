using Unity.Entities;
using Unity.Transforms;

namespace ScriptsAndPrefabs {

	public class MovementSystem : SystemBase
	{
		protected override void OnUpdate() {

			var deltaTime = Time.DeltaTime;
		
		
			Entities.ForEach((ref Translation translation, in VelocityComponent velocity) => {

				translation.Value.xyz += velocity.value * deltaTime;

			}).ScheduleParallel();
		
		}
	
	}

}
