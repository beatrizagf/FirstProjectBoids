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
    public bool Collision { get; set; }
    public bool Collision2 { get; set; }
    public bool Collision3 { get; set; }
    public RaycastHit Hit { get; set; }
    public Vector3 Velocity { get; set; }
    public Vector3 WhiskersLeft { get; set; }
    public Vector3 WhiskersRight { get; set; }



        public DynamicAvoidObstacle(Collider obstacle)
    {
        //this.CollisionDetector = new Collision();
        this.Target = new KinematicData();
        this.Obstacle = obstacle;
     }

    public override string Name
    {
        get { return "Avoid Obstacle"; }
    }


        public override MovementOutput GetMovement()
        {
            RaycastHit hit1;
            RaycastHit hit2;
            RaycastHit hit3;

            if (this.Character.velocity == Vector3.zero) {
                Collision = false;
            }
            else {

            Velocity = this.Character.velocity;
            Ray RayVector = new Ray(this.Character.Position, this.Velocity.normalized);

            Debug.DrawRay(this.Character.Position, this.Velocity.normalized * this.MaxLookAhead);
            
            //
            WhiskersLeft = Quaternion.Euler(0,-30,0) * Velocity;
            Ray RayVector2 = new Ray(this.Character.Position, this.WhiskersLeft.normalized);

            Debug.DrawRay(this.Character.Position, this.WhiskersLeft.normalized * this.MaxLookAhead/2);

            WhiskersRight = Quaternion.Euler(0, 30, 0) * Velocity;
            Ray RayVector3 = new Ray(this.Character.Position, this.WhiskersRight.normalized);

            Debug.DrawRay(this.Character.Position, this.WhiskersRight.normalized * this.MaxLookAhead/2);



            Collision = Obstacle.Raycast(RayVector, out hit1, this.MaxLookAhead);
            Collision2 = Obstacle.Raycast(RayVector2, out hit2, this.MaxLookAhead/2);
            Collision3 = Obstacle.Raycast(RayVector3, out hit3, this.MaxLookAhead/2);


            }

            if (!Collision && Collision2 == false && !Collision3) {
                return new MovementOutput();
            }

            this.Target.Position = Hit.point + Hit.normal * this.AvoidMargin;


        return base.GetMovement();
    }
}

}
