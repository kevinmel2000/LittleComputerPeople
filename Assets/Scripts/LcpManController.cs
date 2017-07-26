﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LcpManController : MonoBehaviour {

    public GameObject characterModel;
    public float speed;
    public int currentFloor = 1;
    public System.DateTime arrivalTime = System.DateTime.Now;

    public enum Activities
    {
        Kitchen,
        FrontDoor,
        ComputerRoom,
        Stove,
        TV,
        BathroomSink,
        KitchenTable,
        ComputerDesk,
        Shower,
        TVChair,
        Typewriter,
        Piano,
        Sofa,
        Sleep,
        LeaveHouse,
        BuildFire,
        Cupboard,
        Toilet,
        ReadBook
    }
    public enum CharacterStates
    {
        none,
        idle,
        walking,
        startactivity,
        doingactivity,
        finishedactivity
    }

    public Stack<Activities> activityQueue = new Stack<Activities>();
    public Activities currentActivity;

    public CharacterStates state;
    private Movement path;
    private string lastSound = "";

    private Vector3 tempVec1;
    private Vector3 tempVec2;
    private Vector3 panelVector3;

    // Use this for initialization
    void Start () {

        currentActivity = Activities.FrontDoor;
        state = CharacterStates.idle;

        panelVector3 = GameObject.Find("Panel").transform.position;
        GameObject.Find("Panel").transform.position = new Vector3(0, 0, 0);

        StartCoroutine(BlinkCoroutine());
	}
    

    private void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            activityQueue.Push(Activities.TV);
        }

        if (Input.GetKeyDown("s"))
        {
            activityQueue.Push(Activities.Sleep);
        }

        if (Input.GetKeyDown("t"))
        {
            activityQueue.Push(Activities.Toilet);
        }

        if (Input.GetKeyDown("b"))
        {
            activityQueue.Push(Activities.ReadBook);
        }
        if (Input.GetKeyDown("w"))
        {
            activityQueue.Push(Activities.Typewriter);
        }


        if (Input.GetKeyDown("a"))
        {
            GameObject.Find("Alarm_clock").GetComponent<AudioSource>().Play();
            if(currentActivity == Activities.Sleep)
            {
                activityQueue.Push(Activities.Toilet);
                FinishedActivity(currentActivity);
            }
        }

        if (state == CharacterStates.idle)
        {
            currentActivity = GetNextActivity();
            GoToActivity(currentActivity);
        }

        if (state == CharacterStates.startactivity)
        {
            StartActivity(currentActivity);
        }

        if (state == CharacterStates.doingactivity)
        {
            DoingActivity(currentActivity);
        }

        if (state == CharacterStates.finishedactivity)
        {
            FinishedActivity(currentActivity);
        }

    }

    void GoToActivity(Activities activity)
    {
        if (activity == Activities.TV)
            StartWalk(3, new Vector3(6.67f, 8.7f, 2.41f), Movement.FacingDirection.right, 1);

        if (activity == Activities.BathroomSink)
            StartWalk(2, new Vector3(0.65f, 5.55f, 2.41f), Movement.FacingDirection.backward, 4);

        if (activity == Activities.Stove)
            StartWalk(1, new Vector3(8.05f, 2.12f, 2.41f), Movement.FacingDirection.right, 4);

        if (activity == Activities.ComputerRoom)
            StartWalk(2, new Vector3(-4.78f, 5.41f, 2.62f), Movement.FacingDirection.forward, 3);

        if (activity == Activities.KitchenTable)
            StartWalk(1, new Vector3(3.66f, 2.27f, 1.75f), Movement.FacingDirection.forward, 5);

        if (activity == Activities.ComputerDesk)
            StartWalk(2, new Vector3(-6.04f, 5.32f, 1.64f), Movement.FacingDirection.backward, 20);

        if (activity == Activities.Shower)
            StartWalk(2, new Vector3(-2.39f, 5.32f, 0.86f), Movement.FacingDirection.left, 8);

        if (activity == Activities.TVChair)
            StartWalk(3, new Vector3(4.8f, 8.35f, 1.75f), Movement.FacingDirection.right, 20);

        if (activity == Activities.FrontDoor)
            StartWalk(1, new Vector3(-7.24f, 2.12f, 2.62f), Movement.FacingDirection.forward, 2);

        if (activity == Activities.Typewriter)
            StartWalk(3, new Vector3(-2.327f, 8.77f, 1.137f), Movement.FacingDirection.forward, 10);

        if (activity == Activities.Kitchen)
            StartWalk(1, new Vector3(5.56f, 2.17f, 2.62f), Movement.FacingDirection.forward, 5);

        if (activity == Activities.Piano)
            StartWalk(3, new Vector3(1.99f, 8.54f, 1.767f), Movement.FacingDirection.backward, 10);

        if (activity == Activities.Sofa)
            StartWalk(1, new Vector3(-2.86f, 1.98f, 0.84f), Movement.FacingDirection.forward, 5);

        if (activity == Activities.Sleep)
            StartWalk(2, new Vector3(7.22f, 5.43f, 2.62f), Movement.FacingDirection.up, 3600);

        if (activity == Activities.LeaveHouse)
            StartWalk(1, new Vector3(-7.83f, 2.16f, 1.15f), Movement.FacingDirection.backward, 20);

        if (activity == Activities.BuildFire)
            StartWalk(1, new Vector3(-5.9f, 2.16f, 1.54f), Movement.FacingDirection.backward, 2);

        if (activity == Activities.Cupboard)
            StartWalk(1, new Vector3(6.24f, 2.16f, 1.79f), Movement.FacingDirection.backward, 2);

        if (activity == Activities.Toilet)
            StartWalk(2, new Vector3(-1.11f, 5.53f, 1.43f), Movement.FacingDirection.backward, 10);

        if (activity == Activities.ReadBook)
            StartWalk(2, new Vector3(-4.249f, 5.392f, 1.43f), Movement.FacingDirection.backward, 1);
            

    }

    void StartActivity(Activities activity)
    {
        if (currentActivity == Activities.Shower)
        {
            GameObject.Find("Shower").GetComponent<ParticleSystem>().Play();
            PlaySound("shower", true);
        }

        if (currentActivity == Activities.ComputerRoom || currentActivity == Activities.Kitchen || currentActivity == Activities.FrontDoor)
        {
            PlaySound("speak1", true);
        }

        if (currentActivity == Activities.BathroomSink)
        {
            PlaySound("sink", true);
        }

        if (currentActivity == Activities.ComputerDesk)
        {
            PlaySound("computerkeyboard", true);
        }

        if (currentActivity == Activities.Typewriter)
        {
            GameObject.Find("Panel").transform.position = panelVector3;
            PlaySound("typewriter", true);
        }

        if (currentActivity == Activities.Piano)
        {
            PlaySound("piano1", true);
        }

        if (currentActivity == Activities.Sofa)
        {
            characterModel.GetComponent<Animator>().SetBool("isSitting", true);
        }

        if (currentActivity == Activities.BuildFire || currentActivity == Activities.TV)
        {
            characterModel.GetComponent<Animator>().SetBool("isBending", true);
        }

        if (currentActivity == Activities.Cupboard || currentActivity == Activities.ComputerDesk 
            || currentActivity == Activities.Typewriter || currentActivity == Activities.BathroomSink || currentActivity == Activities.Piano)
        {
            characterModel.GetComponent<Animator>().SetBool("isDoing", true);
        }

        if (currentActivity == Activities.KitchenTable)
        {
            characterModel.GetComponent<Animator>().SetBool("isEating", true);
        }

        if (currentActivity == Activities.Sleep)
        {
            transform.position = new Vector3(7.948f, 5.97f, 2.62f);
            transform.forward = new Vector3(0f, 1f, 0f);
            PlaySound("snore", true);
        }

        if (currentActivity == Activities.LeaveHouse)
        {
            GameObject.Find("FrontDoor").transform.Rotate(new Vector3(0, 75.42f, 0));
            GameObject.Find("FrontDoor").transform.position = new Vector3(-6.96f, 3.13f, 1.13f);
            GameObject.Find("FrontDoor").tag = "open";

            transform.position = new Vector3(-7.24f, 2.12f, -1.4f);
        }

        if (currentActivity == Activities.Toilet)
        {
            GameObject.Find("ToiletDoor").transform.Rotate(new Vector3(0, 63.73f, 0));
            GameObject.Find("ToiletDoor").transform.position = new Vector3(-0.53f, 6.38f, 0.91f);
            GameObject.Find("ToiletDoor").tag = "open";

            transform.position = new Vector3(-7.24f, 2.12f, -1.4f);
        }

        if(currentActivity == Activities.ReadBook)
        {
            GameObject.Find("Book").GetComponent<MeshRenderer>().enabled = true;
            activityQueue.Push(Activities.Sofa);
        }

        state = CharacterStates.doingactivity;

    }

    void DoingActivity(Activities activity)
    {
        System.TimeSpan timeDifference = System.DateTime.Now - arrivalTime;

        if (timeDifference.Seconds > path.stayMaxTime)
        {
            state = CharacterStates.finishedactivity;
        }

        if (currentActivity == Activities.LeaveHouse && timeDifference.Seconds > 0.5f)
        {
            if (GameObject.Find("FrontDoor").tag == "open")
            {
                GameObject.Find("FrontDoor").transform.position = new Vector3(-7.37f, 3.13f, 0.43f);
                GameObject.Find("FrontDoor").transform.rotation = Quaternion.identity;

                PlaySound("door-close", false);
                GameObject.Find("FrontDoor").tag = "closed";
            }

        }

        if (currentActivity == Activities.Toilet)
        {
            if (timeDifference.Seconds > 0.5f)
            {
                if (GameObject.Find("ToiletDoor").tag == "open")
                {
                    GameObject.Find("ToiletDoor").transform.position = new Vector3(-0.77f, 6.38f, 0.39f);
                    GameObject.Find("ToiletDoor").transform.rotation = Quaternion.identity;

                    PlaySound("door-close", false);
                    GameObject.Find("ToiletDoor").tag = "closed";
                }
            }

            if (timeDifference.Seconds > 2.0f && lastSound != "toilet")
                PlaySound("toilet", false);
        }

    }

    void FinishedActivity(Activities activity)
    {
        state = CharacterStates.idle;

        GameObject.Find("Shower").GetComponent<ParticleSystem>().Stop();
        GetComponent<AudioSource>().Stop();

        characterModel.GetComponent<Animator>().SetBool("isSitting", false);
        characterModel.GetComponent<Animator>().SetBool("isBending", false);
        characterModel.GetComponent<Animator>().SetBool("isDoing", false);
        characterModel.GetComponent<Animator>().SetBool("isEating", false);

        if (activity == Activities.Sleep)
        {
            transform.position = new Vector3(7.22f, 5.43f, 2.62f);
            transform.forward = new Vector3(0f, 0f, 1f);
        }

        if (activity == Activities.TV)
        {
            if(GameObject.Find("TVScreen").GetComponent<TVController>().state == TVController.States.off)
            {
                GameObject.Find("TVScreen").GetComponent<TVController>().state = TVController.States.on;
                PlaySound("tv1", false);
                activityQueue.Push(Activities.TVChair);
            }
            else
            {
                GameObject.Find("TVScreen").GetComponent<TVController>().state = TVController.States.off;
            } 
        }

        if (activity == Activities.Cupboard)
        {
            activityQueue.Push(Activities.Stove);
        }

        if (activity == Activities.Stove)
        {
            activityQueue.Push(Activities.KitchenTable);
        }

        if (activity == Activities.LeaveHouse)
        {
            GameObject.Find("FrontDoor").transform.Rotate(new Vector3(0, 0, 0));
            GameObject.Find("FrontDoor").transform.position = new Vector3(-7.37f, 3.13f, 0.43f);

            transform.position = new Vector3(-7.83f, 2.16f, 1.15f);
        }

        if (activity == Activities.Toilet)
        {
            transform.position = new Vector3(-1.11f, 5.53f, 1.43f);
            activityQueue.Push(Activities.BathroomSink);
        }

        if (activity == Activities.BuildFire)
        {
            GameObject.Find("Fire").GetComponent<ParticleSystem>().Play();
        }

        if (currentActivity == Activities.Sofa)
        {
            GameObject.Find("Book").GetComponent<MeshRenderer>().enabled = false;
        }

        if (currentActivity == Activities.Typewriter)
        {
            GameObject.Find("Panel").transform.position = new Vector3(0, 0, 0);
        }
    }

    Activities GetNextActivity()
    {
        if (activityQueue.Count > 0)
            return activityQueue.Pop();

        TimeSpan start = new TimeSpan(23, 00, 0);
        TimeSpan end = new TimeSpan(6, 00, 0); 
        TimeSpan now = DateTime.Now.TimeOfDay;

        if ((now > start) && (now < end))
        {
            return Activities.Sleep;
        }

        int nextActivity = UnityEngine.Random.Range(1, 20);

        switch (nextActivity)
        {
            case 1: return Activities.BathroomSink;
            case 2: return Activities.ComputerDesk;
            case 3: return Activities.ComputerRoom;
            case 4: return Activities.FrontDoor;
            case 5: return Activities.Kitchen;
            case 6: return Activities.KitchenTable;
            case 7: return Activities.Shower;
            case 8: return Activities.Stove;
            case 9: return Activities.TV;
            case 10: return Activities.TVChair;
            case 11: return Activities.Typewriter;
            case 12: return Activities.Piano;
            case 13: return Activities.Sofa;
            case 14: return Activities.Sleep;
            case 15: return Activities.LeaveHouse;
            case 16: return Activities.BuildFire;
            case 17: return Activities.Cupboard;
            case 18: return Activities.Toilet;
            case 19: return Activities.ReadBook;
        }

        return Activities.FrontDoor;
    }

    void StartWalk(int floor, Vector3 location, Movement.FacingDirection facing, int timeToStay)
    {
        List<Vector3> waypoints = MoveBetweenFloors(currentFloor, floor);
        waypoints.Add(location);
        path = new Movement(waypoints, facing, floor, timeToStay);
        state = CharacterStates.walking;
        StartCoroutine(MoveCharCoroutine(path));
    }

    List<Vector3> MoveBetweenFloors(int startFloor, int endFloor)
    {
        List<Vector3> waypoints = new List<Vector3>();

        if (startFloor == 1 && endFloor == 2)
        {
            waypoints.Add(new Vector3(0.35f, 2.12f, 2.62f));
            waypoints.Add(new Vector3(0.35f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(1.55f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(2.62f, 5.3f, 2.4f));
        }

        if (startFloor == 1 && endFloor == 3)
        {
            waypoints.Add(new Vector3(0.35f, 2.12f, 2.62f)); // 1 floor stairs
            waypoints.Add(new Vector3(0.35f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(1.55f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(2.62f, 5.3f, 2.4f)); // 2ns floor stairs
            waypoints.Add(new Vector3(2.62f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(1.89f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(0.21f, 8.58f, 0.79f)); // 3rd floor stairs
            waypoints.Add(new Vector3(0.21f, 8.58f, 2.37f));
        }

        if (startFloor == 2 && endFloor == 3)
        {
            waypoints.Add(new Vector3(2.62f, 5.3f, 2.4f));
            waypoints.Add(new Vector3(2.62f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(1.89f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(0.21f, 8.58f, 0.79f));
            waypoints.Add(new Vector3(0.21f, 8.58f, 2.37f));
        }

        if (startFloor == 3 && endFloor == 2)
        {
            waypoints.Add(new Vector3(0.21f, 8.58f, 2.37f));
            waypoints.Add(new Vector3(0.21f, 8.58f, 0.79f));
            waypoints.Add(new Vector3(1.89f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(2.62f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(2.62f, 5.3f, 2.4f));
        }

        if (startFloor == 3 && endFloor == 1)
        {
            waypoints.Add(new Vector3(0.21f, 8.58f, 2.37f));
            waypoints.Add(new Vector3(0.21f, 8.58f, 0.79f)); // 3rd floor stairs
            waypoints.Add(new Vector3(1.89f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(2.62f, 6.55f, 0.79f));
            waypoints.Add(new Vector3(2.62f, 5.3f, 2.4f)); // 2ns floor stairs
            waypoints.Add(new Vector3(1.55f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(0.35f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(0.35f, 2.12f, 2.62f)); // 1 floor stairs
        }

        if (startFloor == 2 && endFloor == 1)
        {
            waypoints.Add(new Vector3(2.62f, 5.3f, 2.4f));
            waypoints.Add(new Vector3(1.55f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(0.35f, 3.35f, 1.11f));
            waypoints.Add(new Vector3(0.35f, 2.12f, 2.62f));
        }

        return waypoints;
    }
    
    IEnumerator MoveCharCoroutine(Movement m)
    {
        characterModel.GetComponent<Animator>().SetBool("isWalking", true);

        foreach (Vector3 position in m.waypoints)
        {
            while (Vector3.Distance(transform.position, position) > .0001)
            {
                var lookPos = position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

                transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
                yield return null;
            }
        }

        characterModel.GetComponent<Animator>().SetBool("isWalking", false);

        if (m.finalFacing == Movement.FacingDirection.forward)
                transform.forward = new Vector3(0f, 0f, 1f);
        else if (m.finalFacing == Movement.FacingDirection.backward)
            transform.forward = new Vector3(0f, 0f, -1f);
        else if (m.finalFacing == Movement.FacingDirection.right)
            transform.forward = new Vector3(1f, 0f, 0f);
        else if (m.finalFacing == Movement.FacingDirection.left)
            transform.forward = new Vector3(-1f, 0f, 0f);
        else if (m.finalFacing == Movement.FacingDirection.up)
            transform.forward = new Vector3(0f, 1f, 0f);

        currentFloor = m.floor;
        state = CharacterStates.startactivity;
        arrivalTime = System.DateTime.Now;
    }

    void PlaySound(string sound, bool loop)
    {
        if (sound != lastSound)
        {
            GetComponent<AudioSource>().Stop();

            AudioClip clip = Resources.Load<AudioClip>(sound);
            GetComponent<AudioSource>().loop = loop;
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();

            lastSound = sound;
        }

    }

    IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            int r = UnityEngine.Random.Range(1, 3);

            yield return new WaitForSeconds(r);

            tempVec1 = GameObject.Find("REyeball").transform.localScale;
            tempVec2 = GameObject.Find("LEyeball").transform.localScale;

            //Turn My game object that is set to false(off) to True(on).
            GameObject.Find("REyeball").transform.localScale = new Vector3(0, 0, -1);
            GameObject.Find("LEyeball").transform.localScale = new Vector3(0, 0, -1);

            //Turn the Game Oject back off after 1 sec.
            yield return new WaitForSeconds(0.1f);

            //Game object will turn off
            GameObject.Find("REyeball").transform.localScale = tempVec1;
            GameObject.Find("LEyeball").transform.localScale = tempVec2;
        }

    }

}