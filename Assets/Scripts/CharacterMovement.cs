using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5.0f;
    private bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello, I am bubble");
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
        Jump();
    }

    void MoveCharacter()
    {
        float move = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(0, 0, move);
    }

    void Jump()
    {   
        if(Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                GetComponentInChildren<Rigidbody>().AddForce(new Vector2(0, 5), ForceMode.Impulse);
                isJumping = true;
            }
            StartCoroutine(ResetJump());
        }
    }

    IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(1);
        isJumping = false;
    }
}
