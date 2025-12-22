using UnityEngine;

public class SignController : MonoBehaviour
{
    public GameObject[] sign;
    public KeyCode[] Key;
    public bool[] OnOff;
    public Vector3[] innitialPlacement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i =0;i<sign.Length;i++){
            OnOff[i] = false;
            innitialPlacement[i] = sign[i].transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i =0;i<sign.Length;i++){
            if(Input.GetKeyDown(Key[i])){
                OnOff[i] =! OnOff[i];
            }
        }

        for(int j =0;j<sign.Length;j++){
            if(OnOff[j]){
                sign[j].SetActive(true);
                sign[j].GetComponent<AutoMove>().enabled = true;
            }
            else {
                sign[j].transform.localPosition = innitialPlacement[j];
                sign[j].SetActive(false);
                sign[j].GetComponent<AutoMove>().enabled = false;
            }
        }
        
    }
}