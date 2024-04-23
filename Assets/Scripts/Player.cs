using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
   
    public static Player Instance{get; private set;}
   public event EventHandler<OnSelectionChangeEventArgs> OnSelectedCounterChange;
   public class   OnSelectionChangeEventArgs : EventArgs
   {
        public ClearCounter selectedCounter;
   }
   
   [SerializeField] private float moveSpeed = 7f;
   [SerializeField] private GameInput gameInput;
   [SerializeField] private LayerMask countersLayerMask;

   private Vector3 lastInteractDirection;
   private ClearCounter selectedCounter;
   
   bool isWalking;

   void Awake()
   {
        if(Instance!=null)
        {
            Debug.LogError("There are more the one player");
        }
        Instance = this;
   }
   private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

private void GameInput_OnInteractAction(object sender, System.EventArgs e) {


    if(selectedCounter != null)
    {
        selectedCounter.Interact();
    }
}


    private void Update() {
        HandleMovement();
        HandleInteractions();
     
    }

    
            public void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementNormalized();
        Vector3 moveDir = new Vector3(inputVector.x,0f,inputVector.y);
        
        if(moveDir != Vector3.zero)
        {
            //Debug.Log("moveDir: " + moveDir);
            lastInteractDirection = moveDir;
        }

        float ineractionDistance = 2f;
        //Debug.Log("lastInteraction: " + lastInteractDirection);
        if(Physics.Raycast(transform.position,lastInteractDirection,out RaycastHit hitInfo,ineractionDistance,countersLayerMask))
        {
            if(hitInfo.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if(clearCounter != selectedCounter)
                {

                    SetSelectedCounter(clearCounter);

                }
            }
            else
            {
                //Debug.Log("first Null");

                SetSelectedCounter(null);
            }
            
        }
        else
            {
                SetSelectedCounter(null);
            }
        
    }



    public void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementNormalized();
        Vector3 moveDir = new Vector3(inputVector.x,0,inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDir,moveDistance);
        

        if(!canMove)
        {
            //Cannot move in the moveDir
            
            //only move in x axis 
            Vector3 moveDirX = new Vector3(moveDir.x,0,0).normalized;
            canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirX,moveDistance);
            if(canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                //only move in z direction 
                Vector3 moveDirZ = new Vector3(0,0,moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirZ,moveDistance);

                if(canMove)
                {
                   moveDir = moveDirZ;
                }
            }
        }
        if(canMove)
        {
            transform.position += moveDir * moveDistance;
        }
        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward,moveDir,Time.deltaTime * rotateSpeed);
    }


    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChange?.Invoke(this,new OnSelectionChangeEventArgs
        {
            selectedCounter = selectedCounter
        });
    }


    public bool IsWalking()
    {
        return isWalking;
    }
}
