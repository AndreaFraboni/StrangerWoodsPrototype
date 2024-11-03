using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public float maxHealth = 100f; // Salute massima del personaggio
    public float currentHealth; // Salute attuale del personaggio

    public float moveSpeed = 5f; // Velocità di movimento del personaggio

    [SerializeField] private float gravity = -9.81f;

    private bool isPlayerAttacking = false;
    private bool isPlayerAggression = false;

    private Vector2 moveVector;
    private Vector2 lookVector;
    private Vector3 rotation;
    private Vector3 _direction;
    private float smoothTime = 0.05f;
    private float _currentVelocity;
    public float verticalVelocity;

    private CharacterController characterController;
    private Animator animator;

    public bool isDead;
    public GameObject Spada;

    public bool CanAttack = true;

    public bool getIsDead()
    {
        return isDead;
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        isDead = false;
        Spada.GetComponent<BoxCollider>().enabled = false;
    }

    void Start()
    {
        currentHealth = maxHealth; // All'inizio, la salute attuale è uguale alla salute massima
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        ApplyGravity();
        if (!isPlayerAttacking) ApplyRotation();
        if (!isPlayerAttacking) Move();
    }

    //*******************************************************************************************************//
    //************************* PLAYER TAKE DAMAGE **********************************************************//
    //*******************************************************************************************************//
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount; // Sottrai i danni dalla salute attuale    
        //Debug.Log("CurrentHealth => " + currentHealth.ToString());
        GameController.Instance.LiveLost();

        animator.SetTrigger("React");

        // Controlla se la salute è inferiore o uguale a zero
        if (currentHealth <= 0)
        {
            isDead = true;
            Die(); // Se sì, il personaggio muore
        }
    }

    //*******************************************************************************************************//
    //************************* PLAYER DIE ******************************************************************//
    //*******************************************************************************************************//
    private void Die()
    {
        //Debug.Log("SONO MORTO !!!!");
        // Opzionale: Aggiungi qui effetti visivi o sonori per la morte del personaggio
        animator.SetBool("Die", true);
    }

    //*******************************************************************************************************//
    //************************* PLAYER OnMove ***************************************************************//
    //*******************************************************************************************************//
    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
        _direction = new Vector3(moveVector.x, 0.0f, moveVector.y);

        if (moveVector.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    //*******************************************************************************************************//
    //************************* PLAYER ROTATION *************************************************************//
    //*******************************************************************************************************//
    private void ApplyRotation()
    {
        if (moveVector.sqrMagnitude == 0) return;
        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    //*******************************************************************************************************//
    //************************* APPLY GRAVITY TO PLAYER ****************************************************//
    //*******************************************************************************************************//
    private void ApplyGravity()
    {
        if (characterController.isGrounded && verticalVelocity < 0.0f)
        {
            verticalVelocity = -1.0f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        _direction.y = verticalVelocity;
    }

    //*******************************************************************************************************//
    //************************* PLAYER MOVEMENT *************************************************************//
    //*******************************************************************************************************//
    private void Move()
    {
        characterController.Move(_direction * moveSpeed * Time.deltaTime);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookVector = context.ReadValue<Vector2>();
    }

    //*******************************************************************************************************//
    //************************* MANAGE PLAYER ATTACK ********************************************************//
    //*******************************************************************************************************//
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (CanAttack)
        {
            isPlayerAttacking = true;
            Spada.GetComponent<BoxCollider>().enabled = true;

            if (animator.GetBool("isWalking"))
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking", true);
            }
            else
            {
                animator.SetBool("isAttacking", true);
            }
            CanAttack = false;
        }
    }

    IEnumerator SetCanAttack()
    {
        yield return new WaitForSeconds(0.5f);
        CanAttack = true;
    }

    public void isAttackingFinished()
    {
        isPlayerAttacking = false;
        Spada.GetComponent<BoxCollider>().enabled = false;

        if (moveVector.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        StartCoroutine(SetCanAttack());
    }

    public void AttackFinished(string input)
    {
        animator.SetBool("isAttacking", false);
        Invoke("isAttackingFinished", 0.2f);
    }
}
