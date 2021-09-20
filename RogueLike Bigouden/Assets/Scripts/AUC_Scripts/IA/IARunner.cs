using UnityEngine;
using UnityEngine.AI;

public class IARunner : MonoBehaviour
{
    //Utilities
    [SerializeField] Transform target;
    private NavMeshAgent agent;
    private Rigidbody2D rb;
    public bool playerInAttackRange, readyToShoot, playerAggro;
    
    // Tweakable Values
    public float attackRange = 1.5f;
    public float timeToResetShoot = 1f;
    public float timeBeforeAggro = .5f;
    public Transform shootPoint;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerAggro = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        readyToShoot = true;
        Invoke(nameof(WaitToGo), timeBeforeAggro);
    }
    
    private void FixedUpdate()
    { 
        Vector3 lookdir = target.position - rb.transform.position;
        float angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }
    void Update()
    {
        playerInAttackRange = Vector2.Distance(transform.position, target.position) < attackRange;

        if (!playerInAttackRange && playerAggro)
            ChasePlayer();
        if (playerInAttackRange && playerAggro)
            Attacking();
    }

    private void ChasePlayer()
    {
        agent.SetDestination(target.position);
    }

    private void Attacking()
    {
        agent.SetDestination(transform.position);

        if (readyToShoot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        // Play an attack animation
        // Detect if player in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(shootPoint.position, attackRange / 2);
        // Damage if true
        foreach (Collider2D hittenObj  in hitEnemies)
        {
            Debug.Log("We hit " + hittenObj.name);
        }
        Invoke(nameof(ResetShoot), timeToResetShoot);
    }

    private void ResetShoot()
    {
        readyToShoot = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(shootPoint.position, attackRange / 2);
    }

    private void WaitToGo()
    {
        playerAggro = true;
    }
}
