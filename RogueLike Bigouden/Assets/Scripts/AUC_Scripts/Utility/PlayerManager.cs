using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Utilities
    public static PlayerManager instance;
    private Rigidbody2D rb;
    
    // X Attack
    public Transform attackPointX;
    public float attackRangeX = 0.7f;
    public bool readyToAttackX;
    public float xAttackCd = 0.2f; // A MODIFIER
    
    // Y Attack
    public Transform attackPointY;
    public float attackRangeY = 2f;
    public bool readyToAttackY;
    public float yAttackCd = 0.2f; // A MODIFER
    
    // Dash
    private bool isDashing;
    public bool readyToDash;
    private float dashDuration = .2f;
    private float dashCooldown = 2f;
    private float dashForce = 15f;
    
    
    // Player Stats
    [SerializeField] private float life = 100f;

    private void Awake()
    {
        if (instance != null)
            return;
        
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        readyToDash = true;
        readyToAttackX = true;
        readyToAttackY = true;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void XAttack()
    {
        readyToAttackX = false;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPointX.position, attackRangeX);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name + " with the little one");
        }
        Invoke(nameof(ResetX), xAttackCd);
    }

    public void YAttack()
    {
        readyToAttackY = false;
        Collider2D[] hitEnemiesY = Physics2D.OverlapCircleAll(attackPointY.position, attackRangeY);
        foreach (Collider2D enemy in hitEnemiesY)
        {
            Debug.Log("We hit " + enemy.name + " with the big one");
        }
        Invoke(nameof(ResetY), yAttackCd);
    }

    public void Dash()
    {
        rb.velocity *= dashForce;
        readyToDash = false;
        Invoke(nameof(StopDash), dashDuration);
        Invoke(nameof(ResetDash), dashCooldown);
    }

    private void StopDash()
    {
        rb.velocity = Vector2.zero; 
    }
    private void ResetDash()
    {
        readyToDash = true;
    }

    private void ResetX()
    {
        readyToAttackX = true;
    }

    private void ResetY()
    {
        readyToAttackY = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPointX.position, attackRangeX);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(attackPointY.position, attackRangeY);
    }
}
