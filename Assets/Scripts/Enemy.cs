﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    private GameManager gm = null;

    [Header("General")]
    [SerializeField, Tooltip("How many health it have")]
    private int health = 1;

    [SerializeField, Tooltip("How many points per kill")]
    private int lootPoints = 10;

    [SerializeField, Tooltip("Lateral moving speed")]
    private float movingSpeed = 1.0f;

    [SerializeField, Tooltip("Explosion prefab to spawn at death")]
    private GameObject explosion;


    [Header("Projectiles")]
    [SerializeField]
    private Bullet bulletPrefab;

    [SerializeField]
    private int bulletSpeed = 5;

    private float shootTimer = 0f;
    float timer = 0f;

    private Vector3 borderRight;
    private Vector3 borderLeft;
    private float newPosY = 0.0f;

    private bool hasTouchEdgeOfScreen = false;


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
            movingSpeed = -Mathf.Abs(movingSpeed);
            hasTouchEdgeOfScreen = true;
        }

        if (transform.position.x <= borderLeft.x)
        {
            movingSpeed = Mathf.Abs(movingSpeed);
            hasTouchEdgeOfScreen = true;
        }

        if(transform.position.x >= borderLeft.x && transform.position.x <= borderRight.x && hasTouchEdgeOfScreen)
        {
            StepDown();
            hasTouchEdgeOfScreen = false;
        }

        transform.position = new Vector3(transform.position.x + movingSpeed * Time.deltaTime, Mathf.Lerp(transform.position.y, newPosY, 0.06f));

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
        bullet.SetSpeed(-Mathf.Abs(bulletSpeed));
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Bullet")
        {
            if (gm.GodMode())
            {
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
                    GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
                    Destroy(exp.gameObject, 3.0f);
                    Destroy(gameObject);
                    gm.IncrementScore(lootPoints);
                }
            }
            Destroy(other.gameObject);
        }
    }

    private void StepDown()
    {
        newPosY = transform.position.y - 0.7f;
    }

}
