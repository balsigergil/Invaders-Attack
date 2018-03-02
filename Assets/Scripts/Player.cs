using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private int health = 3;

    [SerializeField]
    private Bullet bulletPrefab;

    [SerializeField]
    private GameObject bulletSpawn;

    [SerializeField]
    private int bulletSpeed;

    [SerializeField]
    private GameManager gm;

    [SerializeField]
    private GameObject explosion;

    private void Start()
    {
        gm.SetHealthText(health.ToString());
    }

    // Update is called once per frame
    void Update () {

        if (!gm.GetIsPaused())
        {
            // For Android
            if(Application.platform == RuntimePlatform.Android)
            {

                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (i >= 2)
                        break;
                
                    Touch touch = Input.GetTouch(i);
                    if(touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved){
                        if (touch.position.y <= Screen.height * 0.15)
                        {
                            // move player
                            Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                            transform.position = new Vector3(touchedPos.x, transform.position.y);
                        }
                        else
                        {
                            Fire();
                        }

                    }
                }
            }

            // For Windows (testing)
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                Vector3 newPos = new Vector3(touchedPos.x, transform.position.y);

                transform.position = newPos;

                if (Input.GetMouseButtonDown(0))
                {
                    if (Input.mousePosition.y >= Screen.height * 0.15)
                    {
                        Fire();
                    }
                }
            }
        }

	}

    void Fire()
    {
        if (GameObject.FindGameObjectsWithTag("Bullet").Length == 0)
        {
            Bullet bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            bullet.SetSpeed(bulletSpeed);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EnemyBullet")
        {
            if (health == 1)
            {
                gm.Loose();
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            health--;
            gm.SetHealthText(health.ToString());
            Destroy(other.gameObject);
        }
    }
}
