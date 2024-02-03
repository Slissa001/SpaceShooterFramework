using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ememy_Laser : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private float _speed = 6.0f;
    private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.Log("The Player is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        if (transform.position.y < -8.5f)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
        }
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
      if (other.tag == "Player")
        {
            _player.Damage();
            Destroy(this.gameObject);
        }
    }
}
