using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform center; 
    public float radius;
    public float angle;      
    public float speed;     
    

    // Update is called once per frame
    void Update()
    {
        angle += speed * Time.deltaTime;

        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        Vector3 orbitPos = center.position + new Vector3(x, 0f, z);

        transform.position = orbitPos;

    }
}
