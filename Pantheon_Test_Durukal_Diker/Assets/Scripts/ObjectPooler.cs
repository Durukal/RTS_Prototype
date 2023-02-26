using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPooler : MonoBehaviour {
    //Parent Objects for Pooled Objects to clear visual garbage in the hierarchy
    public Transform Buildings;
    public Transform Units;
    public static ObjectPooler SharedInstance;
    public List<GameObject> pooledObjects;
    public List<ObjectPoolItem> itemsToPool;


    private void Awake() {
        SharedInstance = this;
    }

    void Start() {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool) {
            for (int i = 0; i < item.amountToPool; i++) {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                if (obj.CompareTag("PowerPlant") || obj.CompareTag("Barracks")) {
                    obj.transform.parent = Buildings;
                    obj.SetActive(false);
                } else if (obj.CompareTag("Peasant") || obj.CompareTag("Infantry") || obj.CompareTag("Knight")) {
                    obj.transform.parent = Units;
                    obj.SetActive(false);
                }

                pooledObjects.Add(obj);
            }
        }
    }


    void Update() { }

    public GameObject GetPooledObject(string tag) {
        for (int i = 0; i < pooledObjects.Count; i++) {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag) {
                return pooledObjects[i];
            }
        }

        foreach (ObjectPoolItem item in itemsToPool) {
            if (item.objectToPool.tag == tag) {
                if (item.shouldExpand) {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }

        return null;
    }
}