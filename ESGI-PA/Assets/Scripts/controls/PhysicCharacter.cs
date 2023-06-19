using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

internal enum PlayerState
{
    Active,
    Vehicle,
    Unactive
}

[Serializable]
public struct CharacterComponents
{
    [SerializeField] public Transform character;
    [SerializeField] public Rigidbody body;
    [SerializeField] public Collider collider;
    [SerializeField] public PlayerInput input;
    [SerializeField] public Transform camera;
    [SerializeField] public CameraBehavior cameraScript;
    [SerializeField] public Animator animator;
    [SerializeField] public GameObject model;
    [SerializeField] public Player player;
}

public class PhysicCharacter : MonoBehaviour
{
    [SerializeField] private CharacterComponents components;
    public CharacterComponents Components => components;

    private Vector2 _axis;
    private bool shouldJump;
    private bool canJump = true;
    private bool canWallJump = false;

    private int _runAnim;
    
    private bool canDash = true;
    private bool dashing = false;
    private bool isRunning = false;
    private bool useBonus = false;
    private bool _shouldExit = false;

    private bool hasVehicle = false;
    private GameObject _vehicle;

    public GameObject Vehicle
    {
        get => _vehicle;
        set
        {
            _vehicle = value;
            hasVehicle = true;
        }
    }

    private GameObject activeVehicle;
    private PhysicsVehicle activeVehicleProps;

    [SerializeField] private float turnSmoothTime = 0.1f;

    [SerializeField] private int speed = 1;

    [SerializeField] private int jumpForce = 1;

    [SerializeField] private float gravity = 1;

    [SerializeField] private float dashSpeed;

    [Range(1,2)]
    [SerializeField] private float runMultiplier = 1f;

    [SerializeField] private PlayerState state;
    
    private RaycastHit leftWallInfo;
    private RaycastHit rightWallInfo;

    private bool leftWallHit;
    private bool rightWallHit;

    private bool stillOnWall = false;

    public bool isIAControlled = false;
    public CharacterAI AIModule;

    private Vector2 _lookAxis;
    public Vector2 LookAxis => _lookAxis;
    
    // Start is called before the first frame update
    void Start()
    {
        components.input.defaultActionMap = "Character";
        _runAnim = Animator.StringToHash("Running Threshold");

    }

    // Update is called once per frame
    void Update()
    {
        if (!isIAControlled) ReadInput();
        else
        {
            _axis = AIModule.axis * (Time.deltaTime * 10f);
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case PlayerState.Vehicle:
                OnVehicle();
                break;
            case PlayerState.Active:
                DefaultState();
                break;
            case PlayerState.Unactive:
                break;
            default:
                break;
        }
    }

    private void ReadInput()
    {
        if (state == PlayerState.Vehicle)
        {
            _shouldExit = components.input.actions["Escape"].IsPressed();
            return;
        }
        _axis = components.input.actions["Move"].ReadValue<Vector2>() * (Time.deltaTime * 5000f);
        _lookAxis = components.input.actions["Look"].ReadValue<Vector2>();
        shouldJump = components.input.actions["Jump"].IsPressed();
        dashing = components.input.actions["Dash"].IsPressed();
        isRunning = components.input.actions["Run"].IsPressed();
        useBonus = components.input.actions["Bonus"].IsPressed();
    }

    private void OnVehicle()
    {
        if (_shouldExit)
        {
            DestroyVehicle();
            return;
        }
        transform.position = activeVehicleProps.Components.vehicle.position;
    }
    
    private void DefaultState()
    {
        if (useBonus && hasVehicle) InvokeVehicle();
        CheckWalls();
        Gravity();
        Move();
        if (dashing) StartCoroutine(Dash());
        StartCoroutine(Jump());
        canJump = Physics.Raycast(new Ray(components.character.position, -components.character.up), 1.5f);
    }

    private void Move()
    {
        var forward = isIAControlled ? Vector3.forward : components.camera.forward;
        var right = isIAControlled ? Vector3.right : components.camera.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        var forceDirection = (forward * _axis.y) + (right * -_axis.x);
        components.body.AddForce(forceDirection * (speed * (!canJump ? 0.8f : 1) * (isRunning && canJump ? runMultiplier : 1))
            , ForceMode.Acceleration);
        if (forceDirection.magnitude > 0.2f)
        {
            components.animator.SetFloat(_runAnim, canJump ? 1 : 0);
            var targetRotation = Quaternion.LookRotation(forceDirection, Vector3.up);
            components.character.rotation = Quaternion.Lerp(components.character.rotation, targetRotation, turnSmoothTime); 
        }
        else
        {
            components.animator.SetFloat(_runAnim, 0);
        }
    }

    private IEnumerator Jump()
    {
        if (canJump && shouldJump)
        {
            components.body.AddForce(0,jumpForce * Time.deltaTime * 50f,0,ForceMode.Impulse);
            canJump = false;
        }
        else if (canWallJump && shouldJump)
        {
            components.body.AddForce(new Vector3(0,jumpForce * Time.deltaTime * 75f,0) + 
                                     (leftWallHit ? leftWallInfo.normal : rightWallHit ?  rightWallInfo.normal : Vector3.zero) * (Time.deltaTime * 500f)
                ,ForceMode.Impulse);
            canWallJump = false;
            yield return new WaitForSeconds(0.5f);
            canWallJump = true;
        }
    }

    private void Gravity()
    {
        if (canJump) return;
        components.body.AddForce(Vector3.down * (components.body.mass * gravity * Time.deltaTime * 100));
    }

    private IEnumerator Dash()
    {
        if (!canDash) yield break;
        var forward = components.camera.forward;
        var right = components.camera.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        var forceDirection = (forward * _axis.y) + (right * -_axis.x);
        components.body.AddForce(forceDirection * (dashSpeed * (!canJump ? 0.8f : 1)), ForceMode.Impulse);
        if (forceDirection.magnitude > 0.5f)
        {
            if (canJump) components.animator.SetFloat(_runAnim, 1);
            else components.animator.SetFloat(_runAnim, 0);
            var targetRotation = Quaternion.LookRotation(forceDirection, Vector3.up);
            components.character.rotation = Quaternion.Lerp(components.character.rotation, targetRotation, turnSmoothTime); 
        }
        else
        {
            components.animator.SetFloat(_runAnim, 0);
        }
        canDash = false;
        yield return new WaitForSeconds(5f);
        canDash = true;
    }

    private void CheckWalls()
    {
        rightWallHit = Physics.Raycast(new Ray(components.character.position, components.camera.right), out rightWallInfo, 0.7f,
            LayerMask.GetMask("Wall"));
        leftWallHit = Physics.Raycast(new Ray(components.character.position, -components.camera.right), out leftWallInfo, 0.7f,
            LayerMask.GetMask("Wall"));
        canWallJump = rightWallHit || leftWallHit;
        if (rightWallHit) components.character.right = Vector3.Lerp(components.character.right, rightWallInfo.normal, 0.1f);
        if (leftWallHit) components.character.right = Vector3.Lerp(components.character.right, leftWallInfo.normal, 0.1f);;
    }

    private void InvokeVehicle()
    {
        hasVehicle = false;
        activeVehicle = Instantiate(_vehicle, components.character.position, components.character.rotation);
        activeVehicle.TryGetComponent(out activeVehicleProps);
        state = PlayerState.Vehicle;
        components.collider.enabled = false;
        components.cameraScript.Locked = true;
        components.cameraScript.Target = activeVehicleProps.Components.vehicle;
        components.model.SetActive(false);
        activeVehicleProps.LinkPlayer(Components.player);
    }

    public void DestroyVehicle()
    {
        Destroy(activeVehicle);
        components.collider.enabled = true;
        components.cameraScript.Locked = false;
        components.cameraScript.Target = components.character;
        components.model.SetActive(true);
        state = PlayerState.Active;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            AIModule.AddReward(-0.5f);
            AIModule.EndEpisode();
        }
    }
    
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            AIModule.AddReward(-0.1f);
        }
    }
    
}