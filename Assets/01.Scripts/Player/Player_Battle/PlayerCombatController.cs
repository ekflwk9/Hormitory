using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Component;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCombatController : BasePlayerController
{
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private float rollSpeed = 5f;
    [SerializeField] private float delay = 1f;
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float camFOV;
    [SerializeField] private float bonusFOV = 15f;
    
    private bool isRolling;
    protected override void Awake()
    {
        base.Awake();
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        
    }

    private void Start()
    {
        mainCamera = Camera.main;
        camFOV = mainCamera.fieldOfView;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
    
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.F))
        {
            if(!isRolling)
                StartCoroutine(Roll());
        }
    }

    private IEnumerator Roll()
    {
        isRolling = true;
        float elapsed = 0f;
        
        mainCamera.fieldOfView = camFOV - bonusFOV;
        moveInput = playerActions.Player.Move.ReadValue<Vector2>();
        Vector3 direction = (moveInput.y * transform.forward) + (moveInput.x * transform.right).normalized;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            characterController.Move(direction * rollSpeed * Time.deltaTime);
            yield return null;
        }
        
        mainCamera.fieldOfView = camFOV;
        yield return new WaitForSeconds(delay);
        isRolling = false;
    }
}
