using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private AudioClip _powerSFX;
    
    
    [SerializeField]
    private float _speed = 3.0f;
    //ID for powerups
    // 0 = Triple Shot
    // 1 = Speed
    // 2 = Shields
    // 3 = Ammo
    // 4 = Health

    [SerializeField]
    private int _powerUpID;

    Player player;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float RandomP = Random.Range(8.0f, -8.0f);
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.4f)
        {
            Destroy(this.gameObject);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_powerSFX, transform.position);
            if (player != null)
            {
                switch(_powerUpID)
                {
                    case 0: 
                        player.tripleShotActivate();
                        break;
                    case 1:
                        player.SpeedBoostActivate();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AmmoRenewActivate();
                        break;
                    case 4:
                        player.AddLife();
                        break;
                    case 5:
                        player.FireSShotActivate();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            Destroy(this.gameObject);
        }            
    }
       
}
