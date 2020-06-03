using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float speed = 8.5f;

    private bool _isEnemyLaser = false;
    private bool _isPlayerOneLaser = false;
    private bool _isPlayerTwoLaser = false;


    // Update is called once per frame
    void Update()
    {
        CalculateSpeed();
    }

    // tell the laser that if its from an enemy it will go down Vector3.down
    private void CalculateSpeed()
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
    private void MoveUp()
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
    private void MoveDown()
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

    public void AssignPlayerOneLaser()
    {
        _isPlayerOneLaser = true;
    }
    public bool GetPlayerOneLaser()
    {
        return _isPlayerOneLaser;
    }

    public void AssignPlayerTwoLaser()
    {
        _isPlayerTwoLaser = true;
    }
    public bool GetPlayerTwoLaser()
    {
        return _isPlayerTwoLaser;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "PlayerOne" || other.tag == "PlayerTwo") && _isEnemyLaser == true)
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }
    }
}