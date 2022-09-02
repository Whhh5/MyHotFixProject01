using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Runtime.InteropServices;
using BXB.Core;
using System.IO;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float intervalTime = 0.2f;
    [SerializeField] float r = 10.0f;
    [SerializeField] uint bulletCount = 10;
    [SerializeField] List<Transform> targets = new List<Transform>();



    [SerializeField] uint removeIndex = 0;
    [SerializeField] public A_List<GameObject> _List;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(Fire());
        }
        

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject newObj = new GameObject($"game_{_List.countP}");
            if (_List.TryAdd(newObj, out uint index))
            {
                Debug.Log(index);
            }
            else
            {
                Debug.Log("ÃÌº” ß∞‹");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (_List.TryRemoveAtIndex(removeIndex, out GameObject index))
            {
                Debug.Log(index.name);
            }
            else
            {
                Debug.Log("…æ≥˝ ß∞‹");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TestManager.Instance._pool.ClearPool();
        }
    }


    void Start()
    {
    }

    private Vector3 GetRandomPoint(float r)
    {
        return transform.position + new Vector3(UnityEngine.Random.Range(-r, r), UnityEngine.Random.Range(-r, r), UnityEngine.Random.Range(-r, r));
    }


    IEnumerator Fire()
    {
        for (int i = 0; i < 10; i++)
        {
            Addressables.LoadAssetAsync<GameObject>("Bullet").Completed += (asyncOperationsHandle) =>
            {
                if (asyncOperationsHandle.Status != AsyncOperationStatus.Failed)
                {
                    if (TestManager.Instance._pool.Get(asyncOperationsHandle.Result, out Bullet bullet, true))
                    {
                        bullet.transform.position = this.transform.position;
                        StartCoroutine(bullet.Move(bullet.transform.position, GetRandomPoint(r), targets[UnityEngine.Random.Range(0, targets.Count)]));
                    }
                    else
                    {
                        Debug.Log("ªÒ»° ß∞‹");
                    }
                }
            };
            yield return new WaitForSeconds(intervalTime);
        }
        num.Add(2);

    }
    int num = 3;



    public void Gen<T>(T asd)
    {
        var hash = asd.GetHashCode();
        Debug.Log(hash);
    }
}


public static class Extid
{
    public static void Add(this int ut, int a)
    {

    }
}



class Box
{
    private double? length = null;  
    private double? breadth; 
    private double? height;  
    public static Box operator+(Box b, Box c)
    {
        Box box = new Box();





        return box;
    }
}

