using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("John Lemmon Movement Settings")]

    /* turnSpeed <float>
    * Angle, in Rad, you want John to turn per second.
    * 
    */
    public float turnSpeed = 10f;

    /* Unity Internal Naming conventions:
    * All variables use camelCase: horizontal, vertical;
    * NON-PUBLIC MEMBER VARIABLES: m_<PascalCase>: m_Movement, m_Score;
    *
    * Member Variables: variables that belongs to a class
    * rather than a specific method (variávels de classe);
    */
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    Animator m_Animator;
    Rigidbody m_RigidBody;
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    private const string IS_WALKING = "IsWalking";

    // Start is called before the first frame update
    void Start() {
        // GetComponent is a generic method. That means, it has two different sets
        // of parameters: normal parameters and type parameters;
        m_Animator = GetComponent<Animator>();
        m_RigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate() {
        // move player
        float horizontal = Input.GetAxis(HORIZONTAL);
        float vertical  = Input.GetAxis(VERTICAL);
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        // animate player
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool(IS_WALKING, isWalking);        

        // Set John lemmon direction
        Vector3 desiredFoward = Vector3.RotateTowards(
            transform.forward,
            m_Movement,
            turnSpeed * Time.deltaTime, // Time.deltaTime = time since previous frame
            0f
        );
        m_Rotation = Quaternion.LookRotation(desiredFoward);
    }

    private void OnAnimatorMove() {
        m_RigidBody.MovePosition(m_RigidBody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_RigidBody.MoveRotation(m_Rotation);
    }
}
