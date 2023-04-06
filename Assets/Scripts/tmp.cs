using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmp : MonoBehaviour
{
    // Start is called before the first frame update
    
    private Animation anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        anim.Play("A");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward*3.5f*Time.deltaTime);
    }
}
