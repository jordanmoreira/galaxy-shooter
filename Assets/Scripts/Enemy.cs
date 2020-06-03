using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool _enemyShot = false;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    private Animator _animator;
    private Player _player;

    [SerializeField]
    private float _speed = 4f;

    [SerializeField]
    private AudioClip _explosionAudioClip;
    [SerializeField]
    private AudioClip _laserAudioClip;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;

    // Start is called before the first frame update
    void Start()
    {
        float randomXPosition = Random.Range(-9.1f, 10.1f);
        transform.position = new Vector3(randomXPosition, 5.9f, 0);

        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        Shoot();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.8f)
        {
            float randomXPosition = Random.Range(-9.1f, 10.1f);
            transform.position = new Vector3(randomXPosition, 5.9f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            _speed = 0;
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.clip = _explosionAudioClip;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            _speed = 0;
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.clip = _explosionAudioClip;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);

            _player.IncreaseScore(10);
        }
    }

    public void Shoot()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
            _enemyShot = true;
        }

        if (_enemyShot == true)
        {
            _audioSource.clip = _laserAudioClip;
            _audioSource.Play();
        }

        _enemyShot = false;
    }
}