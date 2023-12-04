using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    public Camera playerCamera;

    [Header("-----Audio-----")]
    public PlayerSounds PlayerSounds;
    public AudioSource ShootSource;


    [Header("----- Player Stats -----")]
    [SerializeField] int HP;
    private int HPMax;

    [Header("----- Player Attack -----")]
    public List<GunStats> gunList = new List<GunStats>();
    [SerializeField] float shootRate;
    public int shootDamage;
    //[SerializeField] int ammoCur;
    //[SerializeField] int ammoMax;
    [SerializeField] GameObject gunModel;
    //[SerializeField] int shootDist;
    private bool isShooting;
    public int selectedGun;

    [Header("----- Player Movement -----")]
    [SerializeField] float MoveSpeed;
    [SerializeField] float SprintMod;
    public int JumpMax;
    [SerializeField] int JumpHeight;
    [SerializeField] int GravityValue;
    private bool groundedPlayer;
    private Vector3 moveDirection;
    private Vector3 playerVelocity;
    private int jumpCount;
    private bool isSprinting;
    private bool isJumping;
    private float lastJumpTime = 0f;
    public float jumpCooldown;

    public State state;
    public enum State
    {
        Walking, Flying
    }

    void Start()
    {
        HPMax = HP;
        JumpMax = 1;
        jumpCooldown = 1f;
        spawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Walking:

            break;


            case State.Flying:

            break;
        }
        // Walking:
        MovePlayer();
        Sprint();
        SelectGun();

        if (gunList.Count > 0 && Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }

        // Flying:
        //MoveFlyingPlayer();

    }

    void MovePlayer()
    {
        groundedPlayer = controller.isGrounded;

        //Handle Grounded State
        if (groundedPlayer)
        {

            if (isJumping)
            {
                PlayerSounds.LandEmote();
            }

            playerVelocity.y = 0f;
            playerVelocity.x = 0f;
            playerVelocity.z = 0f;// Ensures the player does not accumulate downward velocity when grounded.
            jumpCount = 0;
            isJumping = false;

            //Handle FootStepSFX
            if (moveDirection.normalized.magnitude > 0f && !isJumping)
            {
                playerVelocity = moveDirection * MoveSpeed;
                PlayerSounds.PlayFootstep(playerVelocity);
            }
        }

        //Handle Input
        moveDirection = (Input.GetAxis("Horizontal") * transform.right) +
                        (Input.GetAxis("Vertical") * transform.forward);
        moveDirection.Normalize();
        controller.Move(moveDirection * Time.deltaTime * MoveSpeed);

        //Handle Jumping
        if (Input.GetButtonDown("Jump") && jumpCount < JumpMax && Time.time - lastJumpTime > jumpCooldown)
        {
            lastJumpTime = Time.time;
            playerVelocity.y = Mathf.Sqrt(JumpHeight * -2f * GravityValue);
            PlayerSounds.JumpEmote();
            jumpCount++;
            isJumping = true;
        }

        {
            //Apply Gravity
            playerVelocity.y += GravityValue * Time.deltaTime;
        }
        controller.Move(playerVelocity * Time.deltaTime);

    }

    void MoveFlyingPlayer()
    {
        groundedPlayer = controller.isGrounded;

        //Handle Grounded State
        if (groundedPlayer)
        {

            if (isJumping)
            {
                PlayerSounds.LandEmote();
            }

            playerVelocity.y = 0f;
            playerVelocity.x = 0f;
            playerVelocity.z = 0f;// Ensures the player does not accumulate downward velocity when grounded.
            jumpCount = 0;
            isJumping = false;

        }

        //Handle Input
        moveDirection = (Input.GetAxis("Horizontal") * transform.right) +
                        (Input.GetAxis("Vertical") * transform.forward);
        moveDirection.Normalize();
        controller.Move(moveDirection * Time.deltaTime * MoveSpeed);

        controller.Move(playerVelocity * Time.deltaTime);

    }

    void OnToggleFlying()
    {
        state = state == State.Flying ? State.Walking : State.Flying;
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            MoveSpeed *= SprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            MoveSpeed /= SprintMod;
        }
    }
    ////This code is for raycast shooting instead of projectile
    //IEnumerator shoot()
    //{
    //    isShooting = true;
    //    RaycastHit hit;
    //    if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
    //    {
    //        IDamage damageable = hit.collider.GetComponent<IDamage>();

    //        if(damageable != null)
    //        {
    //            damageable.TakeDamage(shootDamage);
    //        }
    //    }

    //    yield return new WaitForSeconds(shootRate);
    //    isShooting = false;
    //}

    IEnumerator shoot()
    {
        if (gunList[selectedGun].ammoCur > 0)
        {
            isShooting = true;

            gunList[selectedGun].ammoCur--;
            UpdatePlayerUI();

            //Instantiate(bullet, shootPos.position, transform.rotation);
            GameObject bulletObject = Instantiate(bullet);

            // bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
            bulletObject.transform.position = shootPos.transform.position;
            bulletObject.transform.forward = playerCamera.transform.forward;

            ShootSource.Play();
            yield return new WaitForSeconds(shootRate);
            isShooting = false;

            ////debug tool
            //RaycastHit hit;
            //if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            //{
            //    Debug.Log(hit.transform.name);
            //}
        }
    }

    public void TakeDamage(int amount)
    {
        //look at enemy takedamage to turn friendly fire off.

        HP -= amount;
        StartCoroutine(GameManager.instance.PlayerFlashDamage());
        UpdatePlayerUI();
        if (HP <= 0)
        {
            GameManager.instance.youLose();
        }
    }

    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
        HP = HPMax;
    }

    public void UpdatePlayerUI()
    {
        //when deviding an int by an int you need to convert one to a float. 
        GameManager.instance.playerHPBar.fillAmount = (float)HP / HPMax;

        if (gunList.Count > 0)
        {
            GameManager.instance.ammoCur.text = gunList[selectedGun].ammoCur.ToString("F0");
            GameManager.instance.ammoMax.text = gunList[selectedGun].ammoMax.ToString("F0");
        }

    }

    public void GunPickup(GunStats gun)
    {
        gunList.Add(gun);

        shootDamage = gun.shootDamage;
        shootRate = gun.shootRate;
        ShootSource.clip = gun.shootSound;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;

        selectedGun = gunList.Count - 1;

        UpdatePlayerUI();
    }

    void SelectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            ChangeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            ChangeGun();
        }
    }

    void ChangeGun()
    {
        shootDamage = gunList[selectedGun].shootDamage;
        shootRate = gunList[selectedGun].shootRate;
        ShootSource.clip = gunList[selectedGun].shootSound;
        //ammoMax = gunList[selectedGun].ammoMax;      

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
        UpdatePlayerUI();
    }
}
