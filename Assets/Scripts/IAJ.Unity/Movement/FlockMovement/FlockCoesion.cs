using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Util;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using System;

namespace Assets.Scripts.IAJ.Unity.Movement.FlockMovement {
	public class FlockCoesion : DynamicArrive {
		public List<DynamicCharacter> Flock;
		public float CRadius;
		public float FanAngle;

		public FlockCoesion() {

		}
		// Use this for initialization
		public override MovementOutput GetMovement() {

			Vector3 massCenter = new Vector3();
			Vector3 direction = new Vector3();
			float closeBoids = 0;

			foreach (var dBoid in Flock) {
				var boid = dBoid.KinematicData;
				if (Character != boid) {
					direction = boid.Position;
					direction -= Character.Position;
					if (direction.magnitude <= CRadius) {
						var angle = MathHelper.ConvertVectorToOrientation(direction);
						var angleDifference = MathHelper.ShortestAngleDifference(Character.Orientation, angle);
						if (Math.Abs(angleDifference) <= FanAngle) {
							massCenter += boid.Position;
							closeBoids++;
						}
					}
				}
			}
			if (closeBoids == 0) return new MovementOutput();
			massCenter = massCenter * (1/closeBoids);
			this.RealTarget = new KinematicData();
			this.RealTarget.Position = massCenter;

			return base.GetMovement();
		}
	}
}