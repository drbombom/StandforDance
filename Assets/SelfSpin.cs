using UnityEngine;

public class SelfSpin : MonoBehaviour
{
    public KeyCode spinKey;
    public float spinSpeed = 50f;
    public float spinAngle = 90f;
    public Vector3 angle;
    public int counter;
    float currentAngle;
    float nextAngle;
    void Start()
    {
        angle = transform.rotation.eulerAngles;
        nextAngle = 0f;
    }
    void Update()
    {
        if(Input.GetKeyDown(spinKey))
        {
            counter++;
            nextAngle += spinAngle;
        }

        if( currentAngle < nextAngle)
        {            
            currentAngle += Time.deltaTime * spinSpeed;
            transform.localRotation = Quaternion.Euler(Vector3.forward * currentAngle);
        }

    }
}
