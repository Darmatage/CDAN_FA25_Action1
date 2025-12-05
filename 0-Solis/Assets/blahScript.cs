using UnityEngine;

public class blahScript : MonoBehaviour
{
	Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Blah"))
		{
			anim.SetTrigger("blah");
		}



    }
}
