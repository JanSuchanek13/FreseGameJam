using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtention : CinemachineExtension
{
    public float horizontalSpeed = 10f;
    public float verticalSpeed = 10f;
    public float clampAngle = 80f;
    public float controllerMultiplier = 5;

    private Vector3 startingRotation;
    private InputHandler playerInput;

    protected override void Awake()
    {
        playerInput = GetComponentInParent<InputHandler>();
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                Vector2 deltaInput = playerInput.mouseValue;
                if (playerInput.controlType == InputHandler.ControlType.Controller)
                {
                    deltaInput *= controllerMultiplier;
                }
                startingRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
            }
        }
        //throw new System.NotImplementedException();
    }

    
}
