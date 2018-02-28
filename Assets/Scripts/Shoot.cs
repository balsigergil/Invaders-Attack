using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private GameObject bulletSpawn;

    [SerializeField]
    private GameManager gm;

    [SerializeField]
    private GameObject explosion;
	
	// Update is called once per frame
	void Update () {

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

    void Fire()
    {
        int i = 0;
        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            i++;
        }

        if (i == 0)
            Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EnemyBullet")
        {
            gm.Loose();
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
