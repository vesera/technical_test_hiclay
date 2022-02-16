using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject coinPrefab;

    private Vector2 lookDirection = new Vector2(-1, 0);
    private Rigidbody2D body;

    [SerializeField] public int healthTurret;
    [SerializeField] private int turretSpawnCoin;

    [SerializeField] private int delayTurret;
    [SerializeField] private int delayShoot;
    [HideInInspector] private float offsetShoot = 0;
    [SerializeField] private int turretShootLoop;

    private int randMin = 0;
    private int randMax = 4;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        for(int i = 0; i < turretShootLoop; i++)
        {
            InvokeRepeating("Shoot", delayShoot + offsetShoot, delayTurret);
            offsetShoot += 0.05f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Projectile")
        {
            healthTurret -= 1;

            if(healthTurret == 0) {

                Destroy(gameObject);

                for (int i = 0; i < turretSpawnCoin; i++)
                {
                    GameObject coinObject = Instantiate(coinPrefab, body.position, Quaternion.identity);

                    Coin coin = coinObject.GetComponent<Coin>();

                    int randomX = Random.Range(randMin, randMax);

                    coin.SpawnCoin(new Vector2(randomX, 1), 100);
                }

            }
        }
    }

    private void Shoot()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, body.position, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, (1200));
    }
}