using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float speed = 8.5f;

    private bool _isEnemyLaser = false;

    // Update is called once per frame
    void Update()
    {
        CalculateSpeed();
    }

    // tell the laser that if its fro man enemy it will go down Vector3.down
    void CalculateSpeed()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();

        }
        else if (_isEnemyLaser == true)
        {
            MoveDown();
        }
    }

    public void MoveUp()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y > 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void MoveDown()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    public bool GetEnemyLaser()
    {
        return _isEnemyLaser;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();

        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }
    }
}