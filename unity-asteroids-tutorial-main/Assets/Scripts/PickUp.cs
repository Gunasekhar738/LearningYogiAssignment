using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PickUp : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }


    public PowerUpInfo powerUpInfo;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Assign random properties to make each asteroid feel unique
        spriteRenderer.sprite = powerUpInfo.powerSprite;
     
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);

        // Set the scale and mass of the asteroid based on the assigned size so
        // the physics is more realistic
        transform.localScale = Vector3.one * powerUpInfo.size/5;
        rigidbody.mass = powerUpInfo.size;

        // Destroy the asteroid after it reaches its max lifetime
        Destroy(gameObject, powerUpInfo.maxLifetime);
       
    }

    public void SetTrajectory(Vector2 direction)
    {
       
        rigidbody.AddForce(direction * powerUpInfo.movementSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           
            Destroy(gameObject);
        }
    }



}
