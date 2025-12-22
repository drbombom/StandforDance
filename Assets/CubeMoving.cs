using UnityEngine;

public class CubeMoveing : MonoBehaviour
{
    public KeyCode moveKey;
    public int counter;
    public float speed = 5f;
    public float moveDistance = 5f;
    public Vector3 Position;

    void Start()
    {
        Position = transform.localPosition;
    }
    void Update()
    {
        if(Input.GetKeyDown(moveKey))
        {
            counter++;
        }
        switch(counter % 4)
        {
            case 0:
                if( transform.localPosition.x < Position.x + moveDistance)
                {
                    transform.Translate(Vector3.right * Time.deltaTime * speed);
                }
                break;
            case 1:
                if( transform.localPosition.y > Position.y - moveDistance)
                {                    
                    transform.Translate(Vector3.down * Time.deltaTime * speed);
                }
                break;
            case 2:
                if( transform.localPosition.x > Position.x - moveDistance)
                {
                    transform.Translate(Vector3.left * Time.deltaTime * speed);
                }
                break;
            case 3:
                if( transform.localPosition.y < Position.y + moveDistance)
                {
                    transform.Translate(Vector3.up * Time.deltaTime * speed);
                }
                break;
        }
    }
}
