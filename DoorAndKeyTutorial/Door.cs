using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public bool inRange;

    public Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inRange)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                anim.SetTrigger("Open");
            }
        }
    }
    
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            inRange = true;
        }
    }
    
    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            inRange = false;
        }
    }
}
