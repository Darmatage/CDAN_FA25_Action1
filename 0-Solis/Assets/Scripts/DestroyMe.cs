using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyMe : MonoBehaviour
{
	public float destroyTime = 0.8f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(TimeToDestroy());
    }

    // Update is called once per frame
    IEnumerator TimeToDestroy()
    {
        yield return new WaitForSeconds(destroyTime);
		Destroy(gameObject);
    }
}
