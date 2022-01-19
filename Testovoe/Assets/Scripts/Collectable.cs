using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    Animator anim;
    public bool inList = false;
    int chopCount;
    [SerializeField] GameObject log;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    public void TreeChoping()
    {
        FindObjectOfType<PlayerCollect>().anim.SetBool("chop", true);
        anim.SetBool("Choping", true);
    }
    public void AlertOnComplete(string message)
    {

        if (message.Equals("Chop"))
        {
            chopCount += 1;
            Instantiate(log, new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z), Quaternion.identity);
        }
        if (chopCount >= 3)
        {
            foreach (GameObject log in GameObject.FindGameObjectsWithTag("Log"))
                if (log.GetComponent<DroppedTree>())
                {
                log.GetComponent<DroppedTree>().StartMovingToplayer();
                }
            Destroy(this.gameObject);
            FindObjectOfType<PlayerCollect>().playerIsChoping = false;
            FindObjectOfType<PlayerCollect>().anim.SetBool("chop", false);
            FindObjectOfType<PlayerBackpack>()._woodCount += 3;
            FindObjectOfType<PlayerBackpack>().EnableVisualWood();
        }
    }
}
