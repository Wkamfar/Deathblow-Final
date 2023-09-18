using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour // make a training dummy class that inherits from this one, so that I don't need two controllers all the times
{
    //scripts
    //[SerializeField] private GameObject inputManager;
    //[SerializeField] private GameObject movementManager;
    //[SerializeField] private GameObject animationManager;
    //[SerializeField] private GameObject actionManager;
    //[SerializeField] private GameObject collisionManager;
    public CharacterInputManager inputs { get; protected set; }
    public CharacterMovement movements { get; protected set; }
    public CharacterAnimations animations { get;  protected set; }
    public CharacterActions actions { get; protected set; }
    public CharacterBlock blocks { get; protected set; }
    // 
    public CameraController camController { get; private set; }
    public MapManager mapManager { get; private set; }
    //enemy
    public GameObject enemy;
    //controller
    InputDevice controller;
    //player model
    public GameObject playerModel;
    // 
    public GameObject collidersHolder;

    public int stamina { get; private set; }
    public int vitality { get; private set; }
    //ADD ACCELERATION
    //ADD DECELERATION
    //ADD GRAVITY

    //
    [Header("Animations")] //make json files to do this, just store the reference, idk or make a separate script to do this // make it easy to add characters
    //idle
    public GameObject idle;
    //walk
    public GameObject walk;
    public GameObject backWalk;
    //Run
    public GameObject Run;
    //crouch
    public GameObject couch;
    //dash
    public GameObject dash;
    public GameObject backdash;
    //attack
    public GameObject heavySlash;
    public virtual void Setup(InputDevice _controller, GameObject _enemy, CameraController _camController, MapManager _mapManager) // later add in the character / other options you have to make
    {
        //set the enemy for input manager so orientation can be determined
        //game = gameManger.GetComponent<GameVariablesManager>();
        //game = new GameVariablesManager();
        // setting up player
        controller = _controller;
        enemy = _enemy;

        //setting up scripts
        inputs = GetComponent<CharacterInputManager>();
        movements = GetComponent<CharacterMovement>();
        animations = GetComponent<CharacterAnimations>();
        actions = GetComponent<CharacterActions>();
        blocks = GetComponent<CharacterBlock>();
        //
        camController = _camController;
        mapManager = _mapManager;
        inputs.Setup(this, controller);
        movements.Setup(this);
        animations.Setup(this);
        //idle test 
        actions.Setup();
    }
    public virtual void LinkedUpdate(int curFrame, int deltaFrame) // add the linked frame rate
    {
        inputs.LinkedUpdate(curFrame, deltaFrame);
        movements.LinkedUpdate(curFrame, deltaFrame);
        animations.LinkedUpdate(curFrame, deltaFrame);
        actions.LinkedUpdate(curFrame, deltaFrame);
        blocks.LinkedUpdate(curFrame, deltaFrame);
    }
}
enum Stats
{
    Stamina = 0,
    Vitality = 1
}
