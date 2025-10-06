using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class BattleProximityController : MonoBehaviour
{
    [Header("Animators")]
    public Animator ghostAnimator;
    public Animator adventurerAnimator;

    [Header("Vuforia ImageTargets (ObserverBehaviour)")]
    public ObserverBehaviour ghostObserver;
    public ObserverBehaviour adventurerObserver;

    [Header("Transforms (optional, auto-filled from observers)")]
    public Transform ghostTransform;
    public Transform adventurerTransform;

    [Header("Settings")]
    [Tooltip("Distance (in world units) below which characters consider each-other in range to attack")]
    public float attackDistance = 1.0f;

    [Tooltip("Seconds to wait after battle stops before sending ReturnToNormal trigger")]
    public float idleReturnDelay = 5.0f;

    [Tooltip("Bool parameter name used by both animators")]
    public string isAttackingParam = "isAttacking";

    [Tooltip("Trigger name on adventurer animator to return from idle_battle -> idle_normal")]
    public string returnToNormalTrigger = "ReturnToNormal";

    //internal state
    bool ghostVisible = false;
    bool adventurerVisible = false;
    bool inBattle = false;

    float idleTimer = 0f;
    bool timerRunning = false;

    void OnEnable()
    {
        if (ghostObserver != null) ghostObserver.OnTargetStatusChanged += OnGhostStatusChanged;
        if (adventurerObserver != null) adventurerObserver.OnTargetStatusChanged += OnAdventurerStatusChanged;

        if (ghostTransform == null && ghostObserver != null) ghostTransform = ghostObserver.transform;
        if (adventurerTransform == null && adventurerObserver != null) adventurerTransform = adventurerObserver.transform;
    }

    void OnDisable()
    {
        if (ghostObserver != null) ghostObserver.OnTargetStatusChanged -= OnGhostStatusChanged;
        if (adventurerObserver != null) adventurerObserver.OnTargetStatusChanged -= OnAdventurerStatusChanged;
    }

    void Update()
    {
        if (timerRunning)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleReturnDelay)
            {
                timerRunning = false;
                idleTimer = 0f;
                if (adventurerAnimator != null)
                    adventurerAnimator.SetTrigger(returnToNormalTrigger);
            }
        }
    }

    //vuforia callback
    private void OnGhostStatusChanged(ObserverBehaviour obs, TargetStatus status)
    {
        ghostVisible = IsStatusTracked(status);
        EvaluateBattle();
    }

    //vuforia callback
    private void OnAdventurerStatusChanged(ObserverBehaviour obs, TargetStatus status)
    {
        adventurerVisible = IsStatusTracked(status);
        EvaluateBattle();
    }


    private bool IsStatusTracked(TargetStatus status)
    {
        return status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;
    }


    private void EvaluateBattle()
    {
        bool closeEnough = false;

        if (ghostVisible && adventurerVisible && ghostTransform != null && adventurerTransform != null)
        {
            float d = Vector3.Distance(ghostTransform.position, adventurerTransform.position);
            closeEnough = d <= attackDistance;
        }

        SetBattleState(closeEnough);
    }

    private void SetBattleState(bool active)
    {
        if (active == inBattle) return; //no changes
        inBattle = active;
        Debug.Log("SetBattleState called. InBattle: " + inBattle);

        if (ghostAnimator != null) ghostAnimator.SetBool(isAttackingParam, active);
        if (adventurerAnimator != null) adventurerAnimator.SetBool(isAttackingParam, active);

        if (active)
        {
            //stop timer
            timerRunning = false;
            idleTimer = 0f;
        }
        else
        {
            //fight is over, start timer
            timerRunning = true;
            idleTimer = 0f;
        }
    }
}
