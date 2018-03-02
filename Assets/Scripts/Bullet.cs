using UnityEngine;

/// <summary>
/// Base class for bullets
/// </summary>
public class Bullet : MonoBehaviour {

    /// <summary>
    /// Falling speed of the bullet
    /// </summary>
    private float speed = 1f;

    void Update () {
        // Move the bullet every frame
        transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime);
    }

    /// <summary>
    /// When the bullet collides with another bullet
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider collidingBullet)
    {
        // Destroys the bullet if it isn't the same tag than the other bullet
        if((tag == "EnemyBullet" && collidingBullet.gameObject.tag == "Bullet") || (tag == "Bullet" && collidingBullet.gameObject.tag == "EnemyBullet"))
        {
            Destroy(collidingBullet.gameObject);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// When the bullet gets out of the screen
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Boundary")
        {
            // Destroys the bullet when it gets out of the boundary
            Destroy(gameObject);
        }
    }


    /************************************************************************/
    /* Getters and setters                                                  */
    /************************************************************************/

    public void SetSpeed(int speed)
    {
        this.speed = speed;
    }

}
