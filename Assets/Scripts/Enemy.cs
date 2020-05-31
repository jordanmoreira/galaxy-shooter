using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private Player _player;

    private Animator _animator;

    [SerializeField]
    private AudioClip _explosionAudioClip;
    [SerializeField]
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        float randomXPosition = Random.Range(-9.1f, 10.1f);
        transform.position = new Vector3(randomXPosition, 5.9f, 0);

        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();

        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _explosionAudioClip;

    }

    // Update is called once per frame
    void Update()
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
        GameObject laser = GameObject.FindWithTag("Laser");
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _speed = 0;
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            Destroy(laser);
            if (_player != null)
            {
                _player.IncreaseScore(10);
            }
            _speed = 0;
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }
}