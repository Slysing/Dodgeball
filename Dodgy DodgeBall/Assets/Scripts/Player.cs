using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Description: Handles the movement/catch of a player
/// Authors Leith Merrifield
/// Creation Date: 07/10/2019
/// Modified: 07/10/2019
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


    // Xbox Controller Settings
    [Header("Xbox Movement")]
    public bool m_useXboxController = true;
    private Vector3 m_moveInput_Xbox;
    private Vector3 m_moveVelocity_Xbox;
    private Rigidbody m_rigidBody_Xbox;
    public float m_moveSpeed_Xbox;


    public void Start()
    {
        m_rigidBody_Xbox = GetComponent<Rigidbody>();
        m_rb = transform.GetComponent<Rigidbody>();
        if(m_hand == null)
        {
            m_hand = transform.gameObject;
        }
        
    }

    // Fixed update is general used for anything physics related
    public void FixedUpdate()
    {
        var speed = m_speed * m_speedMultiplier * Time.deltaTime;
        var velocity = new Vector3();

        // Used for keyboard + mouse movement
        if (m_useXboxController == false)
        {
            // substitute the values in the getkey for the controller equivalent
            if (Input.GetKey(KeyCode.RightArrow))
            {
                velocity.x = 1 * speed;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                velocity.x = -1 * speed;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                velocity.z = 1 * speed;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                velocity.z = -1 * speed;
            }
        }
        
        

        // Used for xbox controller movement
        else if (m_useXboxController == true)
        {
            //Left Stick Movement
            m_moveInput_Xbox = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            m_moveVelocity_Xbox = m_moveInput_Xbox * m_moveSpeed_Xbox;

            //Right Stick Rotation
            Vector3 playerDirection = Vector3.right * Input.GetAxisRaw("RHorizontal") + Vector3.forward * Input.GetAxisRaw("RVertical");
            if (playerDirection.sqrMagnitude > 0.0f)
            {
                transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
            }
        }
        m_rb.velocity = velocity;

        m_rigidBody_Xbox.velocity = m_moveVelocity_Xbox;




        //This will also work, however it's not as specific as the above movement
        //var h = Input.GetAxisRaw("Horizontal");
        //var v = Input.GetAxisRaw("Vertical");

        //var speed = m_speed * m_speedMultiplier * Time.deltaTime;
        //var velocity = new Vector3(v * -speed, 0f, h * speed);
        //m_rb.velocity = velocity;
    }


public void Update()
    {
       

        
        // Casts to from the mouse position to an object
        // if hit then grab that position and create a direction vector
        // from the player
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            transform.LookAt(hit.point);
            transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0f);
            m_lastLookDirection = hit.point - transform.position;
        }

        // only runs if a ball is being held and cooldown is off
        // Used to continually change the ball position when the player
        // moves or rotates
        if (m_ball != null && !m_onCoolDown)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                m_ball.gameObject.transform.position = m_hand.transform.position;
                m_ball.gameObject.transform.rotation = m_hand.transform.rotation;
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                m_holdingBall = false;
                m_ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                m_ball = null;
            }
        }

        // if a ball is being held and space is pushed run the throw coroutine
        if (m_holdingBall && m_ball != null)
        {
            if (!m_onCoolDown && Input.GetKey(KeyCode.Space))
            {
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

    // thowing function with a cooldown
    IEnumerator Throw()
    {
        m_onCoolDown = true;
        var rb = m_ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;        
        var force = m_lastLookDirection.normalized * m_throwStrength;
        rb.AddForce(force,ForceMode.Impulse);

        yield return new WaitForSeconds(.5f);
        m_ball = null;
        m_holdingBall = false;

        m_onCoolDown = false;
    }
}
