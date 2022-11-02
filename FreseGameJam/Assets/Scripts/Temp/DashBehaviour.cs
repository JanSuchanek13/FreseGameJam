using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DashBehaviour : CharacterBehaviours
{
    [Header("DASH")]
    public float dashTime = 0.1f;
    public float dashSpeed = 50;

    [Header("PUSH")]
    public bool pushTargets = true;
    public float pushForce = 10;
    public float pushRadius = 1.5f;
    public LayerMask pushLayer;

    [Header("DATA")]
    public bool isDashing = false;
    public Vector3 dashDirection;
    private float dashTimer;

    [Header("REFERENCES")]
    private CharacterController characterController;
    [Tooltip("Reference to the PlayerInput Action Mapping")]
    private PlayerInput playerInput;

    public delegate void DashEvent(Vector3 _pos, Vector3 _direction);
    public static DashEvent onDashStart_Event;
    public static DashEvent onDashEnd_Event;

    public void Awake()
    {
        characterController = this.GetComponent<CharacterController>();
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        ResetScript();
        playerInput.Enable();
    }

    private void OnDisable()
    {

        playerInput.Disable();
    }

    public void ResetScript()
    {
        dashTimer = 0;
        isDashing = false;
    }

    public void Update()
    {
        if (isDashing)
            Dash();
        else
            GetPlayerInput();
    }

    /// <summary>
    /// We get the Player Input
    /// </summary>
    public void GetPlayerInput()
    {
        dashDirection = new Vector3(playerInput.CharacterControls.Move.ReadValue<Vector2>().x, 0f, playerInput.CharacterControls.Move.ReadValue<Vector2>().y).normalized;

        if (playerInput.CharacterControls.Jump.ReadValue<float>() != 0)
        {
            StartDash();
        }
    }

    /// <summary>
    /// We start the Dash
    /// </summary>
    public void StartDash()
    {
        // We modify the Variables
        isDashing = true;
        dashTimer = dashTime;

        if(dashDirection == Vector3.zero)
        {
            dashDirection = Vector3.forward;//forward muss Kamera abhängig sein
        }

        // We send an Event when the Dash Starts
        if (onDashStart_Event != null)
            onDashStart_Event(transform.position, dashDirection);
    }

    /// <summary>
    /// We do the Dash
    /// </summary>
    public void Dash()
    {
        characterController.Move((dashDirection * dashSpeed) * Time.deltaTime);
        PushTargets();
        DashTimer();
    }

    public void PushTargets()
    {
        if (!pushTargets)
            return;

        Collider[] allObjects = Physics.OverlapSphere(transform.position, pushRadius, pushLayer);   //all Objects in explosion Range
        foreach (Collider j in allObjects)
        {
            Pushable rig = j.GetComponent<Pushable>();
            if (rig != null)
            {
                Debug.Log("pushing Targets");
                rig.PushObject(this.transform, pushForce);
            }
        }
    }

    /// <summary>
    /// We count down the Dash Timer
    /// </summary>
    public void DashTimer()
    {
        // If we are not dashing we ignore this
        if (!isDashing)
            return;

        // We end the Dash or continue the Timer
        if (dashTimer <= 0)
            EndDash();
        else
            dashTimer -= Time.deltaTime;
    }

    /// <summary>
    /// We end the Dash
    /// </summary>
    public void EndDash()
    {
        // We reset the variables
        ResetScript();

        // We send an Event when the Dash End
        if (onDashEnd_Event != null)
            onDashEnd_Event(transform.position, dashDirection);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, dashDirection);
    }
}
