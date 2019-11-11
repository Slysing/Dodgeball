/* Player.cs
 * Authors: Boran Baykan, Leith Merrifield
 * Description: Handles the movement/catch of a player using keyboard or Controoler
 * Creation: 07/10/2019
 * Modifield: 11/11/2019
 */

using System.Collections;
using TMPro;
using UnityEngine;
using XboxCtrlrInput;

public enum PLAYER_STATE
{
    PLAYER_IDLE,
    PLAYER_MOVING,
    PLAYER_DASH,
    PLAYER_HOLDING
}

public enum PLAYER_TEAM
{
    TEAM_BLUE,
    TEAM_RED
}

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public bool m_isAlive = true;

    public PLAYER_TEAM m_currentTeam;

    [Header("Movement")]
    public float m_speed = 5f;

    public float m_dashSpeed = 5f;

    [Header("Catch and Throw")]
    public float m_pickupRadius = 5f;

    public float m_throwStrength = 5f;
    public GameObject m_hand = null;
    public GameObject m_catchRange = null;
    public GameObject m_fakeBallCollider = null;

    //private float m_speedMultiplier = 20f;
    [SerializeField] private bool m_holdingBall = false;

    private GameObject m_ball = null;
    private Rigidbody m_rb = null;
    private Vector3 m_lastLookDirection = Vector3.forward;
    private bool m_onCoolDown = false;
    private bool m_dashCooldown = false;

    // New Xbox Movement Input
    [Header("New Xbox Movement")]
    public bool m_useXboxController;

    public float m_moveSpeed_Xbox;
    private Vector3 m_moveInput_Xbox;
    public XboxController m_controller;

    public const float m_MaxTriggerScale = 1.21f;

    private Vector3 m_newPosition;

    private static bool m_didQueryNumOfCtrlrs = false;

    public Material matRed;
    public Material matGreen;
    public Material matBlue;
    public Material matYellow;

    // Keyboard and Mouse movement Working
    [Header("Keyboard & Mouse Movement")]
    private Camera m_mainCamera;

    // Ball
    public float m_timeLeft;

    public TextMeshProUGUI m_countDownText;
    public int m_countDownBall = 5;
    private bool m_countDownFlag = false;

    public float m_ballCountdown = 1.2f;
    private bool m_flashing = false;

    public Material m_redMaterial = null;
    public Material m_blueMaterial = null;
    public Material m_greenMaterial = null;

    public Animator playerAnim;
    public GameObject deathParticle;


    public void Start()
    {
        m_timeLeft = 5.0f;

     //  switch (m_controller)
     //  {
     //      case XboxController.First: GetComponent<Renderer>().material = matRed; break;
     //      case XboxController.Second: GetComponent<Renderer>().material = matBlue; break;
     //      case XboxController.Third: GetComponent<Renderer>().material = matGreen; break;
     //      case XboxController.Fourth: GetComponent<Renderer>().material = matYellow; break;
     //  }

        m_newPosition = transform.position;

        if (!m_didQueryNumOfCtrlrs)
        {
            m_didQueryNumOfCtrlrs = true;

            int queriedNumberOfCtrlrs = XCI.GetNumPluggedCtrlrs();
            if (queriedNumberOfCtrlrs == 1)
            {
                Debug.Log("Only " + queriedNumberOfCtrlrs + " Xbox controller plugged in.");
            }
            else if (queriedNumberOfCtrlrs == 0)
            {
                Debug.Log("No Xbox controllers plugged in!");
            }
            else
            {
                Debug.Log(queriedNumberOfCtrlrs + " Xbox controllers plugged in.");
            }

            XCI.DEBUG_LogControllerNames();
        }

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
            //m_moveInput_Xbox = new Vector3(Input.GetAxisRaw("P" + m_PlayerControllerNumber + " HorizontalJoyStick"), 0f, Input.GetAxisRaw("P" + m_PlayerControllerNumber + " VerticalJoyStick"));
            //moveVelocity = m_moveInput_Xbox * m_moveSpeed_Xbox;

            ////Right Stick Rotation
            //Vector3 playerDirection = Vector3.right * Input.GetAxisRaw("P" + m_PlayerControllerNumber + " HorizontalRightJoystick") + Vector3.forward * Input.GetAxisRaw("P" + m_PlayerControllerNumber + " VerticalRightJoystick");
            //if (playerDirection.sqrMagnitude > 0.0f)
            //{
            //    transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
            //    m_lastLookDirection = playerDirection - transform.position;
            //}

            // -New- Left Stick Movement
            m_newPosition = transform.position;
            float axisX = XCI.GetAxis(XboxAxis.LeftStickX, m_controller);
            float axisY = XCI.GetAxis(XboxAxis.LeftStickY, m_controller);

            m_moveInput_Xbox = new Vector3(XCI.GetAxis(XboxAxis.LeftStickX, m_controller), 0f, XCI.GetAxis(XboxAxis.LeftStickY, m_controller));
            moveVelocity = m_moveInput_Xbox * m_moveSpeed_Xbox;

            // New Right Stick Rotation
            m_newPosition = transform.position;
            axisX = XCI.GetAxis(XboxAxis.RightStickX, m_controller);
            axisY = XCI.GetAxis(XboxAxis.RightStickY, m_controller);

            Vector3 playerDirection = Vector3.right * axisX + Vector3.forward * axisY;
            if (playerDirection.sqrMagnitude > 0.0f)
            {
                transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                m_lastLookDirection = playerDirection - transform.position;
            }
            transform.position = m_newPosition;
        }

        if (RoundManager.m_isPlaying)
            m_rb.velocity = moveVelocity * Time.fixedDeltaTime * 50.0f;


        if (moveVelocity.magnitude > 0)
        {
            if (playerAnim != null)
            {
                playerAnim.SetBool("Walking", true);
                //Debug.Log("Walking");
            }
        }
        else
        {
            if (playerAnim != null)
            {
                playerAnim.SetBool("Walking", false);
                //Debug.Log("Walking");
            }
        }
    }

    public void Update()
    {
        if (m_holdingBall == true)
        {
            if (m_fakeBallCollider.activeSelf != true)
                m_fakeBallCollider.SetActive(true);
            m_timeLeft -= Time.deltaTime;
            m_moveSpeed_Xbox = 10.0f;

            if (!m_flashing)
            {
                StartCoroutine(FlashCountdown());
                StartCoroutine(FlashCoroutine());
                m_flashing = true;
            }

            if (!m_countDownFlag)
            {
                m_countDownText.gameObject.SetActive(true);
                StartCoroutine(BallCountDown());
                m_countDownFlag = true;
            }

        }
        else
        {
            Debug.Log("not active");
            m_countDownBall = 5;
            m_countDownText.gameObject.SetActive(false);
            if (m_countDownFlag)
            {
                StopCoroutine(BallCountDown());
                m_countDownFlag = false;
            }

            if (m_flashing)
            {
                StopCoroutine(FlashCoroutine());
                StopCoroutine(FlashCountdown());
                m_flashing = false; 
            }
            m_timeLeft = 5.0f;
            m_moveSpeed_Xbox = 15.0f;
            m_ballCountdown = 1.2f;
        }

        if ((Input.GetKeyDown(KeyCode.Space) || XCI.GetAxis(XboxAxis.LeftTrigger, m_controller) > 0) && !m_dashCooldown)
        {
            StartCoroutine(Dash());
            if (playerAnim != null)
            {
                playerAnim.SetTrigger("Dash");
                Debug.Log("Dash");
            }
        }
        /*
         Only runs if a ball is being held and cooldown is off
         Used to continually change the ball position when the player moves or rotates
        */
        if (m_ball != null && !m_onCoolDown)
        {
            if ((m_MaxTriggerScale * (1.0f - XCI.GetAxis(XboxAxis.RightTrigger, m_controller)) > 0 && m_useXboxController) || Input.GetKey(KeyCode.Mouse0))
            {
                m_ball.gameObject.transform.position = m_hand.transform.position;
                m_ball.gameObject.transform.rotation = m_hand.transform.rotation;
            }
            if ((m_MaxTriggerScale * (1.0f - XCI.GetAxis(XboxAxis.RightTrigger, m_controller)) == 0 && m_useXboxController) || Input.GetKeyUp(KeyCode.Mouse0))
            {
                StartCoroutine(Throw());
                if (playerAnim != null)
                {
                    playerAnim.SetTrigger("Throw");
                    Debug.Log("Throw");
                }
            }
        }
    }

    private void Death()
    {
        m_isAlive = false;
        m_catchRange.GetComponent<CatchCollide>().ResetCooldown();

        if (deathParticle != null)
        {
            Instantiate(deathParticle, transform.position + Vector3.up, transform.rotation);
        }

        m_holdingBall = false;
        m_onCoolDown = false;

        m_dashCooldown = false;
        m_moveSpeed_Xbox = m_speed;

        DropBall();
        gameObject.SetActive(false);
    }

    public void DropBall()
    {
        if (m_ball != null)
        {
            m_ball.tag = "Neutral Ball";
            m_ball.GetComponent<MeshRenderer>().material = m_greenMaterial;
            m_ball = null;
        }
    }

    public void OnCollisionEnter(Collision collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Blue Ball":
                BallCollide(PLAYER_TEAM.TEAM_BLUE);

                break;

            case "Red Ball":
                BallCollide(PLAYER_TEAM.TEAM_RED);
                break;
        }
    }

    private void BallCollide(PLAYER_TEAM team)
    {
        if (team != m_currentTeam)
        {
            Death();
        }
    }

    public void SetBall(GameObject ball)
    {
        m_ball = ball;
        m_holdingBall = true;

        if (m_holdingBall == true)
        {
            if (m_timeLeft < 0)
            {
                m_ballCountdown = 1.2f;
                m_ball = null;
                m_holdingBall = false;
                m_onCoolDown = false;

                ball.tag = "Neutral Ball";
                ball.GetComponent<MeshRenderer>().material = m_greenMaterial;
                ball.GetComponent<Rigidbody>().velocity = Vector3.zero;

                StartCoroutine(WaitPickUp());
            }
        }
    }

    // Thowing function with a cooldown
    private IEnumerator Throw()
    {
        m_onCoolDown = true;
        var rb = m_ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        var force = transform.forward * m_throwStrength;

        rb.AddForce(force, ForceMode.Impulse);

        yield return new WaitForSeconds(.5f);
        m_ball = null;
        m_holdingBall = false;

        m_onCoolDown = false;
    }

    private IEnumerator Dash()
    {
        m_moveSpeed_Xbox = m_dashSpeed;
        yield return new WaitForSeconds(.15f);
        m_moveSpeed_Xbox = m_speed;
        m_dashCooldown = true;

        yield return new WaitForSeconds(1f);
        m_dashCooldown = false;
    }

    private IEnumerator WaitPickUp()
    {
        yield return new WaitForSeconds(2.5f);
    }

    private IEnumerator FlashCoroutine()
    {
        while (m_holdingBall)
        {
            print(m_ballCountdown);
            // flashing code goes here
            Renderer renderer = m_ball.GetComponent<MeshRenderer>();
            Material mat = renderer.material;

            float emission = Mathf.PingPong(Time.time, m_ballCountdown);
            Color baseColor = Color.red; //Replace this with whatever you want for your base color at emission level '1'

            Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

            mat.SetColor("_EmissionColor", finalColor);


            yield return new WaitForSeconds(m_ballCountdown);
        }
                print("Flash"); 
    }

    private IEnumerator FlashCountdown()
    {
        while(true)
        {
            m_ballCountdown -= 0.2f;
            if (m_ballCountdown <= 0)
            {
                m_ballCountdown = 0.2f;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator BallCountDown()
    {
        while (m_holdingBall)
        {
            
            m_countDownText.text = m_countDownBall.ToString();
            if (m_countDownBall == 0)
            {
                Death();
                
                m_ball = null;
                m_holdingBall = false;

                Debug.Log("CountDown = 0");
                break;
            }
            yield return new WaitForSeconds(1.0f);
            m_countDownBall--;
        }
    }

    private void OnDrawGizmos()
    {
        switch (m_controller)
        {
            case XboxController.First: Gizmos.color = Color.red; break;
            case XboxController.Second: Gizmos.color = Color.blue; break;
            case XboxController.Third: Gizmos.color = Color.green; break;
            case XboxController.Fourth: Gizmos.color = Color.yellow; break;
            default: Gizmos.color = Color.white; break;
        }

        Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.5f);

        Gizmos.DrawCube(transform.position, new Vector3(1.2f, 1.2f, 1.2f));
    }
}