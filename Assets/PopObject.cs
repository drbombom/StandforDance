using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PopObject : MonoBehaviour
{
    public KeyCode popKey;
    public KeyCode switchKey;
    public GameObject[] ObjectDesk;
    public GameObject popUpPrefab;
    public float popUpSpeed = 2f;
    public float popUpDistance = 1f;
    public List<GameObject> popUpInstances = new List<GameObject>();
    public bool rythemMode = false;
    int switchIndex = 0;
    
    void Update()
    {
        if(Input.GetKeyDown(popKey))
        {
            GameObject pop = Instantiate(popUpPrefab, this.gameObject.transform);
            popUpInstances.Add(pop);

            if (popUpInstances.Count > 10)
            {
                Destroy(popUpInstances[0]);
                popUpInstances.RemoveAt(0);
            }
        }
        if(Input.GetKeyDown(switchKey))
        {
            switchIndex ++;
            if(switchIndex > ObjectDesk.Length - 1)
                switchIndex = 0;
            popUpPrefab = ObjectDesk[switchIndex];
        }

        if(!rythemMode)
        {
            if((popUpInstances.Count > 0)&& popUpInstances[popUpInstances.Count-1].transform.localPosition.y <= popUpDistance)
            {
                foreach (GameObject popUp in popUpInstances)
                {
                    popUp.transform.Translate(Vector3.up * Time.deltaTime * popUpSpeed);
                }
            }
        }
        if(rythemMode)
        {
            if((popUpInstances.Count > 0)&& popUpInstances[popUpInstances.Count-1].transform.localPosition.y <= gameObject.transform.position.y + popUpDistance)
            {
                foreach (GameObject popUp in popUpInstances)
                {
                    popUp.transform.Translate(Vector3.up * Time.deltaTime * popUpSpeed);
                }
            }
        }
    
    }
}
