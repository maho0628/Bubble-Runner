using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(3f * Time.deltaTime , 0, 0);
        if (transform.position.x > 4)
        {
            Destroy(gameObject);
        }
    }

}
