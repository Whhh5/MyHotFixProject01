using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] LayerMask targetLayer;

    private void Start()
    {
        Destroy(gameObject, 15f);
    }
    public IEnumerator Move(Vector3 start, Vector3 midPoint, Transform target)
    {
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            Vector3 p1 = Vector3.Lerp(start, midPoint, i);
            Vector3 p2 = Vector3.Lerp(midPoint, target.position, i);
            Vector3 p = Vector3.Lerp(p1, p2, i);

            yield return StartCoroutine(MoveToPoint(p));
        }
        yield return false;
    }

    public IEnumerator MoveToPoint(Vector3 p)
    {
        yield return null;
        while (Vector3.Distance(transform.position, p) > 0.1f)
        {
            Vector3 dir = p - transform.position;
            transform.up = dir;
            transform.position = Vector3.MoveTowards(transform.position, p, Time.deltaTime * speed);
            yield return null;
        }
        yield return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        int colLayer = (int)Mathf.Pow(2, collision.gameObject.layer);
        int targetLayer = this.targetLayer;
        if ((colLayer & targetLayer) != 0)
        {
            Destroy(gameObject);
            StopAllCoroutines();
        }
    }
}
