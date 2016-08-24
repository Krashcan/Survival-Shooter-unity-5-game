using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed=6.0f;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidBody;
    int floorMask;
    float camRayLength=100f;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor"); //used the tag floor of the quad
        anim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");   //sets movement by default to 0,-1 or 1 instead of -1 to 1. talking about raw.
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);

        Turning();

        Animating(h,v);
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime; //why normalized? movement will always be 1 cuz of this,not 1.4 in diagonal
        playerRigidBody.MovePosition(transform.position + movement);//time.deltatime above to kind of sort the speed .
    }

    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);//the raycast dude is hitting the floor,at the mousepostion,something like that
        RaycastHit floorHit;//unknown type for me,but understandable

        if(Physics.Raycast(camRay,out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);//very nice.
            playerRigidBody.MoveRotation(newRotation);

        }
    }

    void Animating(float h, float v)
    {
        bool walking = (h != 0f) || (v != 0f);
        anim.SetBool("IsWalking", walking);//yeah the animation
    }
}
