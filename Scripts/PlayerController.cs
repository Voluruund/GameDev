using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    public float mvmSpeed = 3;
    public bool isMoving;
    private Vector2 input;
    private int sceneLoaded = 0;
    private Vector3 newScenePos;

    private Rigidbody2D rb;

    private Animator animator;

    public LayerMask solidObj;
    public LayerMask interact;

    private static PlayerController instance;

    private Coroutine walkingCoroutine;
    private bool isTeleporting = false;

    private void Awake()
    {
        // singleton pattern to destroy duplicate players in scenes
        if (instance == null)
        {
            instance = this;
            animator = GetComponent<Animator>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        HandleUpdate();
    }

    public async void HandleUpdate()
    {
        //Debug.Log("entra qua dentro diocane");
        if (isTeleporting) {
            Debug.Log("teleporting");
            await TimeoutBeforeReturn(0.1f);
            isTeleporting = false;
            isMoving = false;
            Debug.Log(isTeleporting);
            Debug.Log(isMoving);
            return;
        }
        if (!isMoving) 
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) 
            {
                input.y = 0;
            }

            if(input != Vector2.zero)
            {
                animator.SetFloat("MoveX", input.x);
                animator.SetFloat("MoveY", input.y);

                var targetPosition = transform.position;
                if(targetPosition.x > 50)
                {
                    Debug.Log("after teleport debug");
                    Debug.Log(isTeleporting);
                    Debug.Log(isMoving);
                    Debug.Log(input.x);
                    Debug.Log(input.y);
                }
                targetPosition.x += input.x;
                targetPosition.y += input.y;
                targetPosition.z = 0;

                if (isWalkable(targetPosition)) 
                {
                   StartMyCoroutine(targetPosition);
                    //rb.velocity = new Vector2(input.x * mvmSpeed, input.y * mvmSpeed); // Update velocity with movement speed
                    animator.SetBool("isMoving", true);
                    //Debug.Log("position " + targetPosition);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    animator.SetBool("isMoving", false);
                }
            }
        }

        //animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    // Method to stop the coroutine
    public void StopMyCoroutine()
    {
        if (walkingCoroutine != null)
        {
            StopCoroutine(walkingCoroutine);
            walkingCoroutine = null; // Reset the reference
        }
    }

    public void StartMyCoroutine(Vector3 v)
    {
        // Ensure the coroutine doesn't start if the player is teleporting
        /*if (!isTeleporting)
        {
            walkingCoroutine = StartCoroutine(Move(v));
        }*/
        walkingCoroutine = StartCoroutine(Move(v));
    }

    void Interact()
    {
        var facingDirection = new Vector3(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"));
        var interactPos = transform.position + facingDirection;

        // debug 
        //Debug.DrawLine(transform.position, interactPos, Color.red, 1f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interact);
        if (collider != null) { 
            // null checking
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    private async Task TimeoutBeforeReturn(float timeout)
    {
        // Wait for the specified timeout
        await Task.Delay((int)(timeout * 1000));  // Task.Delay takes milliseconds, so multiply by 1000
    }

    public IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;
        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon) 
        { 
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, mvmSpeed * Time.deltaTime);
            yield return null;  
        }
        transform.position = targetPosition;
        isMoving = false;
    }
    
    /*
    private IEnumerator Move(Vector3 targetPosition)
    {
        float timeToMove = 0.5f;
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
    */

    public void Teleport(Vector3 targetPosition)
    {
        // Disable movement temporarily
        StopMyCoroutine();
        isTeleporting = true;
        transform.position = targetPosition;
        //animator.SetBool("isMoving", true);



        // Stop movement completely and reset velocities
        Debug.Log("velocità iniziale: " + rb.velocity);
        rb.velocity = Vector3.zero;
        Debug.Log("targetPosition: " + targetPosition);
        rb.angularVelocity = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation; // Freeze position and rotation

        animator.SetFloat("MoveX", 0f);
        animator.SetFloat("MoveY", 0f);
        animator.SetBool("isMoving", false);
        // Immediately allow movement right after teleportation, without delay
        input.x = 0; // Reset horizontal input to 0
        input.y = 0; // Reset vertical input to 0
        isTeleporting = true;
        // After a short delay, re-enable movement
        //StartCoroutine(EnableMovement());  // 0.5 seconds delay, adjust as needed
    }

    private IEnumerator EnableMovement()
    {
        yield return null;
        //isTeleporting = false;
        //rb.velocity = new Vector3(mvmSpeed * Time.deltaTime, mvmSpeed * Time.deltaTime,0);
        Debug.Log("isTeleporting: " + isTeleporting);
    }

    private bool isWalkable(Vector3 targetPosition)
    {
        if (Physics2D.OverlapCircle(targetPosition, 0.5f, solidObj | interact) != null)
        {
            return false;
        }
        return true;
    }
}
