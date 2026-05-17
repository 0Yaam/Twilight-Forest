using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }
    
    private PlayerControls playerControls;
    private float timeBetweenAttacks;
    private bool attackButtonDown,isAttacking = false;
    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Disable();
        }
    }
    private void Start()
    {
        playerControls.Combat.Attack.started += OnAttackStarted;
        playerControls.Combat.Attack.canceled += OnAttackCanceled;
        AttackCooldown();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (playerControls != null)
        {
            playerControls.Combat.Attack.started -= OnAttackStarted;
            playerControls.Combat.Attack.canceled -= OnAttackCanceled;
            playerControls.Dispose();
        }
    }
    private void Update()
    {
        Attack();
    }
    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        AttackCooldown();
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
    }
    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
    }
    private void AttackCooldown()
    {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }
    private IEnumerator TimeBetweenAttacksRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }
    private void StartAttacking()
    {
        attackButtonDown = true;
    }
    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void OnAttackStarted(InputAction.CallbackContext context)
    {
        StartAttacking();
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        StopAttacking();
    }

    private void Attack()
    {
        if(attackButtonDown && !isAttacking && CurrentActiveWeapon)
        {
            AttackCooldown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }
}
