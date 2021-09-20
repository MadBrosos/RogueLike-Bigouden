using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Manager : MonoBehaviour
{
    
    // Utilities
    public static Player2Manager instance;
    private Rigidbody2D rb;
    
    // X Attack
    public Transform attackPointX;
    public float attackRangeX = 0.7f;
    public bool readyToAttackX;
    public float xAttackCd = 0.2f; // A MODIFIER
    
    // Y Attack
    public Transform attackPointY;
    public float attackRangeY;
    public bool readyToAttackY;
    public float yAttackCd;
    
    // Dash
    private bool isDashing;
    public bool readyToDash;
    private float dashDuration = .2f;
    private float dashCooldown = 2f;
    private float dashForce = 15f;
    
    private void Awake()
    {
        if (instance != null)
            return;
        
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        readyToDash = true;
        readyToAttackX = true;
        readyToAttackY = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void XAttack()
    {
        Debug.Log("X Button Pressed!");
    }

    public void YAttack()
    {
        Debug.Log("Y Button Pressed!");
    }

    public void AButton()
    {
        Debug.Log("A Button Pressed!");
    }

    public void BButton()
    {
        Debug.Log("B Button Pressed!");
    }
    
    private void ResetX()
    {
        readyToAttackX = true;
    }

    private void ResetY()
    {
        readyToAttackY = true;
    }

    private void ResetDash()
    {
        readyToDash = true;
    }
}
