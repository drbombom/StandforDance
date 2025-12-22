using UnityEngine;

public class ObjectsGenerator : MonoBehaviour
{
    public GameObject[] prefabs;
    public int maxObjects;
    public float distance;
    public float maxDistance;
    public float nextDistance;
    public float maxHeigh;
    public float maxWeight;
    public Vector3 Drif;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float rWeight = 0;
        float rHeigh = 0;
        for (var i = 0; i < maxObjects; i++)
        {
            int rObject = Random.Range(0, prefabs.Length);
            if(maxDistance!=0){
                float rDistance = Random.Range(0, maxDistance);
            }
            
            if(maxHeigh!=0){
                rHeigh = Random.Range(0, maxHeigh);
            }

            if(maxWeight!=0){
                rWeight = Random.Range(0, maxWeight);
            }
            
            
            nextDistance = rWeight;
            GameObject clone = Instantiate(prefabs[rObject], transform.position-Drif+new Vector3(0, 0.1f, i * distance+nextDistance), prefabs[rObject].transform.rotation);
            clone.transform.localScale += new Vector3 (rWeight,rHeigh,rWeight);
            if (clone.GetComponent<Renderer>()!=null){
                clone.GetComponent<Renderer>().material.color += Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            }

            //Debug.Log(clone.GetComponent<Renderer>());
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
