using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterController m_characterController;

    [Header("Properties")]
    [SerializeField] bool m_applyGravity;
    [SerializeField] float m_gravity;
    [SerializeField] float m_moveSpeed;
    [SerializeField] float m_parkourDistance;
    [SerializeField] AnimationCurve m_moveAcceleration;
    [SerializeField] LayerMask m_groundedLayers;
    [SerializeField] LayerMask m_parkourLayers;

    [Header("States")]
    [SerializeField] bool m_grounded;
    [SerializeField] bool m_nearLadder;

    [Header("Debug")]
    [SerializeField] Vector2 m_moveVector;
    [SerializeField] float m_parkourTime = 0.5f;

    private void Start()
    {
        
    }

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
        if(!m_applyGravity)
        {
            return 0;
        }
        return m_gravity;
    }

    void Move()
    {
        Vector2 moveVector;
        if (m_nearLadder)
        {
            moveVector = new Vector2(m_moveVector.x * m_moveSpeed * Time.fixedDeltaTime, m_moveVector.y * m_moveSpeed * Time.fixedDeltaTime);
            m_characterController.Move(moveVector);
            return;
        }
        moveVector = new Vector2(m_moveVector.x * m_moveSpeed * Time.fixedDeltaTime, ApplyGravity() * Time.fixedDeltaTime);
        m_characterController.Move(moveVector);
    }

    void CheckForGround()
    {
        RaycastHit hit;
        //Debug.DrawRay(transform.position, -Vector3.up , Color.magenta);
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 0.6f, m_groundedLayers))
        {
            m_grounded = true;
            return;
        }
        m_grounded = false;
    }

    void ParkourCheck()
    {
        RaycastHit hit;
        if (m_grounded)
        {
            //Debug.DrawRay(transform.position, transform.up, Color.cyan);
            if (Physics.Raycast(transform.position, transform.right, out hit, m_parkourDistance, m_parkourLayers))
            {
                Debug.Log(hit.collider.name);
                StartCoroutine(DelayMoveForParkourTest(hit.point));
            }
        }

        if(m_nearLadder)
        {
            if (Physics.Raycast(transform.position, transform.up, out hit, m_parkourDistance, m_parkourLayers))
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
        m_nearLadder = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Reset();
    }

    private void Reset()
    {
        m_nearLadder = false;
        m_applyGravity = true;
        m_characterController.enabled = true;
        this.enabled = true;
    }
    #endregion

    #region Player Input Listeners
    public void MoveInputListener(InputAction.CallbackContext ctx)
    {
        m_moveVector = ctx.ReadValue<Vector2>();
        if(m_moveVector.x > 0) 
        { 
            transform.rotation = Quaternion.Euler(Vector3.zero); 
        }
        else if(m_moveVector.x < 0) 
        { 
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0)); 
        }
    }

    public void SlashInputListener(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            transform.position = Vector3.zero;
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

        yield return new WaitForSeconds(m_parkourTime);

        transform.position = basePosition + Vector3.up;
        Reset();
    }
    #endregion
}
