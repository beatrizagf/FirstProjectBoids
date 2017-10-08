using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Movement.Arbitration;
using Assets.Scripts.IAJ.Unity.Movement.FlockMovement;
using System.Collections.Generic;

public class FlockCharacterController : MonoBehaviour
{

	public const float X_WORLD_SIZE = 55;
	public const float Z_WORLD_SIZE = 32.5f;
	private const float MAX_ACCELERATION = 40.0f;
	private const float MAX_SPEED = 20.0f;
	private const float DRAG = 0.1f;
	private const float AVOID_MARGIN = 18.0f;
	private const float MAX_LOOK_AHEAD = 20.0f;
	private const float COESION_RADIUS = 5.0f;
	private const float COESION_FAN_ANGLE = MathConstants.MATH_PI_2;


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
			this.blendedMovement.Movements.Add(new MovementWithWeight(avoidObstacleMovement, 5.0f));
		}

		foreach (var otherCharacter in characters)
		{
			if (otherCharacter != this.character)
			{
				//TODO: add your AvoidCharacter movement here
				var flockSeparation = new FlockSeparation(otherCharacter.KinematicData)
				{
					Character = this.character.KinematicData,
					MaxAcceleration = MAX_ACCELERATION,
					DebugColor = Color.cyan
				};
				this.blendedMovement.Movements.Add(new MovementWithWeight(flockSeparation, 5.0f));
			}
		}

		var flockCoesion = new FlockCoesion() {
			Flock = characters,
			Radius = COESION_RADIUS,
			FanAngle = COESION_FAN_ANGLE
		};
		this.blendedMovement.Movements.Add(new MovementWithWeight(flockCoesion, 5.0f));

		var flockVelocityMatching = new FlockVelocityMatching() {
			Flock = characters,
			Radius = COESION_RADIUS,
			FanAngle = COESION_FAN_ANGLE
		};
		this.blendedMovement.Movements.Add(new MovementWithWeight(flockVelocityMatching, 5.0f));

		/*
		 * TODO: add your wander behaviour here!*/
		var wander = new DynamicWander {
			MaxAcceleration = MAX_ACCELERATION,
			WanderOffset = 5,
			WanderRadius = 5.0f,
			WanderRate = MathConstants.MATH_PI_4 / 3,
			Character = this.character.KinematicData,
			DebugColor = Color.yellow
		};
		this.blendedMovement.Movements.Add(new MovementWithWeight(wander, 1));

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


		this.UpdateMovingGameObject();
		this.UpdateMovementText();
	}


	void OnDrawGizmos()
	{
		//TODO: this code is not working, try to figure it out
		if (this.character != null && this.character.Movement != null)
		{
			//for blending movement
			if (this.character.Movement == this.blendedMovement)
			{
				var blender = this.character.Movement as BlendedMovement;
				var wander = blender.Movements.Find(x => x.Movement is DynamicWander).Movement as DynamicWander;

				if (wander != null)
				{
					Gizmos.color = Color.blue;
					Gizmos.DrawWireSphere(wander.CircleCenter, wander.WanderRadius);
				}
			}

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


