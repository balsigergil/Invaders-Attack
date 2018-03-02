using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class for enemies
/// </summary>
public class Enemy : MonoBehaviour {

    [Header("General")]
    [SerializeField, Tooltip("How many health it have")]
    private int maxHealth = 1;
    private int health = 0;

    [SerializeField, Tooltip("How many points per kill")]
    private int lootPoints = 10;

    [SerializeField, Tooltip("Lateral moving speed")]
    private float movingSpeed = 1.0f;

    [SerializeField, Tooltip("Explosion prefab to spawn at death")]
    private GameObject explosion;

    [SerializeField]
    private Slider healthSlider;


    [Header("Projectiles")]
    [SerializeField]
    private Bullet bulletPrefab;

    [SerializeField]
    private int bulletSpeed = 5;

    private GameManager gm = null;

    private float shootTimer = 0f;
    private float shootTimerMin = 1f;
    private float shootTimerMax = 20f;
    float timer = 0f;

    private Vector3 borderRight;
    private Vector3 borderLeft;

    /// <summary>
    /// New position in the Y axis
    /// </summary>
    private float newPosY = 0.0f;

    private bool hasTouchEdgeOfScreen = false;


    // Initialization
    void Start()
    {
        // Find the game manager in the current scene
        gm = FindObjectOfType<GameManager>();

        // Define the border of the screen based on the user screen resolution
        borderRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10));
        borderLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10));

        // Define the shooting timer
        shootTimer = Random.Range(shootTimerMin, shootTimerMax);

        // Set enemy health at maximum health for the beginning
        health = maxHealth;

        // Hide the health bar at start
        healthSlider.gameObject.SetActive(false);

        newPosY = transform.position.y;
  
    }

    // Update is called once per frame
    void Update()
    { 
        // Check the enemy position compared to the edge of the device screen
        if (transform.position.x >= borderRight.x)
        {
            movingSpeed = -Mathf.Abs(movingSpeed);
            hasTouchEdgeOfScreen = true;
        }
        if (transform.position.x <= borderLeft.x)
        {
            movingSpeed = Mathf.Abs(movingSpeed);
            hasTouchEdgeOfScreen = true;
        }

        // Goes one step down if the enemy touched the edge
        if(transform.position.x >= borderLeft.x && transform.position.x <= borderRight.x && hasTouchEdgeOfScreen)
        {
            StepDown();
            hasTouchEdgeOfScreen = false;
        }

        // Update the enemy position
        transform.position = new Vector3(transform.position.x + movingSpeed * Time.deltaTime, Mathf.Lerp(transform.position.y, newPosY, 0.06f));

        // Define a timer and shoot when it's done
        timer += Time.deltaTime;
        if (timer >= shootTimer)
        {
            timer = 0;
            shootTimer = Random.Range(shootTimerMin, shootTimerMax);
            Shoot();
        }
    }

    /// <summary>
    /// Handles the firing
    /// </summary>
    private void Shoot()
    {
        // Creates a new bullet in the scene at the current enemy position
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(-90, 0, 0));
        // Assigning the bullet velocity always to a negative value so the bullet goes down
        bullet.SetSpeed(-Mathf.Abs(bulletSpeed));
    }

    /// <summary>
    /// When a bullet collides
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider collidingBullet)
    {
        // Checks if the bullet is from the player
        if (collidingBullet.gameObject.tag == "Bullet")
        {
            // GOD MOD for testing of course :)
            if (gm.GodMode())
            {
                // Destroys all enemies in one fell swoop
                foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                {
                    GameObject exp = Instantiate(explosion, enemy.transform.position, enemy.transform.rotation);
                    Destroy(enemy.gameObject);
                    Destroy(exp.gameObject, 3.0f);
                    gm.IncrementScore(lootPoints);
                }
            }
            else
            {
                if (health > 1)
                    health--;
                else
                {
                    // Kill the enemy when it has only one life left, create explosion and increment score
                    GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
                    Destroy(exp.gameObject, 3.0f);
                    Destroy(gameObject);
                    gm.IncrementScore(lootPoints);
                }
            }

            // Shows the enemy's health bar only when it takes damage
            if (!healthSlider.gameObject.activeInHierarchy)
                healthSlider.gameObject.SetActive(true);

            // Sets health bar value between 0 to 1
            healthSlider.value = (float)health / maxHealth;

            // Destroys the bullet
            Destroy(collidingBullet.gameObject);
        }
    }

    /// <summary>
    /// Update the new position in Y axis
    /// </summary>
    private void StepDown()
    {
        newPosY = transform.position.y - 0.8f;
    }


    /************************************************************************/
    /* Getters and setters                                                  */
    /************************************************************************/

    public void SetMovingSpeed(float speed)
    {
        movingSpeed = speed;
    }

    public void SetShootTimerRange(float shootTimerMin, float shootTimerMax)
    {
        this.shootTimerMin = shootTimerMin;
        this.shootTimerMax = shootTimerMax;
    }

}
