using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerCollect : MonoBehaviour
{
    [SerializeField] List<Transform> TreeList = new List<Transform>();
    [SerializeField] float _fillSpeed;
    [SerializeField] GameObject homePoint;
    [SerializeField] public Animator anim;
    public Vector3 homePossition;
    //public Vector3 target;
    public bool playerIsChoping;
    public bool playerIsMoving;
    public bool playerInHome;
    PlayerBackpack bp;
    public int _closestTreeIndex;
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        foreach (GameObject tree in GameObject.FindGameObjectsWithTag("Tree"))
            if (tree.GetComponent<Collectable>().inList == false)
            {
                TreeList.Add(tree.transform);                             //add tree in list
                tree.GetComponent<Collectable>().inList = true; //protect adding already added tree in list
            }
    }
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        bp = GetComponentInChildren<PlayerBackpack>();
        StartCoroutine(FindByTag());
        Debug.Log("First start");
        StartMoving();
        homePossition = homePoint.transform.position;
    }
    public void StartMoving()
    {
        if (TreeList.Count == 0)
        {
            AfterCorutineCall();
            foreach (GameObject home in GameObject.FindGameObjectsWithTag("Finish"))
            {
                if (home.GetComponent<HomePoint>().inList == false)
                {
                    TreeList.Add(home.transform);                             //add tree in list
                    home.GetComponent<HomePoint>().inList = true;   //protect adding already added tree in list
                    int positionIndex = TreeList.IndexOf(home.transform);
                    StartCoroutine(moveObject(AfterCorutineCall, TreeList[positionIndex].transform.position));
                    playerInHome = true;
                }
            }
        }
        else
        {
            if (bp._woodCount == 0 && TreeList.Count != 0 && !playerIsChoping && !playerIsMoving)
            {
                Debug.Log("Start again");
                Transform closestEnemy = GetClosestEnemy(TreeList, this.transform);
                StartCoroutine(moveObject(AfterCorutineCall, closestEnemy.transform.position));
            }
            else
            {
                FindHome();
                homePoint.GetComponent<HomePoint>().inList = false;
            }
        }
       
    }

    public IEnumerator moveObject(Action action, Vector3 target)
    {
        float totalMovementTime = 100f;      //the amount of time you want the movement to take
        float currentMovementTime = 0f;       //The amount of time that has passed
        while (Vector3.Distance(transform.localPosition, target) > 0.3)
        {
            currentMovementTime += Time.fixedDeltaTime;
            //transform.LookAt(target);
            transform.localPosition = Vector3.Lerp(transform.position, target, currentMovementTime / totalMovementTime);
            yield return null;
            transform.LookAt(target);       //look at tree 
            playerIsMoving = true;
            anim.SetBool("walk", true);
            Debug.Log("Moving to target");
        }
        if (!playerInHome) 
        {
            playerIsChoping = true;
            anim.SetBool("chop", true);
            anim.SetBool("walk", false);
            TreeList[_closestTreeIndex].GetComponent<Collectable>().TreeChoping();
            TreeList.RemoveAt(_closestTreeIndex);
        }
        else if (playerInHome)
        {
            FindObjectOfType<PlayerBackpack>().CleanInventory();
            bp._woodCount = 0;
            Debug.Log("Wood delivered");
            foreach (GameObject home in GameObject.FindGameObjectsWithTag("Finish"))
            {
                int positionIndex = TreeList.IndexOf(home.transform);
                TreeList.RemoveAt(positionIndex);
            }
            playerInHome = false;
        }
        anim.SetBool("walk", false);
        playerIsMoving = false;
        action();
    }
    public void AfterCorutineCall()
    {
        //Debug.Log("After Corutine");
        Invoke("StartMoving", 3.2f);
    }
    public void FindHome()
    {
        foreach (GameObject home in GameObject.FindGameObjectsWithTag("Finish"))
        {
            if (home.GetComponent<HomePoint>().inList == false)
            {
                TreeList.Add(home.transform);                             //add tree in list
                home.GetComponent<HomePoint>().inList = true;   //protect adding already added tree in list
                int positionIndex = TreeList.IndexOf(home.transform);
                StartCoroutine(moveObject(AfterCorutineCall, TreeList[positionIndex].transform.position));
                playerInHome = true;
            }
        }
        //StartCoroutine(moveObject());
    }

    Transform GetClosestEnemy(List<Transform> enemies, Transform fromThis)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromThis.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
                int closestIndex = TreeList.IndexOf(bestTarget);
                _closestTreeIndex = closestIndex;
                Debug.Log(closestIndex + "--Closest index");
            }
        }
        return bestTarget;
    }
    IEnumerator FindByTag()             //find tree in scene
    {
        while (true)
        {
            yield return new WaitForSeconds(_fillSpeed);
            //Debug.Log("Searching trees");
            foreach (GameObject tree in GameObject.FindGameObjectsWithTag("Tree"))
                if (tree.GetComponent<Collectable>().inList == false)
                {
                    TreeList.Add(tree.transform);                             //add tree in list
                    tree.GetComponent<Collectable>().inList = true; //protect adding already added tree in list
                }
            
        }
    }
}
