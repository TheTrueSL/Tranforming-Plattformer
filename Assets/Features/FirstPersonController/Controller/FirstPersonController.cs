using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;


/// <summary>
///     Controller that handles the character controls and camera controls of the first person player.
/// </summary>


public enum Form { Ox, Rabbit, Tiger, Crane };

public enum State
{
    Standing,
    Walking,
    Running,
    Jumping
};
public enum Trans
{
    nop,
    yep
};

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour, ICharacterSignals
{

    public IObservable<Vector3> Moved => _moved;
    private Subject<Vector3> _moved;

    public ReactiveProperty<bool> IsRunning => _isRunning;
    private ReactiveProperty<bool> _isRunning;

    public IObservable<Unit> Landed => _landed;
    private Subject<Unit> _landed;

    public IObservable<Unit> Jumped => _jumped;
    private Subject<Unit> _jumped;

    public IObservable<float> Swap => _swap;
    private Subject<float> _swap;

    public IObservable<Unit> Stepped => _stepped;
    private Subject<Unit> _stepped;

    [Header("References")]
    [SerializeField] private FirstPersonControllerInput firstPersonControllerInput;
    private CharacterController _characterController;
    private Camera _camera;

    [Header("Movement Options")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float gravity = 5f;
    [SerializeField] private float jumpSpeed = 12f;
    [SerializeField] private float strideLength = 2.5f;
    public float StrideLength => strideLength;

    [Header("Head Angels")]
    [Range(-90, 0)] [SerializeField] private float minViewAngle = -60f;
    [Range(0, 90)] [SerializeField] private float maxViewAngle = 60f;

    
    [Header("State")]
    [SerializeField]
    public Form currentForm = Form.Ox;
    public int currentCount = 0;
    public int currentMax = 1;
    
    [SerializeField]
    TransformationManager transformationManager;

    public bool RabbitUnlocked = false;
    public bool TigerUnlocked = false;
    public bool CraneUnlocked = false;

    public AudioClip[] sounds = new AudioClip[12];

    private AudioSource speakerphone;

    private State currentState = State.Standing;

    private Trans currentTrans = Trans.nop;
	private float lastChange = 0f;

    public void SetForm(Form form)
    {
        transformationManager.TransFormInto(form);
        speakerphone.gameObject.SetActive(false);
        speakerphone.clip = sounds[8];
        speakerphone.gameObject.SetActive(true);
        switch (form)
        {
            case Form.Ox: 
                currentForm = Form.Ox;
                break;
            case Form.Rabbit:
                currentForm = Form.Rabbit;
                jumpSpeed = 13f;
                break;
            case Form.Tiger:
                jumpSpeed = 11f;
                currentForm = Form.Tiger;
                break;
            case Form.Crane: currentForm = Form.Crane;
                jumpSpeed = 12f;
                break;
        }
    }

    private int walkingIndex = 0;
    private AudioClip GetClip()
    {
        int index = 0;
        switch (currentState)
        {
            case State.Running:
            {
                index += 3; break;
            }
            case State.Jumping:
            {
                if (currentForm == Form.Rabbit)
                    return sounds[11];
                else if (currentForm == Form.Crane)
                    return sounds[10];
                else if (currentForm == Form.Tiger)
                    return sounds[9];
                break;
            }case State.Walking:
            {
                index += walkingIndex;
                break;
            }
            default: return null;
        }

        walkingIndex = (walkingIndex + 1)%6;
        return sounds[index];
    }

    private void Update()
    {
        if (currentTrans == Trans.yep)
        {
            dothemagic();
            currentTrans = Trans.nop;
        }

        if (currentState == State.Jumping)
        {
            speakerphone.gameObject.SetActive(false);
            speakerphone.clip = GetClip();
            speakerphone.gameObject.SetActive(true);
        }
        else if (!speakerphone.isPlaying)
        {
            speakerphone.gameObject.SetActive(false);
            speakerphone.clip = GetClip();
            speakerphone.gameObject.SetActive(true);
        }

        currentState = State.Standing;
    }

    private void Awake() {
        _characterController = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<Camera>();
        speakerphone = GetComponentInChildren<AudioSource>();
        //var stepDistance = 0f;
        //Moved.Subscribe(w => {
        //    stepDistance += w.magnitude;
        //    if (stepDistance > strideLength) {
        //        _stepped.OnNext(Unit.Default);
        //   }
        //    stepDistance %= strideLength;
        //}).AddTo(this);
    }

    private void Start()
    {

        this.HandleLocomotion();
        this.Look();
        this.HandleZoom();
        this.HandleTurn();
        initializeMagic();
        
        _isRunning = new ReactiveProperty<bool>(false);
        _moved = new Subject<Vector3>().AddTo(this);
        _jumped = new Subject<Unit>().AddTo(this);
        _swap = new Subject<float>().AddTo(this);
        _landed = new Subject<Unit>().AddTo(this);
        _stepped = new Subject<Unit>().AddTo(this);
    }

    private void HandleTurn() {
        firstPersonControllerInput.Turn.
            Where(v => v != 0f).
            Subscribe(input =>
            {
                if (currentForm == Form.Ox) {
                    float change = input * turnSpeed;
                    transform.Rotate(0, change / 20, 0, Space.Self);
                }
            }).AddTo(this);

        firstPersonControllerInput.SlowMove.
            Where(v => v != 0f).
            Subscribe(input =>
            {
                
                if (currentForm == Form.Ox)
                    {
                   currentState = State.Walking;
                    float movement = input * moveSpeed * 0.75f;
                    float verticalSpeed = 0f; ;
                    if (!_characterController.isGrounded)
                    {
                        verticalSpeed = _characterController.velocity.y + (Physics.gravity.y * Time.deltaTime * 3.0f);
                    }
                    else if (_characterController.isGrounded)
                    //on the ground. Restore base state
                    {
                    verticalSpeed = -Math.Abs(gravity);
                    currentState = State.Walking;
                    }
                        //apply movement

                    var forward = transform.TransformVector(new Vector3(0, verticalSpeed , movement ));
                        _characterController.Move(forward * Time.deltaTime);
                    }
                
            }).AddTo(this);


    }

    private void HandleZoom(){
        firstPersonControllerInput.Zoom
            .Where(v => v != 0f)
            .Subscribe(input => {
                Vector3 direction = _camera.transform.position - transform.position;
                if ((direction.magnitude > 2 || input > 0) && (direction.magnitude < 8 || input < 0))
                {
                    direction = direction.normalized;
                    direction /= 100;
                    _camera.transform.position += input * direction;
                }
            }).AddTo(this);
    }


    private void initializeMagic()
    {
        firstPersonControllerInput.Swap.Subscribe(j =>  
        { 
            if (j==1 && lastChange > 1f) {
				currentTrans = Trans.yep;
				lastChange = 0f;
			}
			else
				lastChange += Time.deltaTime;
    }).AddTo(this);
        // Throttle(System.TimeSpan.FromSeconds(0.6f)) hat dann nicht mehr funktioniert D:
    }


    private void HandleLocomotion()
    {
        //Start of grounded
        _characterController.Move(-gravity * transform.up);


        var jumpLatch = LatchObservables.Latch(this.UpdateAsObservable(), firstPersonControllerInput.Jump, false);


        //Movement
        _ = firstPersonControllerInput.Move
            .Zip(jumpLatch, (m, j) => new MoveInputData(m, j))
            .Where(moveInputData => moveInputData.jump ||
                                    moveInputData.move != Vector2.zero ||
                                   _characterController.isGrounded == false)
            .Subscribe(i =>
            {

                /*In general jumping is possible if you are touching the ground. 
               Bear is a special case. You can never jump. Cat form may even jump in the air
               Bird may always Perform a Jump and cat may perform a jump if it has a double jump left.*/

                bool canJump = _characterController.isGrounded;
                if (currentForm == Form.Ox)
                {
                    canJump = false;
                    
                }else if(currentForm == Form.Crane)
                {
                    canJump = true;
                }

                //vertical movement
                var verticalSpeed = 0f;
                Debug.Log(_characterController.isGrounded);
                //Determine vertical movement
                //on the ground and want to jump
                if ((i.jump && canJump ) || (currentForm == Form.Rabbit && _characterController.isGrounded))
                {
                    verticalSpeed = jumpSpeed;
                    currentState = State.Jumping;
                    //_jumped.OnNext(Unit.Default);
                }
                //not grounded -> gravity
                else if (!_characterController.isGrounded)
                {
                    verticalSpeed = _characterController.velocity.y + (Physics.gravity.y * Time.deltaTime * 3.0f);
                }
                else if(_characterController.isGrounded)
                //on the ground. Restore base state
                {
                    verticalSpeed = -Math.Abs(gravity);
                    currentState = State.Walking;
                }

                //horizontal movement
                // == if Run runspeed else walkspeed

                float currentSpeed;

                if (firstPersonControllerInput.Run.Value && currentForm == Form.Tiger)
                {
                    currentSpeed = runSpeed;
                    if(currentState != State.Jumping)
                        currentState = State.Running;
                }
                else if (currentForm == Form.Ox)
                {
                    currentSpeed = moveSpeed/2;
                }
                else
                {
                    currentSpeed = moveSpeed;
                }

                
                var horizontalVelocity = i.move * currentSpeed;

                //combine horizontal and vertical movement
                var characterSpeed = transform.TransformVector(new Vector3(horizontalVelocity.x, verticalSpeed, horizontalVelocity.y));

                //apply movement
                var distance = characterSpeed * Time.deltaTime;
                _characterController.Move(distance);

                var tempIsRunning = false;
                if (canJump && _characterController.isGrounded)
                {
                    _moved.OnNext(_characterController.velocity * Time.deltaTime);
                    if (_characterController.velocity.magnitude > 0)
                    {
                        tempIsRunning = firstPersonControllerInput.Run.Value;
                    }
                }

                //if (!wasGrounded && _characterController.isGrounded) {
                //    _landed.OnNext(Unit.Default);
                //}
            }).AddTo(this);
    }

    private void Look() {
        firstPersonControllerInput.Look.Where(v => v != Vector2.zero).
            Subscribe(inputLook =>
            {
                //2D Vector in Euler Angles
                if (currentForm != Form.Ox)
                {

                    //Horizontal around vertical axis + being clockwise
                    var horizontalLook = inputLook.x * Vector3.up * Time.deltaTime;
                    transform.localRotation *= Quaternion.Euler(horizontalLook);

                    //Vertical around the horizontal axis + being up
                    var verticalLook = inputLook.y * Vector3.left * Time.deltaTime;
                    var newQ = _camera.transform.localRotation * Quaternion.Euler(verticalLook);

                    _camera.transform.localRotation = RotationTools.ClampRotationAroundXAxis(newQ, -maxViewAngle, -minViewAngle);

                }




            }).AddTo(this);
    }

    private void dothemagic()
    {
        currentCount = (currentCount + 1) % currentMax;
        switch (currentCount)
        {
            case 1: SetForm(Form.Rabbit);
                break;
            case 2: SetForm(Form.Tiger);
                break;
            case 3: SetForm(Form.Crane);
                break;
            default: SetForm(Form.Ox);
                break;
        }
    }

    public struct MoveInputData {
        public readonly Vector2 move;
        public readonly bool jump;

        public MoveInputData(Vector2 move, bool jump) {
            this.move = move;
            this.jump = jump;
        }
    }
}
