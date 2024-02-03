using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _enemyPrefab;
    
    [SerializeField]
    private GameObject[] _powerUps;
    
    [SerializeField]
    private GameObject _powerUpsRarePrefab;
    
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private bool _stopSpawning = false;

    private float randomLocation;
   
      

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
        StartCoroutine(RarePowerUpRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 randomLocation = new Vector3(Random.Range(-15f, 15f), 8f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, randomLocation, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5);
        }
    }
    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            
            Vector3 randomLocation = new Vector3(Random.Range(-8f, 8f), 8f, 0);
            int randomPowerup = Random.Range(0, 5);
            GameObject newPowerUp = Instantiate(_powerUps[randomPowerup], randomLocation, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }
    
    IEnumerator RarePowerUpRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        while (_stopSpawning == false)
        {
            Vector3 randomLocation = new Vector3(Random.Range(-8f, 8f), 8f, 0);
            GameObject rareNewPowerUp = Instantiate(_powerUpsRarePrefab, randomLocation, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(30f, 60f));
        }
    }
    
    public void OnPlayerDeath()        
    {
        {
            _stopSpawning = true;
        }
    }
}
