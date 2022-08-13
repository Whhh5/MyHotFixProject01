using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameV3 : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed;
 
    void Start()
    {
        
    }


    void Update()
    {
        Vector3 v1 = transform.position;
        transform.position = Vector3.Lerp(v1, target.position, speed * Time.deltaTime);

    }
}
