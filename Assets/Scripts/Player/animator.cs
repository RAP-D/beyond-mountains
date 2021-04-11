using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class animator : MonoBehaviour
{

    public Animator animt;

    void Start()
    {
        animt = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isrun = animt.GetBool("run");
        bool forward= Input.GetKey("w");
        bool walk = animt.GetBool("walk");
        bool run = Input.GetKey("left shift");
        bool jump = Input.GetKey("space");


        if (!walk&&forward)
        {
            animt.SetBool("walk", true);
        }
        if (walk&&!forward)
        {
            animt.SetBool("walk", false);
        }
        if(!isrun&&(forward&&run))
        {
            animt.SetBool("run", true);
        }
        if(isrun&&(!forward || !run))
        {
            animt.SetBool("run", false);
        }
        if(jump)
        {
            animt.SetBool("jump", true);
        }
     
    }
}
