using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 InitialPoint;
    public GameObject[] targets;
    public int target;
    public KeyCode[] Tn;
    public Transform[] placements;
    public int place;
    public KeyCode[] Pn;
    public Vector3 DefaultSpeed;
    public float speedTranlate;
    public float speedRotate;
    public bool m_WorldSpace;
    public bool m_Lookat;
    public Vector3 Pos_Direction;
    public Vector3 Rot_Direction;
    public Vector3 Gap;
    public KeyCode[] Move;

    public float smoothTime = 0.3f;
    float xVelocity = 0.0f;
    float yVelocity = 0.0f;
    float zVelocity = 0.0f;

    float smooth = 5.0f;
    public GameObject cart;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Pos_Direction = Vector3.zero;
        Rot_Direction = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //TargetWho();
        StandWhere();
        Pos_Direction = Vector3.zero;
        Rot_Direction = Vector3.zero;


        if (Input.GetKey(Move[0])){
            Pos_Direction += Vector3.forward;
        }
        if (Input.GetKey(Move[2])){
            Pos_Direction += Vector3.back;
        }
        ////
        if (Input.GetKeyDown(Move[1])){
            targets[target].transform.position += Vector3.left*Gap.x;
            //Pos_Direction += Vector3.left*Gap.x;
        }
        if (Input.GetKeyDown(Move[3])){
            targets[target].transform.position += Vector3.right*Gap.x;
            //Pos_Direction += Vector3.right*Gap.x;
        }
        if (Input.GetKeyDown(Move[4])){
            targets[target].transform.position += Vector3.down*Gap.y;
            //Pos_Direction += Vector3.down*Gap.y;

        }
        if (Input.GetKeyDown(Move[5])){
            targets[target].transform.position += Vector3.up*Gap.y;
            //Pos_Direction += Vector3.up*Gap.y;
        }
        ////
        if (Input.GetKey(Move[6])){
            Rot_Direction += Vector3.left;
        }
        if (Input.GetKey(Move[7])) {
            Rot_Direction += Vector3.forward;
        }
        if (Input.GetKey(Move[8])){
            Rot_Direction += Vector3.right;
        }
        if (Input.GetKey(Move[9])) {
            Rot_Direction += Vector3.back;
        }
        if (Input.GetKey(Move[10])){
            Rot_Direction += Vector3.down;
        }
        if (Input.GetKey(Move[11])) {
            Rot_Direction += Vector3.up;
        }


        ////
        float newPositionX = Mathf.SmoothDamp(transform.position.x, targets[target].transform.position.x, ref xVelocity, smoothTime);
        float newPositionY = Mathf.SmoothDamp(transform.position.y, targets[target].transform.position.y, ref yVelocity, smoothTime);
        float newPositionZ = Mathf.SmoothDamp(transform.position.z, targets[target].transform.position.z, ref zVelocity, smoothTime);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cart.transform.position = InitialPoint;
            transform.position = InitialPoint;
            newPositionX = InitialPoint.x;
            newPositionY = InitialPoint.y;
            newPositionZ = InitialPoint.z;
            targets[target].transform.position = InitialPoint;;
    
            //m_WorldSpace = !m_WorldSpace;
        }
        
        transform.position = new Vector3(newPositionX, newPositionY, newPositionZ);
        
        ////
        Quaternion uniformRotate = Quaternion.Euler((int)transform.rotation.x, (int)transform.rotation.y, (int)transform.rotation.z);
        cart.transform.rotation = Quaternion.Slerp(transform.rotation, uniformRotate,  Time.deltaTime * smooth);
        
        if(m_Lookat){
            transform.LookAt(targets[target].transform);
            m_Lookat = !m_Lookat;
        }
        else {
            
        }

        ////
        if (m_WorldSpace){
            cart.transform.Translate(Pos_Direction * speedTranlate  * Time.deltaTime, Space.World);
            cart.transform.Rotate(Rot_Direction * speedRotate  * Time.deltaTime, Space.World);
        }
        else {
            cart.transform.Translate(Pos_Direction * speedTranlate  * Time.deltaTime, Space.Self);
            cart.transform.Rotate(Rot_Direction * speedRotate  * Time.deltaTime, Space.Self);
        }
        
        cart.transform.Translate(DefaultSpeed * Time.deltaTime, Space.World);
        ////
        
    }

    void TargetWho(){
        for (int i = 0; i < targets.Length; i++)
        {
            if (Input.GetKey(Tn[i]))
            {
                target = i;
                m_Lookat = true;
            }
        }
    }

    void StandWhere(){
        for (int i = 0; i < placements.Length; i++)
        {
            if (Input.GetKey(Pn[i]))
            {
                place = i;
                cart.transform.position = placements[place].position;
            }
        }
    }
    void FreeSpaceMoving(){
        if (Input.GetKey(Move[0])){
            Pos_Direction += Vector3.forward;
        }
        if (Input.GetKey(Move[1])){
            Pos_Direction += Vector3.left;
        }
        if (Input.GetKey(Move[2])){
            Pos_Direction += Vector3.back;
        }
        if (Input.GetKey(Move[3])){
            Pos_Direction += Vector3.right;
        }
        if (Input.GetKey(Move[4])){
            Pos_Direction += Vector3.down;
        }
        if (Input.GetKey(Move[5])){
            Pos_Direction += Vector3.up;
        }
        ////
        if (Input.GetKey(Move[6])){
            Rot_Direction += Vector3.left;
        }
        if (Input.GetKey(Move[7])) {
            Rot_Direction += Vector3.forward;
        }
        if (Input.GetKey(Move[8])){
            Rot_Direction += Vector3.right;
        }
        if (Input.GetKey(Move[9])) {
            Rot_Direction += Vector3.back;
        }
        if (Input.GetKey(Move[10])){
            Rot_Direction += Vector3.down;
        }
        if (Input.GetKey(Move[11])) {
            Rot_Direction += Vector3.up;
        }
        ////
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_WorldSpace = !m_WorldSpace;
        }
        if (m_WorldSpace){
            transform.Translate(Pos_Direction * speedTranlate  * Time.deltaTime, Space.World);
            transform.Rotate(Rot_Direction * speedRotate  * Time.deltaTime, Space.World);
        }
        else {
            transform.Translate(Pos_Direction * speedTranlate  * Time.deltaTime, Space.Self);
            transform.Rotate(Rot_Direction * speedRotate  * Time.deltaTime, Space.Self);
        }
        ////
        transform.Translate(DefaultSpeed * Time.deltaTime, Space.World);
        ////
        if(m_Lookat){
            transform.LookAt(targets[target].transform);
            m_Lookat = !m_Lookat;
        }
        else {
            
        }
        ////
        float newPositionX = Mathf.SmoothDamp(transform.position.x, (int)transform.position.x, ref xVelocity, smoothTime);
        float newPositionY = Mathf.SmoothDamp(transform.position.y, (int)transform.position.y, ref yVelocity, smoothTime);
        float newPositionZ = Mathf.SmoothDamp(transform.position.z, (int)transform.position.z, ref zVelocity, smoothTime);
        transform.position = new Vector3(newPositionX, newPositionY, newPositionZ);
        ////
        Quaternion uniformRotate = Quaternion.Euler((int)transform.rotation.x, (int)transform.rotation.y, (int)transform.rotation.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, uniformRotate,  Time.deltaTime * smooth);
    }
}