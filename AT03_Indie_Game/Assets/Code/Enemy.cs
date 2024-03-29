using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : FiniteStateMachine, IInteractable
{
    public Bounds bounds;
    [SerializeField] public float viewRadius = 5;
    public Transform player;
    [SerializeField] private float stunCooldown = 3f;
    [SerializeField] public EnemyIdleState idleState;
    [SerializeField] public EnemyWanderState wanderState;
    [SerializeField] public EnemyChaseState chaseState;
    [SerializeField] public StunState stunState;

    private float cooldownTimer = -1;

    public NavMeshAgent Agent { get; private set; }
    public PlayerController Player { get; private set; }
    public Animator Anim { get; private set; }
    public float ViewRadius { get { return viewRadius; } }
    public float DefaultSpeed { get; private set; }
    public float DistanceToPlayer
    {
        get
        {
            if(Player != null)
            {
                return Vector3.Distance(transform.position, Player.transform.position);
            }
            else
            {
                return -1;
            }
        }
    }
    public AudioSource AudioSource { get; private set; }
    public bool ForceChasePlayer { get; private set; }
    public EnemyIdleState IdleState { get { return idleState; } }
    public EnemyWanderState WanderState { get { return wanderState; } }
    public EnemyChaseState ChaseState { get { return chaseState; } }
    public StunState StunState { get { return stunState; } }

    protected override void Awake()
    {
        idleState = new EnemyIdleState(this, idleState);
        wanderState = new EnemyWanderState(this, wanderState);
        chaseState = new EnemyChaseState(this, chaseState);
        stunState = new StunState(this, stunState);
        GoalCollect.ObjectiveActivate += TriggerForceChasePlayer;
        entryState = idleState;
        Objective.VictoryEvent += delegate { SetState(new GameOverState(this)); };
        if (TryGetComponent(out NavMeshAgent agent) == true)
        {
            Agent = agent;
        }
        if (TryGetComponent(out AudioSource aSrc) == true)
        {
            AudioSource = aSrc;
        }
        if(transform.GetChild(0).TryGetComponent(out Animator anim) == true)
        {
            Anim = anim;
        }
        Player = FindObjectOfType<PlayerController>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // we can write custom code to be executed after the original Start definition is run
        SetState(idleState);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(DistanceToPlayer <= viewRadius)
        {
            if(CurrentState.GetType() != typeof(EnemyChaseState) && CurrentState.GetType() != typeof(GameOverState) && CurrentState.GetType() != typeof(StunState))
            {
                SetState(chaseState);
            }
        }

        if(cooldownTimer >= 0)
        {
            cooldownTimer += Time.deltaTime;
            if(cooldownTimer >= stunCooldown)
            {
                cooldownTimer = -1;
            }
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

    private void TriggerForceChasePlayer()
    {
        if (ForceChasePlayer == false)
        {
            ForceChasePlayer = true;
            SetState(chaseState);
        }
    }

    public void Activate()
    {
        if(cooldownTimer < 0 && CurrentState.GetType() != typeof(StunState))
        {
            //Stun the enemy
            StartCoroutine(TriggerStun());
        }
    }

    private IEnumerator TriggerStun()
    {
        SetState(stunState);
        yield return new WaitForSeconds(stunState.StunTimer);
        cooldownTimer = 0;
    }
}
public abstract class EnemyBehaviourState : IState
{
    protected Enemy Instance { get; private set; }

    public EnemyBehaviourState(Enemy instance)
    {
        Instance = instance;
    }

    public abstract void OnStateEnter();

    public abstract void OnStateExit();

    public abstract void OnStateUpdate();

    public virtual void DrawStateGizmos() { }
}
[System.Serializable]
public class EnemyIdleState : EnemyBehaviourState
{
    [SerializeField]
    private Vector2 idleTimeRange = new Vector2(3, 10);
    [SerializeField]
    public AudioClip idleClip;

    private float timer = -1;
    private float idleTime = 0;

    public EnemyIdleState(Enemy instance, EnemyIdleState idle) : base(instance)
    {
        idleTimeRange = idle.idleTimeRange;
        idleClip = idle.idleClip;
    }

    public override void OnStateEnter()
    {
        Instance.Agent.isStopped = true;
        idleTime = Random.Range(idleTimeRange.x, idleTimeRange.y);
        timer = 0;
        Debug.Log("Idle state entered, waiting for " + idleTime + " seconds.");
        Instance.AudioSource.PlayOneShot(idleClip, 0.5f);
        Instance.Anim.SetBool("isMoving", false);
    }

    public override void OnStateExit()
    {
        timer = -1;
        idleTime = 0;
        Debug.Log("Exiting the idle state.");
    }

    public override void OnStateUpdate()
    {
        //if (Vector3.Distance(Instance.transform.position, Instance.player.position) <= Instance.viewRadius)
        //{
            //Instance.SetState(Instance.chaseState);
        //}

        if (timer >= 0)
        {
            timer += Time.deltaTime;
            if(timer >= idleTime)
            {
                //set state to new state
                Debug.Log("Exiting Idle State after " + idleTime + " seconds.");
                Instance.SetState(Instance.wanderState);
            }
        }
    }
}
[System.Serializable]
public class EnemyWanderState : EnemyBehaviourState
{
    private Vector3 targetPosition;
    [SerializeField]
    private float wanderSpeed = 5.5f;
    [SerializeField]
    public AudioClip wanderClip;

    public EnemyWanderState(Enemy instance, EnemyWanderState wander) : base(instance)
    {
        wanderSpeed = wander.wanderSpeed;
        wanderClip = wander.wanderClip;
    }

    public override void OnStateEnter()
    {
        Instance.Agent.speed = wanderSpeed;
        Instance.Agent.isStopped = false;
        Vector3 randomPosInBounds = new Vector3
            (
            Random.Range(-Instance.bounds.extents.x, Instance.bounds.extents.x),
            Instance.transform.position.y,
            Random.Range(-Instance.bounds.extents.z, Instance.bounds.extents.z)
            ) + Instance.bounds.center;
        targetPosition = randomPosInBounds;
        Instance.Agent.SetDestination(targetPosition);
        Debug.Log("Wander state entered with a target pos of " + targetPosition);
        Instance.AudioSource.PlayOneShot(clip: wanderClip, volumeScale: 0.5f);
        Instance.Anim.SetBool("isMoving", true);
        Instance.Anim.SetBool("isChasing", false);
    }

    public override void OnStateExit()
    {
        Debug.Log("Wander state exited.");
    }

    public override void OnStateUpdate()
    {
        Vector3 t = targetPosition;
        t.y = 0;
        //Check if the AI is close to its target positon
        if(Vector3.Distance(Instance.transform.position, targetPosition) <= Instance.Agent.stoppingDistance)
        {
            Instance.SetState(Instance.idleState);
        }

        if (Vector3.Distance(Instance.transform.position, Instance.player.position) <= Instance.viewRadius)
        {
            Instance.SetState(Instance.chaseState);
        }
    }

    public override void DrawStateGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(targetPosition, 0.5f);
    }
}
public class GameOverState : EnemyBehaviourState
{
    public GameOverState(Enemy instance) : base(instance)
    {

    }
    public override void OnStateEnter()
    {
        Debug.Log("Player Caught");
        if(Instance.DistanceToPlayer <= Instance.Agent.stoppingDistance)
        {
            GameHUD.Instance.ActivateEndPrompt(false);
        }
        Instance.Agent.isStopped = true;
        PlayerController.canMove = false;
        MouseLook.mouseLookEnabled = false;
    }
    public override void OnStateUpdate()
    {

    }
    public override void OnStateExit()
    {
        
    }
}
[System.Serializable]
public class EnemyChaseState : EnemyBehaviourState
{
    [SerializeField]
    private float chaseSpeed = 9f;
    [SerializeField]
    public AudioClip chaseClip;

    private float defaultSpeed = 0;
    public EnemyChaseState(Enemy instance, EnemyChaseState chaseState) : base(instance)
    {
        chaseSpeed = chaseState.chaseSpeed;
        chaseClip = chaseState.chaseClip;
    }
    public override void OnStateEnter()
    {
        Instance.Agent.isStopped = false;
        Instance.Agent.speed = chaseSpeed;
        Debug.Log("Entered chase state.");
        Instance.Anim.SetBool("isMoving", true);
        Instance.Anim.SetBool("isChasing", true);
        Instance.AudioSource.PlayOneShot(chaseClip, 0.5f);
    }

    public override void OnStateExit()
    {
        Debug.Log("Exited chase state.");
    }

    public override void OnStateUpdate()
    {
        if(Instance.DistanceToPlayer <= Instance.viewRadius)
        {
            if(Instance.DistanceToPlayer <= Instance.Agent.stoppingDistance)
            {
                Instance.SetState(new GameOverState(Instance));
            }
            else
            {
                Instance.Agent.SetDestination(Instance.Player.transform.position);
            }
        }
        else
        {
            if (Instance.ForceChasePlayer == false)
            {
                if (Vector3.Distance(Instance.transform.position, Instance.Player.transform.position) > Instance.viewRadius)
                {
                    Instance.SetState(Instance.wanderState);
                }
            }
            else
            {
                Instance.Agent.SetDestination(Instance.Player.transform.position);
                Instance.Anim.SetBool("isMoving", true);
                Instance.Anim.SetBool("isChasing", true);
            }
        }
    }
}

[System.Serializable]
public class StunState : EnemyBehaviourState
{
    [SerializeField] private float stunTime;

    private float timer = -1;
    [SerializeField] public AudioClip stunClip;

    public float StunTimer { get { return stunTime; } }

    public StunState(Enemy instance, StunState stun) : base(instance)
    {
        stunTime = stun.stunTime;
        stunClip = stun.stunClip;
    }
    public override void OnStateEnter()
    {
        Debug.Log("I is stunned");
        Instance.AudioSource.PlayOneShot(stunClip, 0.5f);
        Instance.Agent.isStopped = true;
        timer = 0;
    }
    public override void OnStateUpdate()
    {
        if(timer >= 0)
        {
            timer += Time.deltaTime;
            if(timer >= stunTime)
            {
                timer = -1;
                if(Instance.ForceChasePlayer == false)
                {
                    if (Instance.Agent.isStopped == true)
                    {
                        Instance.SetState(Instance.idleState);
                    }
                    else
                    {
                        Instance.SetState(Instance.wanderState);
                    }
                }
                else
                {
                    Instance.SetState(Instance.chaseState);
                }
            }
        }
    }
    public override void OnStateExit()
    {
        Debug.Log("Exiting the stun state.");
        //throw new System.NotImplementedException();
    }
}

