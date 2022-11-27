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
    [SerializeField] LayerMask m_groundedLayers;
    [SerializeField] LayerMask m_parkourLayers;

    [Header("Debug")]
    [SerializeField] Vector2 m_moveVector;

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyGravity();
        Move();
        CheckForGround();
        ParkourCheck();
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

    void CheckForGround()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, -Vector3.up , Color.magenta);
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, m_groundedLayers))
        {
            Debug.Log(hit.collider.name);
        }
    }

    void ParkourCheck()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.right, Color.cyan);
        if (Physics.Raycast(transform.position, Vector3.right, out hit, m_parkourLayers))
        {
            Debug.Log(hit.collider.name);
        }
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

    public void ThrustInputListener(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("OnThrust() called");
        }
    }

    public void DodgeInputListener(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("OnDodge() called");
        }
    }

    public void InteractInputListener(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("OnInteract() called");
        }
    }

    public void GunInputListener(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("OnGun() called");
        }
    }

    public void ParryInputListener(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("OnParry() called");
        }
    }

    public void HealInputListener(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("OnHeal() called");
        }
    }
    #endregion
}
