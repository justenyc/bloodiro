using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterController m_characterController;

    [Header("Properties")]
    [SerializeField] bool applyGravity;
    [SerializeField] bool m_grounded;
    [SerializeField] float m_moveSpeed;
    [SerializeField] AnimationCurve m_moveAcceleration;

    [Header("Debug")]
    [SerializeField] Vector2 m_moveVector;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hi");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyGravity();
        Move();
    }

    #region Logic
    void ApplyGravity()
    {
        if(!applyGravity)
        {
            return;
        }
        Vector3 velocity = new Vector3(0, -9.81f, 0);
        m_characterController.Move(velocity * Time.fixedDeltaTime);
    }

    void Move()
    {
        Vector2 moveVector = new Vector2(m_moveVector.x * m_moveSpeed * Time.fixedDeltaTime, 0);
        m_characterController.Move(moveVector);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
    }
    #endregion

    #region Player Input Listeners
    public void MoveInputListener(InputAction.CallbackContext ctx)
    {
        m_moveVector = ctx.ReadValue<Vector2>();
    }

    public void SlashInputListener(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            Debug.Log("OnSlash() called");
        }
    }
    #endregion
}
