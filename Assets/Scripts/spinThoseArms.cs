using UnityEngine;

public class spinThoseArms : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, -600 * Time.deltaTime);
    }
}
