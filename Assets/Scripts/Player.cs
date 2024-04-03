using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
   
   [SerializeField] private float moveSpeed = 7f;
   
   bool isWalking;
    private void Update() {
        Vector2 inputVector = new Vector2(0,0);
        if(Input.GetKey(KeyCode.W))
        {
            inputVector.y = +1;
        }
        if(Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if(Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if(Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1;
        }
        Vector3 movDir = new Vector3(inputVector.x,0,inputVector.y);

        movDir  = movDir.normalized;

        isWalking = movDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward,movDir,Time.deltaTime * rotateSpeed);
        
        transform.position += movDir * moveSpeed * Time.deltaTime;
    }


    public bool IsWalking()
    {
        return isWalking;
    }
}
