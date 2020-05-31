using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float speed = 8.5f;

    // Update is called once per frame
    void Update()
    {
        CalculateSpeed();
        DestroyLaser();
    }

    void CalculateSpeed()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void DestroyLaser()
    {
        if (transform.position.y >= 8)
        {
            Debug.Log(transform.parent);

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }
}