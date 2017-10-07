using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Util;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using System;

namespace Assets.Scripts.IAJ.Unity.Movement.FlockMovement {
	public class FlockCoesion : DynamicArrive {
		KinematicData[] Flock;
		public float Radius;
		public float FanAngle;

		public FlockCoesion() {

		}
		// Use this for initialization
		public override MovementOutput GetMovement() {

			Vector3 massCenter = new Vector3();
			Vector3 direction = new Vector3();
			float closeBoids = 0;

			foreach (var boid in Flock) {
				if (Character != boid) {
					direction = boid.Position;
					direction -= Character.Position;
					if (direction.magnitude <= Radius) {
						var angle = MathHelper.ConvertVectorToOrientation(direction);
						var angleDifference = ShortestAngleDifference(Character.Orientation, angle);
						if (Math.Abs(angleDifference) <= FanAngle) {
							massCenter += boid.Position;
							closeBoids++;
						}
					}
				}
			}
			if (closeBoids == 0) return new MovementOutput();
			massCenter = massCenter * (1/closeBoids);
			Target.Position = massCenter;

			return new MovementOutput();
		}

		public float ShortestAngleDifference(float source, float target) {
			var delta = target - source;
			if (delta > MathConstants.MATH_PI) delta -= 360;
			else if (delta < -MathConstants.MATH_PI) delta += 360;
			return delta;
		}
	}
}