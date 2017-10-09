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

		public List<DynamicCharacter> Flock;
		public float Radius { get; set; }
        public float SeparationFactor { get; set; }

        public FlockSeparation()
        {
            this.Output = new MovementOutput();
        }
        public override MovementOutput GetMovement()
        {


            foreach (var dBoid in Flock)
            {
                var Boid = dBoid.KinematicData;
                if (Character != Boid)
                {

                    Vector3 Direction = this.Character.Position - Boid.Position;
                    //Direction.magnitude = distancia
                    if (Direction.magnitude < Radius)
                    {
                        float SeparationStrength = Mathf.Min(this.SeparationFactor / (Direction.magnitude * Direction.magnitude), MaxAcceleration);
                        this.Output.linear += Direction.normalized * SeparationStrength;
                    }
                }
            }
			if (this.Output.linear.magnitude > this.MaxAcceleration) {
				this.Output.linear = this.Output.linear.normalized;
				this.Output.linear *= this.MaxAcceleration;
			}
			return this.Output;

        }
    }
}

