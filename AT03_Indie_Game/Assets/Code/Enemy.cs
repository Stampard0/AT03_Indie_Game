using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : FiniteStateMachine
{
    public Bounds bounds;
    public float viewRadius;
    public Transform player;

    public NavMeshAgent Agent { get; private set; }

    protected override void Awake()
    {
        entryState = new EnemyIdleState(this);
        if (TryGetComponent(out NavMeshAgent agent) == true)
        {
            Agent = agent;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // we can write custom code to be executed after the original Start definition is run
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
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

public class EnemyIdleState : EnemyBehaviourState
{
    private Vector2 idleTimeRange = new Vector2(3, 10);
    private float timer = -1;
    private float idleTime = 0;

    public EnemyIdleState(Enemy instance) : base(instance)
    {

    }

    public override void OnStateEnter()
    {
        Instance.Agent.isStopped = true;
        idleTime = Random.Range(idleTimeRange.x, idleTimeRange.y);
        timer = 0;
        Debug.Log("Idle state entered, waiting for " + idleTime + " seconds.");
    }

    public override void OnStateExit()
    {
        timer = -1;
        idleTime = 0;
        Debug.Log("Exiting the idle state.");
    }

    public override void OnStateUpdate()
    {
        if (Vector3.Distance(Instance.transform.position, Instance.player.position) <= Instance.viewRadius)
        {
            Instance.SetState(new EnemyChaseState(Instance));
        }

        if (timer >= 0)
        {
            timer += Time.deltaTime;
            if(timer >= idleTime)
            {
                //set state to new state
                Debug.Log("Exiting Idle State after " + idleTime + " seconds.");
                Instance.SetState(new EnemyWanderState(Instance));
            }
        }
    }
}

public class EnemyWanderState : EnemyBehaviourState
{
    private Vector3 targetPosition;
    private float wanderSpeed = 3.5f;

    public EnemyWanderState(Enemy instance) : base(instance)
    {

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
            );
        targetPosition = randomPosInBounds;
        Instance.Agent.SetDestination(targetPosition);
        Debug.Log("Wander state entered with a target pos of " + targetPosition);
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
            Instance.SetState(new EnemyIdleState(Instance));
        }

        if (Vector3.Distance(Instance.transform.position, Instance.player.position) <= Instance.viewRadius)
        {
            Instance.SetState(new EnemyChaseState(Instance));
        }
    }

    public override void DrawStateGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(targetPosition, 0.5f);
    }
}

public class EnemyChaseState : EnemyBehaviourState
{
    private float chaseSpeed = 5f;
    private float defaultSpeed = 0;
    public EnemyChaseState(Enemy instance) : base(instance)
    {

    }
    public override void OnStateEnter()
    {
        Instance.Agent.isStopped = false;
        Instance.Agent.speed = chaseSpeed;
        Debug.Log("Entered chase state.");
    }

    public override void OnStateExit()
    {
        Debug.Log("Exited chase state.");
    }

    public override void OnStateUpdate()
    {
        Instance.Agent.SetDestination(Instance.player.position);

        if (Vector3.Distance(Instance.transform.position, Instance.player.position) > Instance.viewRadius)
        {
            Instance.SetState(new EnemyWanderState(Instance));
        }
    }
}
