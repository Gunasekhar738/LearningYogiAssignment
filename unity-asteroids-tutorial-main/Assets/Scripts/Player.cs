using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum BulletType
{
    Normal,
    Crescend

}
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public Bullet bulletPrefab;
    public Bullet bulletCrescentPrefab;

    public float thrustSpeed = 1f;
    public bool thrusting { get; private set; }

    public float turnDirection { get; private set; } = 0f;
    public float rotationSpeed = 0.1f;

    public float respawnDelay = 3f;
    public float respawnInvulnerability = 3f;

    public bool screenWrapping = true;
    private Bounds screenBounds;

    private float BulletFireRate = 0.1f;
    public int NumberOfBullets = 3;

    [SerializeField] GameInput gameInput;

    [SerializeField] private GameObject Bubble;


    [SerializeField] private float bubbleDuration = 10;
    private bool isBubbleActivated = false;

    [SerializeField] private Text timeLeftText;


    [SerializeField] private BulletType bulletType;

    private IEnumerator blasterCoroutine;
    private IEnumerator bubbleCoroutine;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        GameConfiguration config = GameManager.Instance.gameConfiguration;
        rotationSpeed = config.SpaceshipRotationSpeed;
        thrustSpeed = config.SpaceshipAcceleration;
    }

    private void Start()
    {
        GameObject[] boundaries = GameObject.FindGameObjectsWithTag("Boundary");

        // Disable all boundaries if screen wrapping is enabled
        for (int i = 0; i < boundaries.Length; i++) {
            boundaries[i].SetActive(!screenWrapping);
        }

        // Convert screen space bounds to world space bounds
        screenBounds = new Bounds();
        screenBounds.Encapsulate(Camera.main.ScreenToWorldPoint(Vector3.zero));
        screenBounds.Encapsulate(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)));

      
    }

    private void OnEnable()
    {
        // Turn off collisions for a few seconds after spawning to ensure the
        // player has enough time to safely move away from asteroids
        TurnOffCollisions();
        Invoke(nameof(TurnOnCollisions), respawnInvulnerability);
    }

    private void Update()
    {
        thrusting = gameInput.isForwardPressed();
        turnDirection = gameInput.GetRotationValue();
    }

    private void FixedUpdate()
    {
        if (thrusting) {
            rigidbody.AddForce(transform.up * thrustSpeed);
        }

        if (turnDirection != 0f) {
            rigidbody.AddTorque(rotationSpeed * turnDirection);
        }

        if (screenWrapping) {
            ScreenWrap();
        }

      
    }

    private void ScreenWrap()
    {
        // Move to the opposite side of the screen if the player exceeds the bounds
        if (rigidbody.position.x > screenBounds.max.x + 0.5f) {
            rigidbody.position = new Vector2(screenBounds.min.x - 0.5f, rigidbody.position.y);
        }
        else if (rigidbody.position.x < screenBounds.min.x - 0.5f) {
            rigidbody.position = new Vector2(screenBounds.max.x + 0.5f, rigidbody.position.y);
        }
        else if (rigidbody.position.y > screenBounds.max.y + 0.5f) {
            rigidbody.position = new Vector2(rigidbody.position.x, screenBounds.min.y - 0.5f);
        }
        else if (rigidbody.position.y < screenBounds.min.y - 0.5f) {
            rigidbody.position = new Vector2(rigidbody.position.x, screenBounds.max.y + 0.5f);
        }
    }
    public void ShootNewInput()
    {
        StartCoroutine(Shoot());

    }
    IEnumerator Shoot()
    {
       
       
        for (int i = 0; i < NumberOfBullets; i++)
        {
            if (bulletType == BulletType.Normal)
            {
                Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                bullet.Shoot(transform.up);
            }
            else if (bulletType == BulletType.Crescend)
            {
                Bullet bullet = Instantiate(bulletCrescentPrefab, transform.position, transform.rotation);
                bullet.Shoot(transform.up);
            }
            
            yield return new WaitForSeconds(BulletFireRate);
        }
    }

    private void TurnOffCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
    }

    private void TurnOnCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = 0f;
            if (!isBubbleActivated)
                GameManager.Instance.OnPlayerDeath(this);
            else
                DeActivateBubble();
        }
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            PowerUpType powerUpType = collision.gameObject.GetComponent<PickUp>().powerUpInfo.powerUpType;
            if (powerUpType == PowerUpType.ForceField)
            {

                if (bubbleCoroutine != null)
                StopCoroutine(bubbleCoroutine);

                bubbleCoroutine = ActivateBubble();
                StartCoroutine(bubbleCoroutine);
            }
            else
            if (powerUpType == PowerUpType.ShootUpgrade)
            {

                if (blasterCoroutine != null)
                StopCoroutine(blasterCoroutine);

                blasterCoroutine = ActivateBlaster();
                StartCoroutine(blasterCoroutine);

            }
        }
    }


    IEnumerator ActivateBlaster()
    {
        bulletType = BulletType.Crescend;
        float startTime = Time.time;
        int total = 100;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            total--;
            timeLeftText.text = "Time Left " + total / 10;
            if (Time.time - startTime > 10)
            {
                bulletType = BulletType.Normal;
                timeLeftText.text = " ";
                break;
            }
        }

    }
 

    IEnumerator ActivateBubble()
    {
       
        Bubble.SetActive(true);
        isBubbleActivated = true;
        float startTime = Time.time;
        int total = 100;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            total--;
            if (Time.time - startTime > 10)
            {
                DeActivateBubble();
                break;
            }
        }
    }

    void DeActivateBubble()
    {
        Bubble.SetActive(false);
        isBubbleActivated = false;

    }

}
