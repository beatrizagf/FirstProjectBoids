using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Movement.Arbitration;
using Assets.Scripts.IAJ.Unity.Movement.FlockMovement;
using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.Movement;

public class FlockCharacterController : MonoBehaviour
{

	public const float X_WORLD_SIZE = 55;
	public const float Z_WORLD_SIZE = 32.5f;
	private const float MAX_ACCELERATION = 40.0f;
	private const float MAX_SPEED = 20.0f;
	private const float DRAG = 0.1f;
	private const float AVOID_COLLISION_RADIUS = 4.0f;
	private const float AVOID_MARGIN = 90.0f;
	private const float MAX_LOOK_AHEAD = 10.0f;
	private const float COESION_RADIUS = 25.0f;
	private const float MATCHING_RADIUS = 10.0f;
	private const float SEPARATION_RADIUS = 15.0f;
	private const float SEPARATION_FACTOR = MAX_ACCELERATION * 1.3f;
	private const float COESION_FAN_ANGLE = MathConstants.MATH_PI_4 * 3;
	private const float ARRIVE_STOP_RADIUS = 8.0f;
	private const float ARRIVE_SLOW_RADIUS = 30.0f;
	private const float MATCHING_FAN_ANGLE = MathConstants.MATH_PI_4 * 3;

	


	public KeyCode stopKey = KeyCode.S;
	public KeyCode blendedKey = KeyCode.B;

	public GameObject movementText;
	public DynamicCharacter character;

	public BlendedMovement blendedMovement;

	private Text movementTextText;

	//early initialization
	void Awake()
	{
		this.character = new DynamicCharacter(this.gameObject);
		this.movementTextText = this.movementText.GetComponent<Text>();


		this.blendedMovement = new BlendedMovement
		{
			Character = this.character.KinematicData
		};
	}

	// Use this for initialization
	void Start()
	{
	}

	public void InitializeMovement(GameObject[] obstacles, List<DynamicCharacter> characters)
	{
		foreach (var obstacle in obstacles)
		{
			//TODO: add your AvoidObstacle movement here
			var avoidObstacleMovement = new DynamicAvoidObstacle(obstacle.GetComponent<Collider>()) {
				MaxAcceleration = MAX_ACCELERATION,
				AvoidMargin = AVOID_MARGIN,
				MaxLookAhead = MAX_LOOK_AHEAD,
				Character = this.character.KinematicData,
				DebugColor = Color.magenta
			};
			this.blendedMovement.Movements.Add(new MovementWithWeight(avoidObstacleMovement, 50.0f));
		}


		var avoidCharacter = new DynamicAvoidCharacter() {
			Flock = characters,
			Character = this.character.KinematicData,
			MaxAcceleration = MAX_ACCELERATION,
			CollisionRadius = AVOID_COLLISION_RADIUS,
			DebugColor = Color.cyan
		};
		this.blendedMovement.Movements.Add(new MovementWithWeight(avoidCharacter, 25.0f));

        //Flock separation movement
        var flockSeparation = new FlockSeparation()
        {
            Flock = characters,
            Character = this.character.KinematicData,
            MaxAcceleration = MAX_ACCELERATION,
            Radius = SEPARATION_RADIUS,
            SeparationFactor = SEPARATION_FACTOR,
            //CollisionRadius = 4.0f,
            //AvoidMargin = 18.0f,
            DebugColor = Color.cyan
        };
        this.blendedMovement.Movements.Add(new MovementWithWeight(flockSeparation, 30.0f));

        var flockCoesion = new FlockCoesion() {
			Character = this.character.KinematicData,
			Flock = characters,
			CRadius = COESION_RADIUS,
			FanAngle = COESION_FAN_ANGLE,
			MaxSpeed = MAX_SPEED,
			StopRadius = ARRIVE_STOP_RADIUS,
			SlowRadius = ARRIVE_SLOW_RADIUS,
			MaxAcceleration = MAX_ACCELERATION
		};
		this.blendedMovement.Movements.Add(new MovementWithWeight(flockCoesion, 12.0f));

		var flockVelocityMatching = new FlockVelocityMatching() {
			Character = this.character.KinematicData,
			Flock = characters,
			Radius = MATCHING_RADIUS,
			FanAngle = MATCHING_FAN_ANGLE
		};
		this.blendedMovement.Movements.Add(new MovementWithWeight(flockVelocityMatching, 4.0f));


		var straightAhead = new DynamicStraightAhead() { Character = this.character.KinematicData };
		this.blendedMovement.Movements.Add(new MovementWithWeight(straightAhead, 10.5f));

		this.character.Movement = this.blendedMovement; 
	}


	void Update()
	{
		if (Input.GetKeyDown(this.stopKey))
		{
			this.character.Movement = null;
		}
		else if (Input.GetKeyDown(this.blendedKey))
		{
			this.character.Movement = this.blendedMovement;
		}

		Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		// bool buttonPressed = false;
		//float seekRadius = 5.0f;
		//List<DynamicCharacter> characters;

		var DynamicArriveMovement = new GoToMouse() {
			Character = this.character.KinematicData,
			MaxAcceleration = MAX_ACCELERATION,
			MaxSpeed = MAX_SPEED,
			StopRadius = ARRIVE_STOP_RADIUS,
			SlowRadius = ARRIVE_SLOW_RADIUS
		};

		if (Input.GetMouseButton(0)) {
			var mousePos = Input.mousePosition;
			mousePos.z = 55.8f;
			var click = camera.ScreenToWorldPoint(mousePos);
			// buttonPressed = true;

			DynamicArriveMovement.RealTarget = new KinematicData();
			DynamicArriveMovement.RealTarget.Position = click;
			this.blendedMovement.Movements.Add(new MovementWithWeight(DynamicArriveMovement, 10.0f));
		}

		this.UpdateMovingGameObject();
		this.UpdateMovementText();
	}


	void OnDrawGizmos()
	{
		//TODO: this code is not working, try to figure it out
		if (this.character != null && this.character.Movement != null)
		{
			//for blending movement
			//if (this.character.Movement == this.blendedMovement)
			//{
			//	var blender = this.character.Movement as BlendedMovement;
			//	var wander = blender.Movements.Find(x => x.Movement is DynamicWander).Movement as DynamicWander;

			//	if (wander != null)
			//	{
			//		Gizmos.color = Color.blue;
			//		Gizmos.DrawWireSphere(wander.CircleCenter, wander.WanderRadius);
			//	}
			//}

		}
	}

	private void UpdateMovingGameObject()
	{
		if (this.character.Movement != null)
		{
			this.character.Update();
			this.character.KinematicData.ApplyWorldLimit(X_WORLD_SIZE, Z_WORLD_SIZE);
		}
	}

	private void UpdateMovementText()
	{
		if (this.character.Movement == null)
		{
			this.movementTextText.text = this.name + " Movement: Stationary";
		}
		else
		{
			this.movementTextText.text = this.name + " Movement: " + this.character.Movement.Name;
		}
	}
}


