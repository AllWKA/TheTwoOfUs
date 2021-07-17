using System;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rb;
    private GameObject _actionObject;
    
    public GameObject _emptyObject;
    public GameObject cam;
    public float movementSpeed = 10f;
    public float jumpForce = 100f;

    private void Awake()
    {
        this._animator = GetComponent<Animator>();
        this._rb = GetComponent<Rigidbody>();
        this._actionObject = _emptyObject;
    }

    void Update()
    {
        this.Rotate();
        this.Movement();
        this.ActivateObjectAction();
    }

    private void ActivateObjectAction()
    {
        if (this._actionObject != null && this._actionObject.transform.parent != this.transform)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                this._actionObject.transform.SetParent(this.transform);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                this._actionObject.transform.SetParent(null);
                this.transform.position = new Vector3(this._actionObject.transform.position.x,
                    this._actionObject.transform.position.y + 1, this._actionObject.transform.position.z);
            }
        }
        else if (this._actionObject != null && this._actionObject.transform.parent.CompareTag(this.tag))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                this._actionObject.transform.SetParent(null);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Cube"))
        {
            this._actionObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (this._actionObject != null && this._actionObject.tag.Equals(other.tag))
        {
            this._actionObject = this._emptyObject;
        }
    }

    private void Rotate(){
        this.cam.transform.Rotate(-Input.GetAxis("Mouse Y"),0,0);
        this.transform.Rotate(0,Input.GetAxis("Mouse X"),0);
    }

    private void Movement()
    {
        float deltaTime = Time.deltaTime;
        
        Vector3 fowardVector = GetFowardVector();
        Vector3 horizontalVector = GetHorizontalVector();

        Vector3 direction = (fowardVector+horizontalVector)*movementSpeed;
        this.transform.position += direction*deltaTime;

        PlayAnimation(fowardVector, horizontalVector);
    }

    private void PlayAnimation(Vector3 fowardVector, Vector3 horizontalVector)
    {
        Boolean isMovingFoward = Input.GetAxis("Vertical") > 0;
        Boolean isMovingBackwards = Input.GetAxis("Vertical") < 0;
        Boolean isNotMovingFowardOrBackwards = !isMovingFoward && !isMovingBackwards;
        
        Boolean isMovingLeft = Input.GetAxis("Horizontal") < 0;
        Boolean isMovingRight = Input.GetAxis("Horizontal") > 0;
        Boolean isNotMovingLeftOrRight = !isMovingLeft && !isMovingRight;

        if (isNotMovingLeftOrRight)
        {
            this._animator.SetBool("IsMovingLeft", false);
            this._animator.SetBool("IsMovingRight", false);
        } 
        
        if (isNotMovingFowardOrBackwards)
        {
            this._animator.SetBool("IsMovingFoward", false);
            this._animator.SetBool("IsMovingBackward", false);
        }

        if (isNotMovingFowardOrBackwards || isNotMovingLeftOrRight)
        {
            this._animator.SetBool("IsMovingFowardLeft", false);
            this._animator.SetBool("IsMovingFowardRight", false);
            this._animator.SetBool("IsMovingBackwardLeft", false);
            this._animator.SetBool("IsMovingBackwardRight", false); 
        }
        
        if (isMovingFoward && isNotMovingLeftOrRight)
        {
            this._animator.SetBool("IsMovingFoward", true);
        }
        else if (isMovingBackwards && isNotMovingLeftOrRight)
        {
            this._animator.SetBool("IsMovingBackward", true);
        }
        else if (isMovingFoward && isMovingRight)
        {
            this._animator.SetBool("IsMovingFowardRight", true);
            this._animator.SetBool("IsMovingFoward", false);
        }
        else if (isMovingFoward && isMovingLeft)
        {
            this._animator.SetBool("isMovingFowardLeft", true);
            this._animator.SetBool("IsMovingFoward", false);
        }
        else if (isMovingBackwards && isMovingRight)
        {
            this._animator.SetBool("IsMovingBackwardRight", true);
            this._animator.SetBool("IsMovingBackward", false);
        } else if (isMovingBackwards && isMovingLeft)
        {
            this._animator.SetBool("IsMovingBackwardLeft", true);
            this._animator.SetBool("IsMovingBackward", false);
        }
        
        if (isNotMovingFowardOrBackwards && isMovingLeft)
        {
            this._animator.SetBool("IsMovingLeft", true);
        } else if (isNotMovingFowardOrBackwards && isMovingRight)
        {
            this._animator.SetBool("IsMovingRight", true);
        }
    }

    private Vector3 GetHorizontalVector()
    {
        Vector3 horizontalVector = new Vector3(0,0,0);
        if (Input.GetAxis("Horizontal")>0)
        {
            horizontalVector = this.transform.right;
        } else if (Input.GetAxis("Horizontal") < 0)
        {
            horizontalVector = - this.transform.right;
        }

        return horizontalVector;
    }
    
    private Vector3 GetFowardVector()
    {
        Vector3 fowardVector = new Vector3(0,0,0);
        
        if (Input.GetAxis("Vertical")>0)
        {
            fowardVector = this.transform.forward;
        } else if (Input.GetAxis("Vertical") < 0)
        {
            fowardVector = - this.transform.forward;
        }

        return fowardVector;
    }
}
