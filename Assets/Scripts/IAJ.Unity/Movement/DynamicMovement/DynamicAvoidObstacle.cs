using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{

	public class DynamicAvoidObstacle : DynamicSeek

	{
	public float MaxLookAhead { get; set; }
	public float AvoidMargin { get; set; }
	public Collider Obstacle { get; set; }
	
	public RaycastHit Hit { get; set; }



		public DynamicAvoidObstacle(Collider obstacle) {
		//this.CollisionDetector = new Collision();
			this.Obstacle = obstacle;
		}

		public override string Name
		{
			get { return "Avoid Obstacle"; }
		}


		public override MovementOutput GetMovement()
		{
			RaycastHit hit1 = new RaycastHit();
			RaycastHit hit2 = new RaycastHit();
			RaycastHit hit3 = new RaycastHit();
			bool Collision;
			bool Collision2 = false;
			bool Collision3 = false;
			Vector3 Velocity;
			Vector3 WhiskersLeft;
			Vector3 WhiskersRight;

			if (this.Character.velocity == Vector3.zero) {
				Collision = false;
			} else {
				Velocity = this.Character.velocity;
				Ray RayVector = new Ray(this.Character.Position, Velocity.normalized);
				Debug.DrawRay(this.Character.Position, Velocity.normalized * this.MaxLookAhead);
			
				WhiskersLeft = Quaternion.Euler(0,-30,0) * Velocity;
				Ray RayVector2 = new Ray(this.Character.Position, WhiskersLeft.normalized);
				Debug.DrawRay(this.Character.Position, WhiskersLeft.normalized * this.MaxLookAhead/2);

				WhiskersRight = Quaternion.Euler(0, 30, 0) * Velocity;
				Ray RayVector3 = new Ray(this.Character.Position, WhiskersRight.normalized);
				Debug.DrawRay(this.Character.Position, WhiskersRight.normalized * this.MaxLookAhead/2);

				Collision = Obstacle.Raycast(RayVector, out hit1, this.MaxLookAhead);
				Collision2 = Obstacle.Raycast(RayVector2, out hit2, this.MaxLookAhead/2);
				Collision3 = Obstacle.Raycast(RayVector3, out hit3, this.MaxLookAhead/2);
			}

			if (!Collision && !Collision2 && !Collision3) {
				return new MovementOutput();
			}
			this.Target = new KinematicData();
			if(Collision) {
				this.Target.Position = hit1.point + hit1.normal * this.AvoidMargin;
			} else if (Collision2) {
				this.Target.Position = hit2.point + hit2.normal * this.AvoidMargin;
			} else if (Collision3) {
				this.Target.Position = hit3.point + hit3.normal * this.AvoidMargin;
			} else {
				this.Target.Position = Character.Position;
			}

			return base.GetMovement();
		}
	}
}
