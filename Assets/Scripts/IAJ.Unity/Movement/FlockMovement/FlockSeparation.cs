using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{

public class FlockSeparation : DynamicMovement
{

        //in order to no collide with each other
    public override string Name
    {
        get { return "FlockSeparation"; }
    }

    public float Radius { get; set; }
    public float SeparationFactor { get; set; }
    public KinematicData Boid { get; set; }


        public FlockSeparation(KinematicData otherCharacter)
    {
        this.Radius = 1.0f;
        this.SeparationFactor = 1.0f;
        this.Boid = otherCharacter;
        this.Output = new MovementOutput();
    }
    public override MovementOutput GetMovement()
    {

            Vector3 Direction = this.Character.Position - this.Boid.Position;
            //Direction.magnitude = distancia
            if (Direction.magnitude < Radius) {
                float SeparationStrength = Mathf.Min(this.SeparationFactor / (Direction.magnitude * Direction.magnitude), MaxAcceleration);
                this.Output.linear += Direction.normalized * SeparationStrength;
            }
                
            

            if (this.Output.linear.magnitude > this.MaxAcceleration) {
                this.Output.linear=this.Output.linear.normalized;
                this.Output.linear *= this.MaxAcceleration;
            }

        return this.Output;
    }
}
}

