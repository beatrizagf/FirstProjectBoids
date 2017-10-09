using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement {
	public class DynamicArrive : DynamicVelocityMatch {
		public override string Name {
			get { return "Arrive"; }
		}

		public float MaxSpeed { get; set; }
		public float StopRadius { get; set; }
		public float SlowRadius { get; set; }
		public KinematicData RealTarget { get; set; }

		public DynamicArrive() {
		}

		public override MovementOutput GetMovement() {
			float targetSpeed;
			

			Vector3 direction = RealTarget.Position - Character.Position;
			float distance = direction.magnitude;

			if(distance < StopRadius) {
				targetSpeed = 0;
			} else if (distance > SlowRadius) {
				targetSpeed = MaxSpeed;
			} else {
				targetSpeed = MaxSpeed * (distance / SlowRadius);
			}
			Target = new KinematicData();
			Target.velocity = direction.normalized * targetSpeed;


			return base.GetMovement();
		}
	}
}
