using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Shake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public IEnumerator Shake(float duration, float magnitude)
      {
            Vector3 originalPos = transform.position;
            float timeShaking = 0.0f;

            while (timeShaking < duration)
            {
                float x = Random.Range(1f, -1f) * magnitude;
                float y = Random.Range(1f, -1f) * magnitude;

                transform.position = new Vector3(x, y, originalPos.z);
                timeShaking += Time.deltaTime;
                yield return null; 
            }

            transform.position = originalPos;
      }
    
}
