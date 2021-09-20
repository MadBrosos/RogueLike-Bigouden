using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;


public class IABarman : MonoBehaviour
{
    // Utilities
    [SerializeField] private Transform target;
    private NavMeshAgent agent;
    private Rigidbody2D rb;
    [SerializeField] private bool playerInAttackRange, readyToShoot, playerAggro;
    
    // Tweakable Values
    public float attackRange = 1.5f;
    public float timeToResetShoot = 1f;
    public float timeBeforeAggro = .5f;
    public float height; // Hauteur de la parabole
    private float shootForce;
    public Transform shootPoint;
    
    public int _numberOfElements = 10; // Number of elements should draw in path of parabola
    List<GameObject> _trajectoryElementsContainer = new List<GameObject> ();
    public Transform _moveableObject; // Objet to move on path



    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerAggro = false;
        readyToShoot = true;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        Invoke(nameof(WaitToGo), timeBeforeAggro);

    }

    private void FixedUpdate()
    {
        Vector3 lookdir = target.position - rb.transform.position;
        float angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    // Update is called once per frame
    void Update()
    {
        playerInAttackRange = Vector2.Distance(transform.position, target.position) < attackRange;

        if (playerInAttackRange && playerAggro)
            Attacking();

        float distributionTime = 0;
        for (float i = 1; i <= _numberOfElements; i++)
        {
            distributionTime++;
            Vector3 currentPosition =
                SampleParabola(transform.position, target.position, height, i / (float) _numberOfElements);
            _trajectoryElementsContainer[(int) i - 1].transform.position =
                new Vector3(currentPosition.x, currentPosition.y, 0);

            Vector3 nextPosition = SampleParabola(transform.position, target.position, height,
                (i + 1) / (float) _numberOfElements);
            float angleInR = Mathf.Atan2((nextPosition.y - currentPosition.y), (nextPosition.x - currentPosition.x));
            _trajectoryElementsContainer[(int) i - 1].transform.eulerAngles =
                new Vector3(0, 0, (Mathf.Rad2Deg * angleInR) - 90);
        }

        if (_moveableObject)
        {
            //Shows how to animate something following a parabola
            _moveableObject.position = SampleParabola(transform.position, target.position, height, Time.time % 1);
        }
    }

    private void Attacking()
    {
        if (readyToShoot)
            Shoot();
    }

    private void Shoot()
    {
        shootForce = Random.Range(1.5f, 2.1f);
        height = Random.Range(1, 2);
        readyToShoot = false;
        // Play an attack animation
        GameObject projectile = ObjectPooler.Instance.SpawnFromPool("ProjectileBarman", shootPoint.position, Quaternion.identity);
        Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
        rbProjectile.position = SampleParabola(transform.position, target.position, height, 0.7f);
        rbProjectile.rotation = rb.rotation;
        Invoke(nameof(ResetShoot), timeToResetShoot);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange); 
    }

    Vector3 SampleParabola (Vector3 start, Vector3 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        if (Mathf.Abs (start.y - end.y) < 0.1f) {
            Vector3 travelDirection = end - start;
            Vector3 result = start + t * travelDirection;
            result.y += (-parabolicT * parabolicT + 1) * height;
            return result;
        } else {
            Vector3 travelDirection = end - start;
            Vector3 levelDirection = end - new Vector3 (start.x, end.y, start.z);
            Vector3 right = Vector3.Cross (travelDirection, levelDirection);
            Vector3 up = Vector3.Cross (right, travelDirection);
            if (end.y > start.y)
                up = -up;
            Vector3 result = start + t * travelDirection;
            result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
            return result;
        }
    }

    void OnDrawGizmos ()
    {

        //Draw the parabola by sample a few times
        Gizmos.color = Color.red;
        Gizmos.DrawLine (transform.position, target.position);
        float count = 20;
        Vector3 lastP = transform.position;
        for (float i = 0; i < count + 1; i++) {
            Vector3 p = SampleParabola (transform.position, target.position, height, i / count);
            Gizmos.color = i % 2 == 0 ? Color.blue : Color.green;
            Gizmos.DrawLine (lastP, p);
            lastP = p;
        }
    }

    private void ResetShoot()
    {
        readyToShoot = true;
    }
    
    private void WaitToGo()
    {
        playerAggro = true;
    }
}
