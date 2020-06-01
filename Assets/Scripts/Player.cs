using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    private SpawnManager _spawnManager;

    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 2;

    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;
    private bool _isTripleShotActive = false;
    private bool _isSpeedActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private int _score = 0;

    private UIManager _uiManager;
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _leftFire;
    [SerializeField]
    private GameObject _rightFire;

    [SerializeField]
    private AudioClip _laserShotSoundClip;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    private AudioSource _audioSource;

    public bool _isPlayerOne = false;
    public bool _isPlayerTwo = false;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_gameManager != null)
        {
            if (_gameManager.GetScene() == "Single_Player")
            {
                Debug.Log("Playing in single player mode...");
                transform.position = new Vector3(0, -2.3f, 0);
            }
            else if (_gameManager.GetScene() == "Co-Op_Mode")
            {
                Debug.Log("Playing in coop mode...");
                transform.position = new Vector3(-5.6f, -2.3f, 0);
            }
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }
        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source is NULL.");
        }
        else
        {
            _audioSource.clip = _laserShotSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        IdentifyShooter();
    }

    void CalculateMovement()
    {
        CalculateMovementPlayerOne();
        CalculateMovementPlayerTwo();
    }

    void CalculateMovementPlayerOne()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_isPlayerOne == true)
        {
            transform.Translate(direction * _speed * Time.deltaTime);

            // adding boundaries
            //if (transform.position.y >= 0)
            //{
            //    transform.position = new Vector3(transform.position.x, 0, 0);
            //}
            //else if (transform.position.y <= -3.9f)
            //{
            //    transform.position = new Vector3(transform.position.x, -3.9f, 0);
            //}
            // this is equal to the line below
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

            if (transform.position.x >= 11.2f)
            {
                transform.position = new Vector3(-11.2f, transform.position.y, 0);
            }
            else if (transform.position.x <= -11.2f)
            {
                transform.position = new Vector3(11.2f, transform.position.y, 0);
            }
        }
    }

    void CalculateMovementPlayerTwo()
    {
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");
        //Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_isPlayerTwo == true)
        {
            //transform.Translate(direction * _speed * Time.deltaTime);

            if (Input.GetKey(KeyCode.Keypad8))
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Keypad5))
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Keypad4))
            {
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Keypad6))
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
            }

            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

            if (transform.position.x >= 11.2f)
            {
                transform.position = new Vector3(-11.2f, transform.position.y, 0);
            }
            else if (transform.position.x <= -11.2f)
            {
                transform.position = new Vector3(11.2f, transform.position.y, 0);
            }
        }
    }

    public void IdentifyShooter()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _isPlayerOne == true)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Keypad0) && Time.time > _canFire && _isPlayerTwo == true)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (_isTripleShotActive == false)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + (_laserPrefab.transform.up * 0.8f), Quaternion.identity);
        }

        if (_isTripleShotActive == true)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_tripleShotPrefab, transform.position + (_tripleShotPrefab.transform.up * 0.8f), Quaternion.identity);
        }
        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            PlayerShieldActive(false);

            return;
        }

        _lives -= 1;
        _uiManager.UpdateLives(_lives);

        switch (_lives)
        {
            case 1:
                _lives = 1;
                _rightFire.SetActive(true);
                break;
            case 2:
                _lives = 2;
                _leftFire.SetActive(true);
                break;
        }

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            _audioSource.clip = _explosionSoundClip;
            _audioSource.Play();
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine(5f));
    }

    IEnumerator TripleShotPowerDownRoutine(float waitTime)
    {
        yield return new WaitForEndOfFrame();

        while (_isTripleShotActive == true)
        {
            yield return new WaitForSecondsRealtime(waitTime);
            _isTripleShotActive = false;
        }
    }

    public void SpeedActive()
    {
        _isSpeedActive = true;
        _speed = _speed * _speedMultiplier;

        StartCoroutine(SpeedPowerDownRoutine(5f));
    }

    IEnumerator SpeedPowerDownRoutine(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        _isSpeedActive = false;
        _speed = _speed / _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        PlayerShieldActive(true);
    }

    public void PlayerShieldActive(bool activate)
    {
        _playerShield.SetActive(activate);
    }

    public void IncreaseScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public int GetScore()
    {
        return _score;
    }
}