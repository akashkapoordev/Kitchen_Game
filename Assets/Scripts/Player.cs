using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
   
   [SerializeField] private float moveSpeed = 7f;
   [SerializeField] private GameInput gameInput;
   
   bool isWalking;
    private void Update() {
        Vector2 inputVector = gameInput.GetMovementNormalized();
        Vector3 movDir = new Vector3(inputVector.x,0,inputVector.y);
        
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
