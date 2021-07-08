using System;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rb;
    private String _isMoving = "IsMoving";
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this._rb.AddForce(transform.up*jumpForce);
        }

        
        print("action object:" + this._actionObject.tag + " this tag:" + this.tag);
        if (Input.GetKeyDown(KeyCode.E) && this._actionObject != null && this._actionObject.transform.parent != this.transform)
        {
            print("pick object");
            this._actionObject.transform.SetParent(this.transform);
        } else if (Input.GetKeyDown(KeyCode.E) && this._actionObject != null && this._actionObject.transform.parent.CompareTag(this.tag))
        {
            print("leave object");
            this._actionObject.transform.SetParent(null);
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
            // this._actionObject = this._emptyObject;
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
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            this._animator.SetBool(this._isMoving, false);
        }
        else
        {
            this._animator.SetBool(this._isMoving, true);
        }
        
        if (Input.GetAxis("Vertical")>0 && Input.GetAxis("Horizontal") == 0)
        {
            this._animator.Play("Forwards");
            this._animator.SetBool(this._isMoving, true);
        }
        else if (Input.GetAxis("Vertical")<0 && Input.GetAxis("Horizontal") == 0)
        {
            this._animator.Play("Backwards");
            this._animator.SetBool(this._isMoving, true);
        }
        else if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0)
        {
            this._animator.Play("ForwardsRight");
            this._animator.SetBool(this._isMoving, true);
        }
        else if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0)
        {
            this._animator.Play("ForwardsLeft");
            this._animator.SetBool(this._isMoving, true);
        }
        else if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0)
        {
            this._animator.Play("BackwardsRight");
            this._animator.SetBool(this._isMoving, true);
        }
        else if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0)
        {
            this._animator.Play("BackwardsLeft");
            this._animator.SetBool(this._isMoving, true);
        }
        else if (Input.GetAxis("Vertical")==0 && Input.GetAxis("Horizontal") < 0)
        {
            this._animator.Play("Left");
            this._animator.SetBool(this._isMoving, true);
        }
        else if (Input.GetAxis("Vertical")==0 && Input.GetAxis("Horizontal") > 0)
        {
            this._animator.Play("Right");
            this._animator.SetBool(this._isMoving, true);
        }
        else if (fowardVector.magnitude == 0 && horizontalVector.magnitude == 0)
        {
            this._animator.SetBool(this._isMoving, false);
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
