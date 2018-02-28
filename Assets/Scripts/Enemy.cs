using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    private GameManager gm = null;

    [SerializeField]
    private int loopPoints;

    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private float shootTimer = 0f;
    float timer = 0;

    [SerializeField]
    private Bullet bulletPrefab;

    private Vector3 borderRight;
    private Vector3 borderLeft;
    private float newPosY = 0.0f;

    // Use this for initialization
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        borderRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10));
        borderLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10));
        shootTimer = Random.Range(2.0f, 20.0f);
        newPosY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    { 
        if (transform.position.x >= borderRight.x)
        {
            speed = -speed;
            StepDown();
        }

        if (transform.position.x <= borderLeft.x)
        {
            speed = -speed;
            StepDown();
        }

        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, Mathf.Lerp(transform.position.y, newPosY, 0.06f));

        timer += Time.deltaTime;
        if (timer >= shootTimer)
        {
            timer = 0;
            shootTimer = Random.Range(2.0f, 20.0f);
            Shoot();
        }
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(-90, 0, 0));
        bullet.SetSpeed(-5);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Bullet")
        {

            foreach(Enemy enemy in FindObjectsOfType<Enemy>())
            {
                Destroy(enemy.gameObject);
                gm.IncrementScore(loopPoints);
            }

            //Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }

    private void StepDown()
    {
        newPosY = transform.position.y - 0.7f;
    }

}
