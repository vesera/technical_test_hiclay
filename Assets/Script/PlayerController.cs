using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private TMP_Text lifeText;
    [SerializeField] private TMP_Text respawnText;
    [SerializeField] private TMP_Text moneyText;

    [SerializeField]
    private float speed = 10f;

    private Rigidbody2D body;

    private float offsetJump = 1.5f;
    private int jumpCount;
    private int defaultJumpCount = 1;

    [SerializeField] private float interval;
    private int projectileLoop;
    private float nextFire = 0.0f;

    private Vector2 lookDirection = new Vector2(1, 0);

    private Vector3 initialPos = new Vector3(-7.29f, -1.24f, 0f);

    private float horizontalInput;

    private bool jumpUpgraded, shotUpgraded;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject shopText;

    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject shopMenu;

    [HideInInspector] public int health = 3;
    [HideInInspector] public int respawn = 3;
    [HideInInspector] public int money { get; set; }
    [HideInInspector] public bool isJumping { get; set; }
    [HideInInspector] public bool isInShop { get; set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();

        isJumping = false;
        isInShop = false;

        jumpUpgraded = false;
        shotUpgraded = false;

        money = 0;

        jumpCount = defaultJumpCount;
        projectileLoop = 1;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        Vector2 move = new Vector2(horizontalInput, 0);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        if (horizontalInput > 0.01f)
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }
        else if (horizontalInput < -0.01f)
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
        UpgradePowerUp();

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Time.time > nextFire && projectileLoop <2)
            {
                nextFire = Time.time + interval;
                Shoot();
            }
            else if(projectileLoop >= 2)
            {
                StartCoroutine(Shot());
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && isInShop)
        {
            ToggleShopMenu();
        }

        moneyText.text = "Money : " + money;
        respawnText.text = "Respawn : " + respawn;
        lifeText.text = "Life : " + health;
    }

    IEnumerator Shot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + interval;
            for(int i = 0; i < projectileLoop; i++)
            {
                if(i >= 1)
                {
                    yield return new WaitForSeconds(interval/10);
                    Debug.Log(i);
                }

                Shoot();
            }
        }
    }

    private void jump()
    {
        if (jumpCount > 0)
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
                body.velocity = new Vector2(body.velocity.x, (speed * offsetJump));
                jumpCount--;
                isJumping = true;
            //}
        }
    }

    private void checkHealth()
    {
        if (health <= 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        if (respawn > 0)
        {
            gameObject.transform.position = initialPos;
            respawn -= 1;
            health = 3;
            Debug.Log("Respawn Left : " + respawn);
        }
        else
        {
            SceneManager.LoadSceneAsync("GameOver");
        }
    }

    private void Shoot()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, body.position, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 1500);
    }

    private void ToggleShopMenu()
    {
        if (isInShop)
        {
            gameController.ShowShopMenu();
        }
        else
        {
            gameController.HideShopMenu();
        }
    }

    public void UpgradePowerUp()
    {
        if (gameController.isJumpBrought && !jumpUpgraded)
        {
            defaultJumpCount = 2;
            money -= gameController.jumpPrice;
            jumpUpgraded = true;
        }

        if (gameController.isShotBrought && !shotUpgraded)
        {
            projectileLoop = 2;
            money -= gameController.shotPrice;
            shotUpgraded = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Ground")
        {
            jumpCount = defaultJumpCount;
            isJumping = false;
        }

        //if (collision.collider.gameObject.tag == "Boundary")
        //{
        //    Respawn();
        //}

        if (collision.collider.gameObject.tag == "Enemies" || collision.collider.gameObject.tag == "Turret Projectile")
        {
            health -= 1;
            checkHealth();
            Debug.Log("Health : " + health);
        }

        if(collision.collider.gameObject.tag == "Power Up")
        {
            if (collision.collider.gameObject.name == "Coin(Clone)")
            {
                money += 1;
                Debug.Log(money);
                Destroy(collision.collider.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Shop")
        {
            isInShop = true;
            shopText.gameObject.SetActive(true);
        }

        if (collision.gameObject.tag == "Goal")
        {
            SceneManager.LoadSceneAsync("GameOver");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shop")
        {
            shopText.gameObject.SetActive(false);
            isInShop = false;
            ToggleShopMenu();
        }
    }
}
