using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement {
	public class DynamicArrive : DynamicVelocityMatch {
		public override string Name {
			get { return "Arrive"; }
		}

		public float MaxSpeed { get; set; }
		float StopRadius { get; set; }
		float SlowRadius { get; set; }
		public KinematicData RealTarget { get; set; }

		public DynamicArrive() {
			//FakeTarget = new KinematicData();
			StopRadius = 3.0f;
			SlowRadius = 30.0f;
		}

		public override MovementOutput GetMovement() {
			float targetSpeed;
			

			Vector3 direction = Target.Position - Character.Position;
			float distance = direction.magnitude;

			if(distance < StopRadius) {
				targetSpeed = 0;
			} else if (distance > SlowRadius) {
				targetSpeed = MaxSpeed;
			} else {
				targetSpeed = MaxSpeed * (distance / SlowRadius);
			}

			Target.velocity = direction.normalized * targetSpeed;


			return base.GetMovement();
		}
	}
}
