using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Description: Handles the movement/catch of a player (With keyboard and Controller)
/// Authors Boran Baykan, Leith Merrifield
/// Creation Date: 07/10/2019
/// Modified: 09/10/2019
///</summary>

public enum PLAYER_STATE
{
    PLAYER_IDLE,
    PLAYER_MOVING,
    PLAYER_DASH,
    PLAYER_HOLDING
}

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float m_speed = 5f;

    [Header("Catch and Throw")]
    public float m_pickupRadius = 5f;
    public float m_throwStrength = 5f;
    public GameObject m_hand = null;
    public GameObject m_catchRange = null;

    private float m_speedMultiplier = 20f;
    private bool m_holdingBall = false;
    private GameObject m_ball = null;
    private Rigidbody m_rb = null;
    private Vector3 m_lastLookDirection = Vector3.forward;
    private bool m_onCoolDown = false;
    private bool m_dashCooldown = false;

    // Xbox Controller Settings
    [Header("Xbox Movement")]
    public bool m_useXboxController;
    // Changes the character speed for keyboard and controller at the moment
    public float m_moveSpeed_Xbox;
    private Vector3 m_moveInput_Xbox;

    // Keyboard and Mouse movement Working
    [Header("Keyboard & Mouse Movement")]
    private Camera m_mainCamera;


    //Renderer 
    [Header("Renderer's")]
    public Renderer rightTriggerSphere;


    public void Start()
    {
        m_mainCamera = FindObjectOfType<Camera>();
        m_rb = transform.GetComponent<Rigidbody>();
        
        if (m_hand == null)
        {
            m_hand = transform.gameObject;
        }
    }

    // Fixed update is general used for anything physics related
    public void FixedUpdate()
    {
        //Rigidbody
        //m_rb.velocity = velocity;
        Vector3 moveVelocity = Vector3.zero;

        if (!m_useXboxController)
        {
            /*
             Movement with  [ W A S D ] for player 
            */
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");
            Vector3 moveInput = new Vector3(h, 0f, v);
            moveVelocity = moveInput * m_moveSpeed_Xbox;

            // Leith's Code // - Mouse movement for direction 
            /*
                Casts to from the mouse position to an object
                if hit then grab that position and create a direction vector
                from the player
             */
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            {
                transform.LookAt(hit.point);
                transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0f);
                m_lastLookDirection = hit.point - transform.position;
            }

        }
        else
        {
            /* 
                This movement works if you only want one type selected (E.g. only controller moving the character if ticked in public bool)
            */
            //Left Stick Movement
            m_moveInput_Xbox = new Vector3(Input.GetAxisRaw("HorizontalJoyStick"), 0f, Input.GetAxisRaw("VerticalJoyStick"));
            moveVelocity = m_moveInput_Xbox * m_moveSpeed_Xbox;

            //Right Stick Rotation
            Vector3 playerDirection = Vector3.right * Input.GetAxisRaw("RHorizontal") + Vector3.forward * Input.GetAxisRaw("RVertical");
            if (playerDirection.sqrMagnitude > 0.0f)
            {
                transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                m_lastLookDirection = playerDirection - transform.position;
            }
        }

        m_rb.velocity = moveVelocity * Time.fixedDeltaTime * 50.0f;
    }

    public void Update()
    {

        if((Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetButton("A Button")) && !m_dashCooldown)
        {
            StartCoroutine(Dash());
        }
        //Input Manager
        //float rightTrigger = Input.GetAxis("Right Trigger");
        // Changes the colour of a ball by pressing right trigger [ RT ] ---TESTING PURPOSE---
        //rightTriggerSphere.material.color = new Color(rightTrigger, rightTrigger, rightTrigger, rightTrigger);
        /*
         Only runs if a ball is being held and cooldown is off
         Used to continually change the ball position when the player moves or rotates
        */
        if (m_ball != null && !m_onCoolDown)
        {
            if ((Input.GetAxis("Right Trigger") > 0 && m_useXboxController) || Input.GetKey(KeyCode.Mouse0))
            {
                m_ball.gameObject.transform.position = m_hand.transform.position;
                m_ball.gameObject.transform.rotation = m_hand.transform.rotation;
                //Debug.Log("Right Bumper");
            }
            if ((Input.GetAxis("Right Trigger") == 0 && m_useXboxController) || Input.GetKeyUp(KeyCode.Mouse0))
            {
                
                m_holdingBall = false;
                m_ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                m_ball = null;
                //Debug.Log("Right Bumper | Button Up");
            }
        }

        // If a ball is being held and space bar is pushed, run the throw coroutine
        if (m_holdingBall && m_ball != null)
        {
            if (!m_onCoolDown && (Input.GetKey(KeyCode.Space) || Input.GetAxis("Left Trigger") > 0))
            {
                Debug.Log("Thrown the ball");
                Debug.Log(m_onCoolDown);
                StartCoroutine(Throw());
            }
        }
    }  

    public void SetBall(GameObject ball)
    {
            m_ball = ball;
            m_holdingBall = true;
    }

    // Thowing function with a cooldown
    IEnumerator Throw()
    {
        m_onCoolDown = true;
        var rb = m_ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;        
        var force = transform.forward * m_throwStrength;
        
        rb.AddForce(force,ForceMode.Impulse);

        yield return new WaitForSeconds(.5f);
        m_ball = null;
        m_holdingBall = false;

        m_onCoolDown = false;
    }

    IEnumerator Dash()
    {
            var x = 150.0f * Time.deltaTime;
            Debug.Log(x);
            m_moveSpeed_Xbox *= x;
            yield return new WaitForSeconds(.15f);
            m_moveSpeed_Xbox /= x;
            m_dashCooldown = true;

            yield return new WaitForSeconds(1f);
            m_dashCooldown = false;
    }
}
