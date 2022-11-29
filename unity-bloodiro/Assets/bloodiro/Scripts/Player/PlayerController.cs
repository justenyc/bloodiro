using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterController m_characterController;

    [System.Serializable]
    class Properties
    {
        public bool applyGravity = true;
        public float gravity = -9.81f;
        public float terminalVelocity = -2f;
        public float moveSpeed = 10;
        public float parkourDistance = 0.6f;
        public LayerMask groundedLayers;
        public LayerMask parkourLayers;
    }
    [SerializeField] Properties m_properties;

    [System.Serializable]
    class PropertyCurves
    {
        public AnimationCurve gravityVertCurve;
        public AnimationCurve gravityHorzCurve;
        public AnimationCurve moveAcceleration;
    }
    [SerializeField] PropertyCurves m_propertyCurves;

    [System.Serializable]
    class States
    {
        public bool grounded;
        public bool nearLadder;
    }
    [SerializeField] States m_states;

    [System.Serializable]
    class Modifiers
    {
        public Vector2 moveVector;
        public float parkourTime = 0.5f;
        public float moveAccelerationTime;
        public float gravityVertCurveTime;
        public float gravityHorzCurveTime;
    }
    [SerializeField] Modifiers m_modifiers;

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyGravity();
        Move();
        CheckForGround();
        ParkourCheck();
    }

    #region Logic
    float ApplyGravity()
    {
        if(!m_properties.applyGravity)
        {
            return 0;
        }
        return m_properties.gravity;
    }

    void GroundedMovementCurveLerp()
    {
        if (Mathf.Abs(m_modifiers.moveVector.x) > 0)
        {
            m_modifiers.moveAccelerationTime += Time.fixedDeltaTime;
            m_modifiers.moveAccelerationTime = Mathf.Clamp01(m_modifiers.moveAccelerationTime);
        }
        else
        {
            m_modifiers.moveAccelerationTime = 0;
        }
    }

    void GravityCurveLerp()
    {
        if (!m_states.grounded)
        {
            m_modifiers.gravityVertCurveTime += Time.fixedDeltaTime;
            m_modifiers.gravityVertCurveTime = Mathf.Clamp01(m_modifiers.gravityVertCurveTime);
            
            m_modifiers.gravityHorzCurveTime += Time.fixedDeltaTime;
            m_modifiers.gravityHorzCurveTime = Mathf.Clamp01(m_modifiers.gravityHorzCurveTime);
        }
        else
        {
            m_modifiers.gravityVertCurveTime = 0;
            m_modifiers.gravityHorzCurveTime = 0;
        }
    }

    void Move()
    {
        GroundedMovementCurveLerp();
        GravityCurveLerp();

       Vector2 moveVector;
        if(m_states.nearLadder)
        {
            moveVector = new Vector2(
                m_modifiers.moveVector.x * m_properties.moveSpeed * Time.fixedDeltaTime * m_propertyCurves.moveAcceleration.Evaluate(m_modifiers.moveAccelerationTime), 
                m_modifiers.moveVector.y * m_properties.moveSpeed * Time.fixedDeltaTime);
            m_characterController.Move(moveVector);
            return;
        }

        if (m_states.grounded)
        {
            moveVector = new Vector2(
                m_modifiers.moveVector.x * m_properties.moveSpeed * Time.fixedDeltaTime * m_propertyCurves.moveAcceleration.Evaluate(m_modifiers.moveAccelerationTime),
                ApplyGravity() * Time.fixedDeltaTime);
            m_characterController.Move(moveVector);
        }
        else
        {
            moveVector = new Vector2(
                m_characterController.velocity.x * m_propertyCurves.gravityHorzCurve.Evaluate(m_modifiers.gravityHorzCurveTime) * Time.fixedDeltaTime,
                (ApplyGravity() + m_properties.terminalVelocity * m_propertyCurves.gravityVertCurve.Evaluate(m_modifiers.gravityVertCurveTime)) * Time.fixedDeltaTime);
            m_characterController.Move(moveVector);

            Debug.Log(m_characterController.velocity.x * m_propertyCurves.gravityHorzCurve.Evaluate(m_modifiers.gravityHorzCurveTime) * Time.fixedDeltaTime);
            Debug.Log((ApplyGravity() + m_properties.terminalVelocity * m_propertyCurves.gravityVertCurve.Evaluate(m_modifiers.gravityVertCurveTime)) * Time.fixedDeltaTime);
        }
    }

    void CheckForGround()
    {
        RaycastHit hit;
        
        if (Physics.SphereCast(transform.position + transform.up, 1, -Vector3.up, out hit, 0.6f))
        {
            m_states.grounded = true;
            return;
        }
        m_states.grounded = false;
    }

    void ParkourCheck()
    {
        RaycastHit hit;
        if (m_states.grounded)
        {
            //Debug.DrawRay(transform.position, transform.up, Color.cyan);
            if (Physics.Raycast(transform.position, transform.right, out hit, m_properties.parkourDistance, m_properties.parkourLayers))
            {
                Debug.Log(hit.collider.name);
                StartCoroutine(DelayMoveForParkourTest(hit.point));
            }
        }

        if (m_states.nearLadder)
        {
            if (Physics.Raycast(transform.position, transform.up, out hit, m_properties.parkourDistance, m_properties.parkourLayers))
            {
                Debug.Log(hit.collider.name);
                StartCoroutine(DelayMoveForParkourTest(new Vector3(hit.point.x, hit.collider.transform.position.y, hit.point.z)));
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        m_states.nearLadder = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Reset();
    }

    private void Reset()
    {
        m_states.nearLadder = false;
        m_properties.applyGravity = true;
        m_characterController.enabled = true;
        this.enabled = true;
    }
    #endregion

    #region Player Input Listeners
    public void MoveInputListener(InputAction.CallbackContext ctx)
    {
        m_modifiers.moveVector = ctx.ReadValue<Vector2>();
        if(m_modifiers.moveVector.x > 0) 
        { 
            transform.rotation = Quaternion.Euler(Vector3.zero); 
        }
        else if(m_modifiers.moveVector.x < 0) 
        { 
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0)); 
        }
    }

    public void SlashInputListener(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
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

    #region Temp stuff
    IEnumerator DelayMoveForParkourTest(Vector3 basePosition)
    {
        m_characterController.enabled = false;
        this.enabled = false;

        yield return new WaitForSeconds(m_modifiers.parkourTime);

        transform.position = basePosition + Vector3.up;
        Reset();
    }
    #endregion
}
