using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb;
    private float initializationTime;
    private float timeSinceInstantiated;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        initializationTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeSinceInstantiated = (Time.timeSinceLevelLoad - initializationTime);
        if (timeSinceInstantiated > 10)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
