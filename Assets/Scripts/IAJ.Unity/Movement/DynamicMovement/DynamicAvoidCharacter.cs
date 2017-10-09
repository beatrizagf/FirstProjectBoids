using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicAvoidCharacter : DynamicMovement
{
    

    public override string Name
    {
        get { return "Avoid Character"; }
    }
    
    public Vector3 DeltaPos { get; set; }
    public Vector3 DeltaVel { get; set; }
    public float DeltaSpeed { get; set; }
    public float TimeToClosest { get; set; }
    public Vector3 FutureDeltaPos { get; set; }
    public float FutureDistance { get; set; }
    public float CollisionRadius { get; set; }
    public float AvoidMargin { get; set; }
    public KinematicData OtherCharacter { get; set; }








        public DynamicAvoidCharacter(KinematicData target)
		{
			this.Output = new MovementOutput();
			this.OtherCharacter = target;
        }

        public override MovementOutput GetMovement() {

			this.DeltaPos = this.Character.Position - this.OtherCharacter.Position;
			this.DeltaVel = this.Character.velocity - this.OtherCharacter.velocity;
			this.DeltaSpeed = this.DeltaVel.magnitude;


            if (this.DeltaSpeed == 0) {
                return this.Output;
            }

            this.TimeToClosest = -Vector3.Dot(this.DeltaPos,this.DeltaVel)/(this.DeltaSpeed*this.DeltaSpeed);

            if (this.TimeToClosest > AvoidMargin) {
                return this.Output;
            }

            this.FutureDeltaPos = this.DeltaPos + this.DeltaVel * this.TimeToClosest;

            this.FutureDistance = this.FutureDeltaPos.magnitude;

            if (this.FutureDistance > 2*this.CollisionRadius) {
                return new MovementOutput();
            }

            if (this.FutureDistance <= 0 || this.DeltaPos.magnitude < 2 * this.CollisionRadius) {
                //deals with exact or immediate collisions
                this.Output.linear = this.Character.Position - this.OtherCharacter.Position;

            } else {
                this.Output.linear = FutureDeltaPos * -1;
            }

            this.Output.linear = this.Output.linear.normalized * MaxAcceleration;


        return this.Output;
    }
}
}
