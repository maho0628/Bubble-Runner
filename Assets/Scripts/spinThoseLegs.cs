using System.Collections;
using UnityEngine;

public class spinThoseLegs : MonoBehaviour
{
    private float speed = 300.0f;
    private float minAngle = 250;
    private float maxAngle = 300;

    public bool theotherleg = false;
    public bool canstart = false;
    private float phaseOffset = 0.5f; // Offset for the second leg

    private void Start()
    {
        StartCoroutine(DelayStart(3f));
    }

    IEnumerator DelayStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        canStart();
    }

    void canStart()
    {
        Debug.Log("right");
        canstart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canstart)
        {
            return;
        }

        float time = Time.time * speed;
        if (theotherleg)
        {
            time += phaseOffset * (maxAngle - minAngle);
        }

        float angle = Mathf.PingPong(time, maxAngle - minAngle) + minAngle;
        transform.localRotation = Quaternion.Euler(angle, -90, 90);
    }
}


