using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    private void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = Remap(transform.parent.position.x, -7.5f, 7.5f, -5f, 5f);
        transform.position = newPosition;
    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
