//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class MainCharacterMovement : MonoBehaviour
{


    private ThirdPersonInputActionsAsset playerActionAsset;
    private InputAction move;
    private InputAction lookInput;

    private InputAction dodge;

    private Rigidbody rb;


    [SerializeField]
    private float acceleration = 500f;
    [SerializeField]
    private float maxSpeed = 5f;
    [SerializeField]
    private float jumpHeight = 5f;
    [SerializeField] private float routationSensitivity = 10f;

    private Camera cam;
    [SerializeField]
    private VariableJoystick movementJoystick;
    [SerializeField]
    private VariableJoystick playerRotJoystick;

    [SerializeField] private bool isNewInput;
    private bool isAndroid;

    private Vector3 moveDir;
    private Vector2 lookDir;
    Quaternion currentRotation;


    private Animator animator;

    private PlayerStats playerStats;


    [HideInInspector] public Vector3 enemyPosition;
    private Vector3 enemyImpact;
    private bool isPlayerDamaged;
    [SerializeField] private float dodgePower;
    bool isInitalDodgeDirectionSet;
    Vector3 dodgeDirection = Vector3.zero;

    bool canPlayerPassObstacle = true;
    Vector3 obstacleInFrontOfPlayer = Vector3.zero;

    bool isPlayerDodge;
    bool isPlayerReadyToDodge;



    private void Awake()
    {

        rb = this.GetComponent<Rigidbody>();
        playerActionAsset = new ThirdPersonInputActionsAsset();
        if (this.GetComponent<Animator>() != null)
            animator = this.GetComponentInChildren<Animator>();

        cam = Camera.main;

        playerStats = this.GetComponent<PlayerStats>();

    }


    void Start()
    {
        if (playerStats != null)
        {
            maxSpeed = playerStats.playerSpeed;
            jumpHeight = playerStats.playerJumpHeight;

            StartCoroutine(CheckAndroidControl());

            //dodgePower = playerStats.dodgePower;

        }

    }




    private void OnEnable()
    {
        move = playerActionAsset.Player.Move;
        lookInput = playerActionAsset.Player.Look;
        dodge = playerActionAsset.Player.Dodge;
        dodge.performed += ctx => StartCoroutine(DoDodge(0.5f));
        playerActionAsset.Enable();

    }

    private void OnDisable()
    {
        playerActionAsset.Disable();

    }

    private void FixedUpdate()
    {


        //cek apakah ada tumbukan dari enemy
        enemyPosition = playerStats.enemyPosition;

        if (playerStats.isPlayerDamaged)
        {
            enemyImpact = (this.transform.position - enemyPosition).normalized * 3;
            enemyImpact.y = 0;
        }
        else
        {
            enemyImpact = Vector3.Lerp(enemyImpact, Vector3.zero, 0.1f);
        }


        //Menggerakan player
        if (isNewInput)
        {
            if (!isAndroid)
            {

                moveDir.x += (move.ReadValue<Vector2>().x + enemyImpact.x) * acceleration;
                moveDir.z += (move.ReadValue<Vector2>().y + enemyImpact.y) * acceleration;

            }
            else
            {
                moveDir.x += (movementJoystick.Horizontal + enemyImpact.x) * acceleration;
                moveDir.z += (movementJoystick.Vertical + enemyImpact.y) * acceleration;

            }

        }
        else
        {
            if (!isAndroid)
            {
                moveDir.x += (Input.GetAxis("Horizontal") + enemyImpact.x) * acceleration;
                moveDir.z += (Input.GetAxis("Vertical") + enemyImpact.y) * acceleration;
            }
            else
            {
                moveDir.x += (movementJoystick.Horizontal + enemyImpact.x) * acceleration;
                moveDir.z += (movementJoystick.Vertical + enemyImpact.y) * acceleration;


            }
        }

        if (this.GetComponent<PlayerStats>() != null)
        {


            //fitur dodge hanya bisa dipakai jika player menggerakan joystick/control movement
            if (moveDir.sqrMagnitude >= 500f)
                isPlayerReadyToDodge = true;
            else
                isPlayerReadyToDodge = false;



            if (isPlayerDodge)
            {
                PlayerDodge();
            }
            else
            {
                //menggerakan player
                rb.GetComponent<Collider>().isTrigger = false;
                rb.isKinematic = false;
                canPlayerPassObstacle = true;

                rb.AddForce(moveDir, ForceMode.Acceleration);
                isInitalDodgeDirectionSet = false;
            }
        }
        else
        {
            //menggerakan player walaupun tanpa playerStats
            rb.AddForce(moveDir, ForceMode.Acceleration);
        }

        //Membatasi kecepatan player agar tidak terlalu tinggi
        moveDir = Vector3.zero;
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;



        //mengatur putaran player
        LookAt();
    }


    private void PlayerDodge()
    {
        enemyImpact = Vector3.zero;

        rb.isKinematic = true;
        rb.GetComponent<Collider>().isTrigger = true;

        //setup arah dodge dan apakah dodge dapat menembus obstacle atau tidak. Jika obstacle terlalu panjang maka tidak akan ditembus
        if (!isInitalDodgeDirectionSet)
        {
            float x;
            float z;
            //untuk kontrol PC
            if (!isAndroid)
            {
                x = transform.localPosition.x + Input.GetAxis("Horizontal") * dodgePower;
                z = transform.localPosition.z + Input.GetAxis("Vertical") * dodgePower;

            }
            else
            {
                x = transform.localPosition.x + movementJoystick.Horizontal * dodgePower;
                z = transform.localPosition.z + movementJoystick.Vertical * dodgePower;

            }

            dodgeDirection = new Vector3(x, transform.localPosition.y, z);
            isInitalDodgeDirectionSet = true;

            //cek apakah obstacle di depan player dapat ditembus?
            Vector3 boxSize = new Vector3(1f, 1f, 1f);
            Collider[] hitColliders = Physics.OverlapBox(dodgeDirection, boxSize, Quaternion.identity, LayerMask.GetMask("Obstacles")); // mencari semua collider dari objek static yang overlap dengan kotak di dodgeDirection
            if (hitColliders.Length > 0)
            {
                RaycastHit playerHit;
                if (Physics.Raycast(transform.localPosition, (dodgeDirection - transform.position).normalized, out playerHit, 100f, LayerMask.GetMask("Obstacles")))
                {
                    obstacleInFrontOfPlayer = playerHit.point;
                    canPlayerPassObstacle = false;
                }
            }
        }

        //batalkan dodge jika obstacle tidak bisa dilewati
        if (!canPlayerPassObstacle)
        {
            if (Vector3.Distance(transform.localPosition, obstacleInFrontOfPlayer) <= 1f)
            {
                rb.GetComponent<Collider>().isTrigger = false;
                rb.isKinematic = false;
                rb.transform.position = obstacleInFrontOfPlayer;
                isPlayerDodge = false;
            }
        }

        //lakukan dodge ke arah dodgeDirection
        rb.transform.position = Vector3.Lerp(this.transform.localPosition, dodgeDirection, 5f * Time.fixedDeltaTime);
        //}
    }

    IEnumerator DoDodge(float dodgeDuration)
    {
        if (playerStats != null)
        {
            if (playerStats.isPlayerAbleToDodge)
            {
                isPlayerDodge = true;

                yield return new WaitForSeconds(dodgeDuration);
                isPlayerDodge = false;
                yield break;

            }
        }

    }




    private void LookAt()
    {
        if (!isAndroid)
        {
            // raycast dari posisi mouse ke "MousePlane"
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);
            float distance = 0.0f;
            if (plane.Raycast(ray, out distance))
            {
                // mendapatkan posisi mouse terhadap "MousePlane"
                Vector3 mousePosition = ray.GetPoint(distance);
                // mengubah rotasi objek agar menghadap ke arah posisi mouse
                Vector3 lookDirection = mousePosition - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * routationSensitivity);
            }

        }
        else
        {
            Vector3 lookPos = new Vector3();

            lookPos.x += this.transform.position.x + playerRotJoystick.Horizontal * 10f;
            lookPos.z += this.transform.position.z + playerRotJoystick.Vertical * 10f;
            lookPos.y = 0;

            if (Mathf.Abs(playerRotJoystick.Horizontal) > 0.001f || Mathf.Abs(playerRotJoystick.Vertical) > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation((lookPos - transform.position).normalized, Vector3.up);
                targetRotation = Quaternion.Euler(transform.localEulerAngles.x, targetRotation.eulerAngles.y, transform.localEulerAngles.z);
                currentRotation = Quaternion.Slerp(currentRotation, targetRotation, routationSensitivity * Time.fixedDeltaTime);
                transform.rotation = currentRotation;
            }
            else
                    transform.rotation = currentRotation;


            Debug.DrawRay(lookPos, Vector3.up, Color.red);


        }

    }

    IEnumerator CheckAndroidControl()
    {
        yield return new WaitForSeconds(0.25f);
        isAndroid = playerStats.isAndroidControl;
        yield break;

    }

}
