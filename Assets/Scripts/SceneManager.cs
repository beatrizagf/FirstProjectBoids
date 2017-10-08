using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.IAJ.Unity.Movement.Arbitration;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SceneManager : MonoBehaviour
{
    public const float X_WORLD_SIZE = 55;
    public const float Z_WORLD_SIZE = 32.5f;
    public const float AVOID_MARGIN = 4.0f;
    public const float MAX_SPEED = 20.0f;
    public const float MAX_ACCELERATION = 40.0f;
    public const float DRAG = 0.1f;

    public GameObject mainCharacterGameObject;
    public GameObject characterGameObject;

    private BlendedMovement Blended { get; set; }

    private List<FlockCharacterController> characterControllers;

	// Use this for initialization
	void Start () 
	{
		var textObj = GameObject.Find ("InstructionsText");
		if (textObj != null) 
		{
			//textObj.GetComponent<Text>().text = 
			//	"Instructions\n\n" +
				//this.mainCharacterController.blendedKey + " - Blended\n" +
			//	this.mainCharacterController.priorityKey + " - Priority\n"+
             //   this.mainCharacterController.stopKey + " - Stop"; 
		}
	    
        
	    var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

	    this.characterControllers = this.CloneCharacters(this.characterGameObject, 50, obstacles);
        this.characterControllers.Add(this.characterGameObject.GetComponent<FlockCharacterController>());

        //LINQ expression with a lambda function, returns an array with the DynamicCharacter for each secondary character controler
        var characters = this.characterControllers.Select(cc => cc.character).ToList();
        //add the character corresponding to the main character
        //characters.Add(this.mainCharacterController.character);

       // this.mainCharacterController.InitializeMovement(obstacles, characters);//

        //initialize all  characters
	    foreach (var characterController in this.characterControllers)
	    {
            characterController.InitializeMovement(obstacles, characters);
	    }
	}

    private List<FlockCharacterController> CloneCharacters(GameObject objectToClone,int numberOfCharacters, GameObject[] obstacles)
    {
        var characters = new List<FlockCharacterController>();
        for (int i = 0; i < numberOfCharacters; i++)
        {
            var clone = GameObject.Instantiate(objectToClone);
            var Controller = clone.GetComponent<FlockCharacterController>();
            Controller.character.KinematicData.Position = this.GenerateRandomClearPosition(obstacles);
            
            characters.Add(Controller);
        }

        return characters;
    }


    private Vector3 GenerateRandomClearPosition(GameObject[] obstacles)
    {
        Vector3 position = new Vector3();
        var ok = false;
        while (!ok)
        {
            ok = true;

            position = new Vector3(Random.Range(-X_WORLD_SIZE,X_WORLD_SIZE), 0, Random.Range(-Z_WORLD_SIZE,Z_WORLD_SIZE));

            foreach (var obstacle in obstacles)
            {
                var distance = (position - obstacle.transform.position).magnitude;

                //assuming obstacle is a sphere just to simplify the point selection
                if (distance < obstacle.transform.localScale.x + AVOID_MARGIN)
                {
                    ok = false;
                    break;
                }
            }
        }

        return position;
    }

	private Vector3 GenerateRandomVelocity() {
		var velocity = new Vector3();
		velocity.x = Random.Range(0, MAX_SPEED);
		velocity.z = Random.Range(0, MAX_SPEED);
		if (velocity.sqrMagnitude > MAX_SPEED * MAX_SPEED) {
			velocity.Normalize();
			velocity *= MAX_SPEED;
		}
		return velocity;
	}



}
