using System.Collections;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    public GameObject prefab;
    public KeyCode stepKey;
    public float count;
    public float setpsWidth;
    public float distance;
    public float interval;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(stepKey))
        {
            StartCoroutine(Step(interval));
        }
        
    }

    IEnumerator Step(float interval)
    {
        float d = 0;
        for(int i = 0; i < count; i++)
        {
            setpsWidth *= -1;
            d += distance;
            Vector3 v = Vector3.left * setpsWidth + Vector3.down * d;
            GameObject g = Instantiate(prefab,gameObject.transform);
            g.transform.localPosition = v;
            Destroy(g, 3f);
            yield return new WaitForSeconds(interval);
        }
    }
}
