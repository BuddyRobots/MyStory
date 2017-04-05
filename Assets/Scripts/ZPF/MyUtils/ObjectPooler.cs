using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Maybe better write this to generic type object pooler 
// and use instances of this class to create different types of objects need pooling.
public class ObjectPooler : MonoBehaviour {

	public static ObjectPooler current;
	public GameObject pooledObject;
	public int pooledAmount = 2;
	public bool willGrow = true;

	List<GameObject> pooledObjectList;
	
	void Awake()
	{
		current = this;
	}

	void Start()
	{
		pooledObjectList = new List<GameObject>();
		for (int i = 0; i < pooledAmount; i++)
		{
			GameObject obj = (GameObject)Instantiate(pooledObject);
			obj.SetActive(false);
			pooledObjectList.Add(obj);
		}
	}

	public GameObject GetPooledObject()
	{
		for (int i = 0; i < pooledObjectList.Count; i++)
			if (!pooledObjectList[i].activeInHierarchy)
				return pooledObjectList[i];

		if (willGrow)
		{
			GameObject obj = (GameObject)Instantiate(pooledObject);
			pooledObjectList.Add(obj);
			return obj;
		}

		return null;
	}
}
