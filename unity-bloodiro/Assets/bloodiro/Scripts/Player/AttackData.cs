using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quickjam.Player;

[CreateAssetMenu(fileName = "AttackData", menuName = "QuickJam/New Attack", order = 1)]
public class AttackData : ScriptableObject
{
    public enum Behaviour
    {
        Nothing,
        Move,
    }

    [System.Serializable]
    public class MoveBehaviourProperties
    {
        public bool useGravity = true;
        public float gravity = -9.81f;
        public float newMoveSpeed = 1;
        public AnimationCurve moveSpeedCurve;
    }

    public string id;
    public bool debugBreak = false;
    [Space(10)]
    public int cancelPriority;
    public int lengthInFrames;
    [Space(10)]
    public int activeStart;
    public int activeEnd;
    [Space(10)]
    public AnimationClip animationClip;
    public Behaviour behaviour;
    public MoveBehaviourProperties moveBehaviourProperties;

    public System.Action<PlayerController, PlayerAttack.PlayerAttackTimeTracking> GetBehaviour()
    {
        switch (behaviour)
        {
            case Behaviour.Move:
                return Move;

            default:
                return null;
        }
    }

    void Move(PlayerController playerController, PlayerAttack.PlayerAttackTimeTracking timeTracking)
    {
        if (moveBehaviourProperties.useGravity)
        {
            Vector3 moveVector = Vector3.zero;
            if (playerController.m_playerAnimator.transform.localScale.z > -1)
            {
                moveVector = new Vector3(
                        -1 * Time.fixedDeltaTime * moveBehaviourProperties.newMoveSpeed * moveBehaviourProperties.moveSpeedCurve.Evaluate(timeTracking.moveCurveTime),
                        moveBehaviourProperties.gravity * Time.fixedDeltaTime,
                        0);
                playerController.m_characterController.Move(moveVector);
                return;
            }
            moveVector = new Vector3(
                        1 * Time.fixedDeltaTime * moveBehaviourProperties.newMoveSpeed * moveBehaviourProperties.moveSpeedCurve.Evaluate(timeTracking.moveCurveTime),
                        moveBehaviourProperties.gravity * Time.fixedDeltaTime,
                        0);
            playerController.m_characterController.Move(moveVector);
            return;
        }
        else
        {
            if (playerController.m_playerAnimator.transform.localScale.z > -1)
            {
                playerController.m_characterController.Move(Vector3.left * Time.fixedDeltaTime * moveBehaviourProperties.newMoveSpeed * moveBehaviourProperties.moveSpeedCurve.Evaluate(timeTracking.moveCurveTime));
                return;
            }
            playerController.m_characterController.Move(Vector3.right * Time.fixedDeltaTime * moveBehaviourProperties.newMoveSpeed * moveBehaviourProperties.moveSpeedCurve.Evaluate(timeTracking.moveCurveTime));
            return;
        }
    }

    void Parry()
    {

    }
}