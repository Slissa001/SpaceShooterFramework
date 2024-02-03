using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnmanager;

    void Start()
    {
        _spawnmanager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //rotate the asteroid on the zed 
        transform.Rotate(Vector3.forward * -1 * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            GameObject newExplosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            Destroy(newExplosion, 4.0f);
            _spawnmanager.StartSpawning();
        }
    }

}
