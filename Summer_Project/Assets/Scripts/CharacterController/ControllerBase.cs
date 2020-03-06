/*
 * UCSC Level Design Toolkit
 * 
 * Aidan Kennell
 * akennell94@gmail.com
 * 
 * This script is a framework for any controller be it enemy contorllers, character controllers, ect.
 * 
 * Released under MIT Open Source License
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBase : MonoBehaviour
{
#region Member Variables
    protected VirtualInput input;
    protected Animator animator;
    protected const float FORCE_MOD = 100.0f;
    protected float distanceToGrounded;

    [Header("Ground Movement")]
    public float rotationSpeed;
    [Range(20.0f, 180.0f)]
    public float instantTurnAngle;
    public float maxSpeed;
    [Range(0.0f, 1.0f)]
    public float stepUpHeight;

    [Header("Jump Settings")]
    public bool canDoubleJump;
    public LayerMask groundLayer;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    public float gravity;
    [Range(0.0f, 1.0f)]
    public float airControl;
    [Range(1.0f, 100.0f)]
    public float forwardJumpForce;
    [Range(0.0f, 1.0f)]
    protected bool hasDoubleJumped;
    public bool fallDamage;

    [Header("Attack Settings")]
    public float timeBetweenAttacks;
    public float attackTranslationMultiplier;
    protected float attackTimer;
    #endregion

    public ControllerBase()
    {
    }

    public virtual void handleJump()
    {
    }

    public virtual void handleMove()
    {
    }

    public virtual void handleAttack()
    {
    }

    public virtual void updateStates()
    {
    }

    public virtual void freezeMovement()
    {
        input.freezeMovement = true;
    }

    public virtual void unFreezeMovement()
    {
        input.freezeMovement = false;
    }
}
