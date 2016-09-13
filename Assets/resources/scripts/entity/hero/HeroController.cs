using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour {

    private Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");

        transform.position += new Vector3(horizontal, vertical) * Time.deltaTime * 3;

        if (vertical > 0)
        {
            SetDirection(EntityDirection.NORTH);
        }
        else if (vertical < 0)
        {
            SetDirection(EntityDirection.SOUTH);
        }
        else if (horizontal > 0)
        {
            SetDirection(EntityDirection.EAST);
        }
        else if (horizontal < 0)
        {
            SetDirection(EntityDirection.WEST);
        }
    }

    void SetDirection(EntityDirection direction)
    {
        animator.SetInteger("direction", (int)direction);
    }
}
