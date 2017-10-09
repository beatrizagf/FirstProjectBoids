using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
	public class DynamicAvoidCharacter : DynamicMovement {
	

		public override string Name {
			get { return "Avoid Character"; }
		}

		//public float TimeToClosest { get; set; }
		//public Vector3 FutureDeltaPos { get; set; }
		//public float AvoidMargin { get; set; }


		public float CollisionRadius { get; set; }
		public float MaxTimeLookAhead { get; set; }
		public List<DynamicCharacter> Flock;



		public DynamicAvoidCharacter()
		{
			this.Output = new MovementOutput();


			Target = new KinematicData();
			Character = new KinematicData();
		}

		public override MovementOutput GetMovement() {

			//Vector3 deltaPos = new Vector3();
			//Vector3 deltaVel = new Vector3();
			//float deltaSpeed;
			//float futureDistance;

			//deltaPos = this.Character.Position - this.Target.Position;
			//deltaVel = this.Character.velocity - this.Target.velocity;
			//deltaSpeed = deltaVel.magnitude;


			//if (deltaSpeed == 0) {
			//	return this.Output;
			//}

			//this.TimeToClosest = -Vector3.Dot(deltaPos, deltaVel)/(deltaSpeed * deltaSpeed);

			//if (this.TimeToClosest > AvoidMargin) {
			//	return this.Output;
			//}

			//this.FutureDeltaPos = deltaPos + deltaVel * this.TimeToClosest;

			//futureDistance = this.FutureDeltaPos.magnitude;

			//if (futureDistance > 2*this.CollisionRadius) {
			//	return new MovementOutput();
			//}

			//if (futureDistance <= 0 || deltaPos.magnitude < 2 * this.CollisionRadius) {
			//	//deals with exact or immediate collisions
			//	this.Output.linear = this.Character.Position - this.Target.Position;

			//} else {
			//	this.Output.linear = FutureDeltaPos * -1;
			//}

			//this.Output.linear = this.Output.linear.normalized * MaxAcceleration;


			//return this.Output;

			float shortestTime = 9999999;
			Vector3 deltaPos;
			Vector3 deltaVel;
			float deltaSpeed;
			float timeToClosest;
			Vector3 futureDeltaPos;
			float futureDistance;
			KinematicData closestTarget = new KinematicData();
			float closestFutureDistance = 9999999;
			Vector3 closestFutureDeltaPos = new Vector3();
			Vector3 closestDeltaPos = new Vector3();
			Vector3 closestDeltaVel;
			Vector3 avoidanceDirection = new Vector3();

			foreach (var boid in Flock) {
				deltaPos = Target.Position - Character.Position;
				deltaVel = Target.velocity - Character.velocity;
				deltaSpeed = deltaVel.magnitude;
				if (deltaSpeed == 0) continue;
				timeToClosest = -(Vector3.Dot(deltaPos, deltaVel)) / (deltaSpeed * deltaSpeed);
				if (timeToClosest > MaxTimeLookAhead) continue;
				futureDeltaPos = deltaPos + deltaVel * timeToClosest;
				futureDistance = futureDeltaPos.magnitude;
				if (futureDistance > 2 * CollisionRadius) continue;


				if (timeToClosest > 0 && timeToClosest < shortestTime) {
					shortestTime = timeToClosest;
					closestTarget = Target;
					closestFutureDistance = futureDistance;
					closestFutureDeltaPos = futureDeltaPos;
					closestDeltaPos = deltaPos;
					closestDeltaVel = deltaVel;
				}
			}
			if (shortestTime == 9999999) return new MovementOutput();
			if (closestFutureDistance <= 0 || closestDeltaPos.magnitude < 2 * CollisionRadius) {
				avoidanceDirection = Character.Position - closestTarget.Position;
			} else {
				avoidanceDirection = closestFutureDeltaPos * -1;
			}
			Output = new MovementOutput();
			Output.linear = avoidanceDirection.normalized * MaxAcceleration;
			return this.Output;
		}
	}
}
