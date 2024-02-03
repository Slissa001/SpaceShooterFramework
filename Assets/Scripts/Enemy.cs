using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    private Player _player;
    private float _fireRate = 3.0f;
    private float _canFire = -1.0f;
    private Animator _anim;
    [SerializeField]
    private AudioClip _enemyExplodefx;
    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        
        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The Audiosource for the Enemy is NULL");
        }

        _audioSource.clip = _enemyExplodefx;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();   

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
        }
    }
    void CalculateMovement()
    {
        //if at the bottom of the screen respawn at top
        //optional respawn at random x position
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        float Randomx = Random.Range(8.0f, -8.0f);


        if (transform.position.y < -5.4f)
        {
            transform.position = new Vector3(Randomx, 7.5f, 0);
        }

    }
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 1;
            _audioSource.Play();
            Destroy(this.gameObject, 2.0f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 1;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.0f);
        }
    }
}
