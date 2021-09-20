using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller2 : MonoBehaviour
{
    private Player2Controls _controls;
    [SerializeField] private Transform playerCursor;
    
    // Player Movement
    [SerializeField] private Vector2 move;
    public float movementSpeed = 10f;

    private void Awake()
    {
        _controls = new Player2Controls();
        _controls.Player2.XButton.performed += ctx => XAttack();
        _controls.Player2.YButton.performed += ctx => YAttack();
        _controls.Player2.AButton.performed += ctx => AButton();
        _controls.Player2.BButon.performed += ctx => BButton();
        
        _controls.Player2.LStick.performed += ctx => move = ctx.ReadValue<Vector2>();
        _controls.Player2.LStick.performed += ctx => move = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 m = new Vector2(move.x, move.y) * Time.deltaTime * movementSpeed;
        transform.Translate(m, Space.World);
    }

    private void  XAttack()
    {
       Player2Manager.instance.XAttack();
    }

    private void YAttack()
    {
        Player2Manager.instance.YAttack();
    }

    private void AButton()
    {
        Player2Manager.instance.AButton();
    }

    private void BButton()
    {
        Player2Manager.instance.BButton();
    }

    private void OnEnable()
    {
        _controls.Player2.Enable();
    }

    private void OnDisable()
    {
        _controls.Player2.Disable();
    }
}
