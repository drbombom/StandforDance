using UnityEngine;

public class AutoMove : MonoBehaviour
{
    public Vector3 Pos_Direction;
    public float speedTranlate;
    public float maxSpeed;
    public bool FnB;
    public bool Reverse;
    public float smoothTime = 0.3f;
    float xVelocity = 0.0f;
    float yVelocity = 0.0f;
    float zVelocity = 0.0f;
    public Vector3 drift;
    public GameObject target;
    public float Timer;
    public float Rate;
    public float nextTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Timer = Time.time;
        float rSpeed;
        if(maxSpeed!=0){
            rSpeed = Random.Range(1, maxSpeed);
            speedTranlate = rSpeed;
        }

        if(FnB){
            if (Reverse){
                target.transform.position += drift;
            }
            else{
                target.transform.position -= drift;
            }
            float newPositionX = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref xVelocity, smoothTime);
            float newPositionY = Mathf.SmoothDamp(transform.position.y, target.transform.position.y, ref yVelocity, smoothTime);
            float newPositionZ = Mathf.SmoothDamp(transform.position.z, target.transform.position.z, ref zVelocity, smoothTime);
            transform.position = new Vector3(newPositionX, newPositionY, newPositionZ);

        }
        else {
            transform.Translate(Pos_Direction * speedTranlate  * Time.deltaTime, Space.World);
        }

        if (Timer>nextTime){
            Reverse = !Reverse;
            nextTime = Timer + Rate;
        }
        
    }
}
