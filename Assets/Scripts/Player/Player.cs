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
    protected CharacterInputManager inputs;
    protected CharacterMovement movements;
    protected CharacterAnimations animations;
    protected CharacterActions actions;
    protected CharacterBlock blocks;
    // 
    CameraController camController;
    MapManager mapManager;
    //enemy
    public GameObject enemy;
    //controller
    InputDevice controller;
    //player model
    public GameObject playerModel;
    // 
    public GameObject collidersHolder;

    [SerializeField] int stamina;
    [SerializeField] int vitality;
    //ADD ACCELERATION
    //ADD DECELERATION
    //ADD GRAVITY


    [SerializeField] protected Vector2 statPoint;
    //compare strength x weight to their strength x weight
    //make speed when both pushing equal to excess strength x speed // figure out the calculations later

    // make a character class to 
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
        animations.Setup();
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
    public CharacterInputManager Inputs
    {
        get => inputs;
    }
    public CharacterMovement Movements
    {
        get => movements;
    }
    public CameraController CamController
    {
        get => camController;
    }
    public MapManager MapManager
    {
        get => mapManager;
    }
    public GameObject PlayerModel
    {
        get => playerModel;
    }
    public float Stamina
    {
        get => stamina;
    }
    public float Vitality
    {
        get => vitality;
    }
}
enum Stats
{
    Stamina = 0,
    Vitality = 1
}
