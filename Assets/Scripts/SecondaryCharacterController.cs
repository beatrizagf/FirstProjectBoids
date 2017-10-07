using Assets.Scripts.IAJ.Unity.Movement.Arbitration;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryCharacterController : MonoBehaviour {

    public const float X_WORLD_SIZE = 55;
    public const float Z_WORLD_SIZE = 32.5f;
    private const float MAX_ACCELERATION = 40.0f;
    private const float MAX_SPEED = 20.0f;
    private const float DRAG = 0.1f;
    private const float AVOID_MARGIN = 18.0f;
    private const float MAX_LOOK_AHEAD = 20.0f;

    public DynamicCharacter character;
    private PriorityMovement priorityMovement;


    //early initialization
    void Awake()
    {
        this.character = new DynamicCharacter(gameObject);

        this.priorityMovement = new PriorityMovement
        {
            Character = this.character.KinematicData
        };


        character.Movement = this.priorityMovement;

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
            var avoidObstacleMovement = new DynamicAvoidObstacle(obstacle.GetComponent<Collider>())
            {
                MaxAcceleration = MAX_ACCELERATION,
                MaxLookAhead = MAX_LOOK_AHEAD,
                AvoidMargin = AVOID_MARGIN,
                Character = this.character.KinematicData,
                DebugColor = Color.magenta
            };

            this.priorityMovement.Movements.Add(avoidObstacleMovement);
        }

        foreach (var otherCharacter in characters)
        {
            if (otherCharacter != character)
            {
                //TODO: add your avoidCharacter movement here
                var avoidCharacter = new DynamicAvoidCharacter(otherCharacter.KinematicData)
                {
                    Character = this.character.KinematicData,
                    MaxAcceleration = MAX_ACCELERATION,
                    AvoidMargin = AVOID_MARGIN,
                    DebugColor = Color.cyan
                };

                this.priorityMovement.Movements.Add(avoidCharacter);
            }
        }

        var straightAhead = new DynamicStraightAhead
        {
            Character = this.character.KinematicData,
            MaxAcceleration = MAX_ACCELERATION,
            DebugColor = Color.yellow
        };

        this.priorityMovement.Movements.Add(straightAhead);

    }


    // Update is called once per frame
    void Update () {
        UpdateMovingGameObject();
	}

    private void UpdateMovingGameObject()
    {
        if (character.Movement != null)
        {
            character.Update();
            character.KinematicData.ApplyWorldLimit(X_WORLD_SIZE, Z_WORLD_SIZE);
        }
    }
}
