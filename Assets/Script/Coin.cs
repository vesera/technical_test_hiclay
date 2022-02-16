using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Rigidbody2D rb;
    private float initializationTime;
    private float timeSinceInstantiated;

    public GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        initializationTime = Time.timeSinceLevelLoad;
    }
    void FixedUpdate()
    {
        timeSinceInstantiated = (Time.timeSinceLevelLoad - initializationTime);
        if (timeSinceInstantiated > 10)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    public void SpawnCoin(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
    }
}