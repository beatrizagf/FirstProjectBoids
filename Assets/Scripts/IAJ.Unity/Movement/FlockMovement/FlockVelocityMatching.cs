using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.IAJ.Unity.Util;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;

namespace Assets.Scripts.IAJ.Unity.Movement.FlockMovement {
	public class FlockVelocityMatching : DynamicVelocityMatch {
		public List<DynamicCharacter> Flock;
		public float Radius;
		public float FanAngle;

		public FlockVelocityMatching() {}
		public MovementOutput getMovement() {
			var averageVelocity = new Vector3();
			int closeBoids = 0;

			foreach (var dBoid in Flock) {
				var boid = dBoid.KinematicData;
				var direction = boid.Position;
				direction -= Character.Position;
				if (direction.magnitude <= Radius) {
					var angle = MathHelper.ConvertVectorToOrientation(direction);
					var angleDifference = MathHelper.ShortestAngleDifference(Character.Orientation, angle);
					if (Math.Abs(angleDifference) <= FanAngle) {
						averageVelocity += boid.velocity;
						closeBoids++;
					}
				}
			}
			if (closeBoids == 0) return new MovementOutput();
			averageVelocity /= closeBoids;
			//Target = new KinematicData();
			Target.velocity = averageVelocity;
			return base.GetMovement();
		}
	}
}
