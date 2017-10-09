using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class GoToMouse : DynamicArrive
    {
        public override string Name
        {
            get { return "Go To Mouse"; }
        }


        public GoToMouse()
        {
        }

        public override MovementOutput GetMovement()
        {


            return base.GetMovement();
        }
    }
}
