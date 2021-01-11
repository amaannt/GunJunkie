using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviourPunCallbacks
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    Vector3 savePosition;

    public GameObject spine;
    public GameObject L_leg1,L_leg2, R_leg1, R_leg2,hips;
    [HideInInspector]
    public bool canMove = true;


    public GameObject camera,placeholder;

    public float momentum;

    public GameObject spine3, spine2, spine1, neck,shoulder1,shoulder2;
    void Start()
    {

        momentum = walkingSpeed;
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        savePosition = gameObject.transform.position;
       

       
    }

    public bool isPaused = false;
    public bool isCursorActive = false;

    float curSpeedX = 0; 
    float curSpeedY = 0;

    public bool isWalking = false, isCrouching = false;
    bool isRunning = false;
    // stop backkmovement while jumping forwards
    bool stopMovement = false,  continueMovement= false;
    void Update()
    {
        camera.SetActive(photonView.IsMine);
        placeholder.SetActive(!photonView.IsMine);
        if (!photonView.IsMine)
        {
            gameObject.layer = 9;
        }

        if (photonView.IsMine)
        {
            if (!isPaused)
            {
                if (isCursorActive)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    isCursorActive = false;
                }
                // We are grounded, so recalculate move direction based on axes
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 right = transform.TransformDirection(Vector3.right);
                // Press Left Shift to run
                isRunning = Input.GetKey(KeyCode.LeftShift);
                
                /*OLD MOVEMENT CODE
                 * 
                 * float curSpeedX =canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") :0;
                 float curSpeedY =canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
                 */

               
                if (Input.GetKey(KeyCode.W))
                {
                    curSpeedX = canMove ? ((isRunning ? runningSpeed : walkingSpeed) * 1) : 0;
                    
                    if (Input.GetKey(KeyCode.LeftShift)) //shifting
                    {

                        animationState(true,false);
                    }
                    else //if running
                    {
                        animationState( false,true );
                    }
                }
                if (Input.GetKeyUp(KeyCode.W))
                {
                    animationState(true, false);
                }

                //back (s key) down    
                if (Input.GetKey(KeyCode.S))
                {
                    moveBackward = true;
                    if (isRunning)
                    {
                        animationState(true,false);
                    }
                    else
                    {
                        animationState(false, true);
                    }

                } else if (Input.GetKeyUp(KeyCode.S))
                {
                    animationState(true, false);
                    moveBackward = false;
                }
                if (curSpeedX == 0) { animationState(false, false); }
                //Debug.Log("Speed forward: " + curSpeedX + " Speed Horizontal :" + curSpeedY);

                //right key down                
                if (Input.GetKey(KeyCode.D))
                {
                    if (!characterController.isGrounded) {
                        curSpeedY = canMove ? ((isRunning ? runningSpeed : walkingSpeed * 0.55f) * 1) : 0;
                    }
                    else
                    {
                       
                        hips.GetComponent<Animator>().SetBool("isSideWalking", true);
                        
                        curSpeedY = canMove ? ((isRunning ? runningSpeed : walkingSpeed * 0.8f) * 1) : 0;
                       // Debug.Log("Side Walking right");
                    }


                }
                else if (Input.GetKeyUp(KeyCode.D))
                {

                    hips.GetComponent<Animator>().SetBool("isSideWalking", false);
                   
                }

                //moving left or right
                //left key down     

                if (Input.GetKey(KeyCode.A))
                {
                    if (!characterController.isGrounded)
                    {
                        curSpeedY = canMove ? ((isRunning ? runningSpeed : walkingSpeed * 0.55f) * -1) : 0;

                    }
                    else
                    {
                        hips.GetComponent<Animator>().SetBool("isSideWalking", true);
                        curSpeedY = canMove ? ((isRunning ? runningSpeed : walkingSpeed * 0.8f) * -1) : 0;
                        //Debug.Log("Side Walking left");
                    }
                }
                else if (Input.GetKeyUp(KeyCode.A))
                {
                    hips.GetComponent<Animator>().SetBool("isSideWalking", false);
                }

                //if forward speed is more than 0 then reduce it either while on ground or in air
                if (curSpeedX >= 0.0f)
                {
                    if (!characterController.isGrounded)
                    {
                        curSpeedX -= 0.1f;

                    }
                    else
                    {
                        curSpeedX -= 0.5f;
                    }
                }
                else if (curSpeedX <= 0.0f)
                {
                    if (!characterController.isGrounded)
                    {
                        curSpeedX += 0.1f;
                    }
                    else
                    {
                        curSpeedX += 0.5f;
                    }
                }
                //if horizontal (left/right) speed is more than 0 then slowly reduce it either while the character is on ground or not
                if (curSpeedY >= 0.0f)
                {
                    if (!characterController.isGrounded)
                    {
                        curSpeedY -= 0.1f;
                    }
                    else
                    {
                        curSpeedY -= 0.8f;
                    }
                }
                else if (curSpeedY <= 0.0f)
                {
                    if (!characterController.isGrounded)
                    {
                        curSpeedY += 0.1f;
                    }
                    else
                    {
                        curSpeedY += 0.8f;
                    }
                }
               

                float movementDirectionY = moveDirection.y;

                //remove minor speeds from glitching game
                if (curSpeedY > -0.9f && curSpeedY < 0.9f && characterController.isGrounded)
                {
                    curSpeedY = 0;
                }
                if (curSpeedX > -0.9f && curSpeedX < 0.9f && characterController.isGrounded)
                {
                    curSpeedX = 0;
                }


                moveDirection = (forward * curSpeedX) + (right * curSpeedY);
                if (Input.GetKey(KeyCode.Space))
                {
                    hips.GetComponent<Animator>().SetBool("isStandingJump", true);
                }
                //stop jumping animations once on ground
                if (characterController.isGrounded)
                {
                    hips.GetComponent<Animator>().SetBool("isStandingJump", false);
                }

                //when pressing jump, jump up
                if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
                {
                    moveDirection.y = jumpSpeed;
                }
                else
                {
                    moveDirection.y = movementDirectionY;
                }
                
                // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
                // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
                // as an acceleration (ms^-2)
                if (!characterController.isGrounded)
                {
                    moveDirection.y -= gravity * Time.deltaTime;
                }
                
                // Move the controller
                characterController.Move(moveDirection * Time.deltaTime);
                /*
                 
                 HERE IS PLAYER ROTATION CODE
                 NEGATIVE IS LOOK UP

                 */
                // Player and Camera rotation
                if (canMove)
                {
                    rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                    rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                    //playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                    
                    //rotation to look up
                    if (rotationX <= 0)
                    {
                        //bending the spine up to look more natural
                        spine3.transform.localRotation = Quaternion.Euler(rotationX/4, 0, 0);
                        spine2.transform.localRotation = Quaternion.Euler(rotationX / 4, 0, 0);
                        
                            spine1.transform.localRotation = Quaternion.Euler(rotationX/2, 0, 0);
                        
                     }
                    else
                    {
                       spine.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                    }

                    //left and right look
                    transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0); 
                }


                if (Input.GetKey(KeyCode.R))
                {
                    SavePosition();
                }
                if (Input.GetKey(KeyCode.T))
                {
                    setPosition();
                }

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    if (!isCrouching)
                    {
                        spine.transform.position = PositionCrouch(spine, 1.3f);

                        hips.transform.position = PositionCrouch(hips, 1.3f);
                        hips.GetComponent<Animator>().SetBool("isStillCrouch", true);
                        isCrouching = true;
                    }
                }
                else if (Input.GetKeyUp(KeyCode.LeftControl))
                {
                    if (isCrouching)
                    {
                        spine.transform.position = PositionStand(spine, 1.3f);
                        hips.transform.position = PositionStand(hips, 1.3f);
                        hips.GetComponent<Animator>().SetBool("isStillCrouch", false);
                        isCrouching = false;
                    }
                }
            }
            else
            {
                if (!isCursorActive)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    isCursorActive = true;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            RespawnRandomly();
        }
       /* AnimatorClipInfo[] m_CurrentClipInfo = hips.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        Debug.Log(m_CurrentClipInfo[0].clip.name);*/
    }
    //physics stuff which is more efficient

    bool moveForward = false, moveBackward = false, moveLeft = false, moveRight = false;
    private void FixedUpdate()
    {
        //move backwards
        if (moveBackward)
        {
            //if the character is moving forward, make it stagnate and come to a near stop
            if ((curSpeedX > 0 || curSpeedY > 0 || curSpeedY < 0) && !characterController.isGrounded)
            {
                curSpeedX -= curSpeedX / 20;
                curSpeedY = 0;
            }
            else
            {
                //once character has stopped/ is moving from 0 movement speed then slowly increase to imply acceleration backwards
                if (curSpeedX >= -walkingSpeed + 1)
                {
                    if (isRunning)
                    {
                        curSpeedX = runningSpeed * -1;
                    }
                    else
                        curSpeedX += -1 * (walkingSpeed) * 0.1f;
                    Debug.Log(curSpeedX);

                }
                else
                {
                    //once acceleration has reached full speed, move at max walking speed
                    curSpeedX = canMove ? ((isRunning ? runningSpeed : walkingSpeed) * -1) : 0;
                }
            }
        }
    }
    void animationState(bool state1, bool state2) {
        hips.GetComponent<Animator>().SetBool("isShifting", state1);
        hips.GetComponent<Animator>().SetBool("isWalking", state2);
    }
    Vector3 PositionCrouch(GameObject x,float amount) {
        return new Vector3(x.transform.position.x, x.transform.position.y - amount, x.transform.position.z);
    }
    Vector3 PositionStand(GameObject x, float amount)
    {
        return new Vector3(x.transform.position.x, x.transform.position.y + amount, x.transform.position.z);
    }
    void SavePosition() {
        savePosition = gameObject.transform.position;

    }
    void setPosition()
    {
        gameObject.transform.position= savePosition;
    }
    void RespawnRandomly()
    {
        gameObject.transform.position = new Vector3(Random.Range(-65, 44), 45, Random.Range(-58, 60));
    }
    public void SetLookSensitivity(float value) {
        lookSpeed = value;
    }


   
}