using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Player : MonoBehaviour

{
    // Start is called before the first frame update

    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _speedBoost = 2.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleshotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    private Vector3 _tripleshotOffset = new Vector3(0, -1.05f, 0);
    private Vector3 _laserOffset = new Vector3(0, 1.05f, 0);
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    private SpawnManager _spawnManager;

    [SerializeField]
    private int _thrusterBoost = 2;

    [SerializeField]
    private int _score;

    [SerializeField]
    private int _shieldLife = 3;

    [SerializeField]
    public int _ammo = 15;

    [SerializeField]
    private float _thrusterFeul = 100.0f;

    [SerializeField]
    private float _thrusterFuelUse = 10.0f;

    [SerializeField]
    private float _thrusterRecharge = 0.1f;

    private float _durationShake = 0.3f;
    private float _magnitudeShake = 0.4f;

    [SerializeField]
    private SpriteRenderer _shieldRenderer;

    private UIManager _uiManager;
    private Camera_Shake _cameraShake;

    private bool _tripleShotActive = false;
    private bool _sShotActive = false;
    private bool _speedBoostActive = false;
    private bool _playerShieldActive = false;
    private bool _barRecharge = false;
    

    [SerializeField]
    private AudioClip _laserSFX;
    private AudioSource _audioSource;


    void Start()
    {
        //take the current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);

        
        _shieldRenderer = this.transform.Find("Shield_visualizer").GetComponent<SpriteRenderer>();

        if (_shieldRenderer == null)
        {
            Debug.Log("The Renderer is NULL");
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.Log("The Spawn Manager is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        _cameraShake = GameObject.Find("Camera_Shake").GetComponent<Camera_Shake>();

        if (_cameraShake == null)
        {
            Debug.LogError("The Camera_Shake component is NULL");
        }

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("The audio source for the Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSFX;
        }


    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_ammo == 0)
            {
                _uiManager.OutOfBullets();
                return;
            }
            FireLaser();
        }
    }
    
    
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        transform.Translate(direction * _speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed *= _thrusterBoost;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed /= _thrusterBoost;
        }

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x >= 13)
        {
            transform.position = new Vector3(-13, transform.position.y, 0);
        }
        else if (transform.position.x <= -13)
        {
            transform.position = new Vector3(13, transform.position.y, 0);
        }

        if (Input.GetKey(KeyCode.LeftShift) && _thrusterFeul > 0.01f && _speedBoostActive == false)
        {
            //moves spaceship at boosted speed
            transform.Translate(direction * _speed * _thrusterBoost * Time.deltaTime);

            //Removes Feul
            _thrusterFeul = Mathf.MoveTowards(_thrusterFeul, 0.0f, _thrusterFuelUse * Time.deltaTime);

            //Updates UI
            _uiManager.UpdateThrusters(_thrusterFeul);
        }
        else
        {
            //moves Spaceship at a regular spped
            transform.Translate(direction * _speed * Time.deltaTime);
            StartCoroutine(FeulRecharge());
        }        
    }

    IEnumerator FeulRecharge()
    {
        _barRecharge = true;

        while (_barRecharge == true)
        {
            _thrusterFeul = Mathf.MoveTowards(_thrusterFeul, 100.0f, _thrusterRecharge * Time.deltaTime);
            _uiManager.UpdateThrusters(_thrusterFeul);
            if (_thrusterFeul == 100.0f)
            {
                _barRecharge = false;
            }

            yield return null;
    }
      }

    void FireLaser()
    {
        _ammo --;
        _uiManager.UpdateAmmo(_ammo);
        _canFire = Time.time + _fireRate;

        if (_tripleShotActive == true && _sShotActive == false)
        {
            Instantiate(_tripleshotPrefab, transform.position + _tripleshotOffset, Quaternion.identity);
        }
        else if (_tripleShotActive == false && _sShotActive == true)
        {
            FireSShot();
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
        }

        _audioSource.Play();
        //play laser audioclip here
    }
                
    void FireSShot()
    {
        Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
        Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.Euler(0, 0, 45f));
        Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.Euler(0, 0, 90f));
        Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.Euler(0, 0, 135f));
        Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.Euler(0, 0, -180f));
        Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.Euler(0, 0, -135f));
        Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.Euler(0, 0, -45f));
        Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.Euler(0, 0, -90f));
    }
     
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void ShieldActive()
    {
        _playerShieldActive = true;
        _shieldVisualizer.SetActive(true);
        _shieldLife = 3;
        _shieldRenderer.color = Color.blue;
    }
    
    public void Damage()
    {
        if (_playerShieldActive == true && _shieldLife > 0)
        {
            _shieldLife--;

            switch (_shieldLife)
            {
                case 0:
                    _playerShieldActive = false;
                    _shieldVisualizer.SetActive(false);
                    break;
                case 1:
                    _shieldRenderer.color = Color.red;
                    break;
                case 2:
                    _shieldRenderer.color = Color.yellow;
                    break;
                case 3:
                    _shieldRenderer.color = Color.blue;
                    break;
            }
            return;
        }

                                
        _lives--; //lives = _lives - 1
        StartCoroutine(_cameraShake.Shake(_durationShake, _magnitudeShake));
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
            _leftEngine.SetActive(false);
        }

        if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void AddLife()
    {
        _lives += 1;
        _uiManager.UpdateLives(_lives);

        if (_lives >= 3)
        {
            _lives = 3;
        }
        
        if (_lives > 2)
        {
            _leftEngine.SetActive(false);
            _rightEngine.SetActive(false);
        }
        if (_lives == 2)
        {
            _leftEngine.SetActive(false);
        }
    }
    
    public void tripleShotActivate()
    {
        _tripleShotActive = true;
        StartCoroutine(tripleShotPowerDownRoutine());
        //start the coroutine for the tripleshot to power down after so many seconds
    }
    IEnumerator tripleShotPowerDownRoutine()
    {
        while (_tripleShotActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            _tripleShotActive = false;
        }
    }
    public void FireSShotActivate()
    {
        _sShotActive = true;
        StartCoroutine(SShotPowerDownRoutine());
    }
    IEnumerator SShotPowerDownRoutine()
    {
        while (_sShotActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            _sShotActive = false;
        }
    }
    public void SpeedBoostActivate()
    {
        _speedBoostActive = true;
        _speed *= _speedBoost;
        StartCoroutine(SpeedBoostCoolDownRoutine());        
    }
    IEnumerator SpeedBoostCoolDownRoutine()
    {
       yield return new WaitForSeconds(5.0f);
       _speedBoostActive = false;
       _speed /= _speedBoost;
    }

    public void AmmoRenewActivate()
    {
        _ammo = 15;
        _uiManager.UpdateAmmo(_ammo);
        
    }
  
 }
