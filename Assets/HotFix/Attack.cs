using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Runtime.InteropServices;
using BXB.Core;

public class Attack : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float intervalTime = 0.2f;
    [SerializeField] float r;
    [SerializeField] uint bulletCount = 10;
    [SerializeField] List<Transform> targets = new List<Transform>();



    [SerializeField] List<GameObject> lists = new List<GameObject>();

    [SerializeField] List<GameObject> lists2 = new List<GameObject>();

    [SerializeField] uint removeIndex = 0;
    [SerializeField] public A_List<GameObject> _List = new A_List<GameObject>(10);

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
                Debug.Log("Ìí¼ÓÊ§°Ü");
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
                Debug.Log("É¾³ýÊ§°Ü");
            }
        }
    }

    void Start()
    {
        _List = new A_List<GameObject>(10);
        //value = null;
        //GCHandle h2 = GCHandle.Alloc(value, GCHandleType.Pinned);
        //IntPtr addr2 = h2.AddrOfPinnedObject();
        //Debug.Log(addr2.ToString("X"));
    }

    private Vector3 GetRandomPoint(float r)
    {
        return transform.position + new Vector3(UnityEngine.Random.Range(-r, r), UnityEngine.Random.Range(-r, r), UnityEngine.Random.Range(-r, r));
    }

    public bool TryGet(int index, out GameObject obj)
    {
        bool ret = false;

        obj = lists[index];


        return ret;
    }


    IEnumerator Fire()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            Addressables.LoadAssetAsync<GameObject>("Bullet").Completed += (asyncOperationsHandle) =>
            {
                if (asyncOperationsHandle.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
                {
                    var obj = asyncOperationsHandle.Result.GetComponent<Bullet>();
                    Bullet bullet = GameObject.Instantiate(obj, transform.position, Quaternion.identity);
                    StartCoroutine(bullet.Move(bullet.transform.position, GetRandomPoint(r), targets[UnityEngine.Random.Range(0, targets.Count)]));
                }
            };
            yield return new WaitForSeconds(intervalTime);
        }
    }

}
