using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrigamiControllerData_StateName", menuName = "OrigamiLovers/Create new Origami Controller Data", order = 1)]
public class OrigamiControllerStateData : ScriptableObject
{
    [Header("MOVEMENT")]
    [Tooltip("The force that we apply to the movement")]
    public float moveSpeed = 5;
    [Tooltip("The upper limit of the Move Velocity")]
    public float maxMoveVelocity = 10;

    [Header("JUMP")]
    [Tooltip("The force that we apply to the Jump")]
    public float jumpForce = 100;
    [Tooltip("The upper limit of the Jump Velocity")]
    public Vector2 maxJumpVelocity = new Vector2(-10, 5);

    [Header("GRAVITY")]
    [Tooltip("Curve that controls the gravity")]
    public AnimationCurve gravityCurve = AnimationCurve.EaseInOut(0,0,1,1);
    [Tooltip("Multiplier for the gravityCurve")]
    public float gravityStrengthMultiplier = 1;
    [Tooltip("Speed in which the gravityCurve is played")]
    public float gravityCurveSpeed = 1;

    [Header("DRAG")]
    [Tooltip("Drag we apply on Ground")]
    public float baseDrag = 5;
    [Tooltip("Drag we apply in air")]
    public float airDrag = 1;

    
}
