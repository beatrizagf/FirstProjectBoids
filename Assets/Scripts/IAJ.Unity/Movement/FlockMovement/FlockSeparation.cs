using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{

public class FlockSeparation : DynamicMovement
{
    public override string Name
    {
        get { return "FlockSeparation"; }
    }

    public float TimeToTargetSpeed { get; set; }

    public FlockSeparation()
    {
        this.TimeToTargetSpeed = 0.5f;
        this.Output = new MovementOutput();
    }
    public override MovementOutput GetMovement()
    {

           // foreach () {
            //}

        this.Output.linear = (this.Target.velocity - this.Character.velocity) / this.TimeToTargetSpeed;

        return this.Output;
    }
}
}
}
