using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComboKeyPlayerController : MonoBehaviour
{
    // Variables de mouvement
    public float speed = 10f;
    public float sprintSpeed = 25f;
    public float jumpForce = 15f;
    private Rigidbody2D rb;
    private Animator animator;

    private bool hasJumpAvailable = false;
    private bool canDoubleJump = false;
    private bool isDashing = false;
    public float dashSpeed = 30f;
    public float dashDuration = 0.2f;
    public KeyCode dashKey = KeyCode.Mouse1;
    private float lastDashTime;
    public float dashCooldown = 1f;
    private bool isFirstDash = true;

    // Variables pour les touches
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;

    // Variables pour le grappin
    public KeyCode grappleKey = KeyCode.E;
    public LayerMask grappleLayer;
    public LineRenderer grappleLine;
    public float maxGrappleDistance = 10f;
    private bool isGrappling = false;
    private DistanceJoint2D grappleJoint;
    private Vector2 grapplePoint;
    public int damage = 1;
    private PlayerHealth PlayerHealth;



    void Start()
    {
        PlayerHealth = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Initialisation du joint pour le grappin
        grappleJoint = gameObject.AddComponent<DistanceJoint2D>();
        grappleJoint.enabled = false;
        grappleJoint.autoConfigureDistance = false;

        if (grappleLine != null)
        {
            grappleLine.enabled = false;
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) // Vérifie si la touche Échap est pressée
        {
            SceneManager.LoadScene("MainMenu"); // Remplace "NomDeLaScene" par le nom de ta scène
        }


        HandleMovement();

        // Gestion du grappin
        if (Input.GetKeyDown(grappleKey))
        {
            if (isGrappling)
            {
                DetachGrapple();
            }
            else
            {
                TryToGrapple();
            }
        }

        // Se détacher du grappin avec la touche saut
        if (Input.GetKeyDown(jumpKey) && isGrappling)
        {
            DetachGrapple();
            hasJumpAvailable = true;
            lastDashTime = -dashCooldown;
        }

        // Gestion du dash
        if (Input.GetKeyDown(dashKey) && !isDashing)
        {
            if (isFirstDash || Time.time >= lastDashTime + dashCooldown)
            {
                StartCoroutine(Dash());
                lastDashTime = Time.time;
                animator.SetTrigger("Dash");
                isFirstDash = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            dashCooldown = 0f;
        }
        
    }

    private void HandleMovement()
    {
        if (isGrappling || isDashing)
        {
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float currentSpeed = Input.GetKey(sprintKey) ? sprintSpeed : speed;

        rb.velocity = new Vector2(horizontalInput * currentSpeed, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

        if (horizontalInput != 0)
        {
            Flip(horizontalInput);
        }

        if (Input.GetKeyDown(jumpKey))
        {
            if (hasJumpAvailable)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                hasJumpAvailable = false;
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false;
            }
        }

        if (isGrappling && grappleLine != null)
        {
            grappleLine.SetPosition(0, transform.position);
        }
    }

    private void TryToGrapple()
    {
        Vector2 playerPosition = transform.position;

        // Cherche tous les colliders autour du joueur dans un rayon
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(playerPosition, maxGrappleDistance, grappleLayer);

        if (hitColliders.Length > 0)
        {
            // Choisir le premier point trouvé comme point d'accrochage
            grapplePoint = hitColliders[0].ClosestPoint(playerPosition);
            isGrappling = true;

            // Configurer le joint
            grappleJoint.enabled = true;
            grappleJoint.connectedAnchor = grapplePoint;
            grappleJoint.distance = Vector2.Distance(playerPosition, grapplePoint);

            // Activer la ligne visuelle
            if (grappleLine != null)
            {
                grappleLine.enabled = true;
                grappleLine.SetPosition(0, transform.position);
                grappleLine.SetPosition(1, grapplePoint);

                // Définir la couleur de la ligne à vert
                grappleLine.startColor = Color.green;
                grappleLine.endColor = Color.green;

                // Configurer la Sorting Layer et l'Order in Layer
                grappleLine.sortingLayerName = "GrappleLine"; // Assurez-vous que cette couche existe
                grappleLine.sortingOrder = 5; // Assurez-vous que c'est plus grand que le fond
            }

            Debug.Log("Grappin attaché à " + grapplePoint);
        }
        else
        {
            Debug.Log("Aucun point d'accrochage trouvé !");
        }
    }

    private void DetachGrapple()
    {
        isGrappling = false;
        grappleJoint.enabled = false;

        if (grappleLine != null)
        {
            grappleLine.enabled = false;
        }

        Debug.Log("Grappin détaché !");
    }

    private IEnumerator Dash()
    {
        isDashing = true;

        float dashDirectionX = transform.localScale.x > 0 ? 1 : -1;
        Vector2 dashDirection = new Vector2(dashDirectionX, 0).normalized;

        float dashEndTime = Time.time + dashDuration;
        while (Time.time < dashEndTime)
        {
            rb.velocity = dashDirection * dashSpeed;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        isDashing = false;
    }

    private void Flip(float direction)
    {
        if ((direction > 0 && transform.localScale.x < 0) || (direction < 0 && transform.localScale.x > 0))
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            lastDashTime = -dashCooldown;
            if (IsGrounded(collision))
            {
                hasJumpAvailable = true;
                canDoubleJump = false;
            }
        }
        else if (collision.gameObject.CompareTag("Mur"))
        {
            lastDashTime = -dashCooldown;
            Debug.Log("Colision Mur");
            hasJumpAvailable = true;
            canDoubleJump = false;
        }
        else if (collision.gameObject.CompareTag("Ronce"))
        {

            Debug.Log("Collision avec une Ronce");
            PlayerHealth PlayerHealth = GetComponent<PlayerHealth>();
            
            if (PlayerHealth != null)
            {
                // Appelle la méthode TakeDamage du script PlayerHealth
                PlayerHealth.TakeDamage(1);
            }

            Vector2 collisionNormal = collision.contacts[0].normal;
            float intensity = -20f;
            Vector2 repulsionForce = -collisionNormal * intensity;
            hasJumpAvailable = true;
            lastDashTime = -dashCooldown;

            Debug.Log($"Collision normale : {collisionNormal}");
            Debug.Log($"Force de répulsion calculée : {repulsionForce}");

            if (rb != null)
            {
                rb.AddForce(repulsionForce, ForceMode2D.Impulse);
                Debug.Log("Force de répulsion appliquée");
            }
            else
            {
                Debug.LogWarning("Rigidbody2D manquant sur l'objet !");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxGrappleDistance);
    }

    private bool IsGrounded(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5)
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateGrappleOrientation()
    {
        if (isGrappling)
        {
            Vector2 direction = grapplePoint - (Vector2)transform.position;
            if (direction.x > 0 && transform.localScale.x < 0 || direction.x < 0 && transform.localScale.x > 0)
            {
                Flip(Mathf.Sign(direction.x));
            }

            if (grappleLine != null)
            {
                grappleLine.SetPosition(0, transform.position);
                grappleLine.SetPosition(1, grapplePoint);
            }
        }
    }

    void FixedUpdate()
    {
        UpdateGrappleOrientation();
    }
}
