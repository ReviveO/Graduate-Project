using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CarController))]
[RequireComponent(typeof(Rigidbody))]

/*

 Initialize() - 매 학습마다 시작 전에 불러오는 함수
 OnEpisodeBegin() - 매 Episode마다 실행되는 함수
 CollectObservation() - Agent가 환경에서의 값들을 수집하는 함수
 OnActionReceived() - Agent가 수집된 정보를 계산하여, 최대 보상을 얻기 위해 도출해 내는 결과값 함수
 Heuristic() - Agent에 사용자의 추정치를 입력하는 함수
 OnCollisionEnter() - 충돌판정을 식별하고 보상을 주는 함수
 OnTriggerEnter() - 최초로 주차공간에 충돌했을 때 보상 주는 함수
 OnTriggerStay() - 주차공간 안에 들어가 목표하는 허용각에 따라 보상 주는 함수

*/

public class CarAgent : Agent
{
    public bool onetime = true;
    private float[] _lastActions;
    private Rigidbody car_rigidbody;
    private CarController car_controller;

    [HideInInspector] public GameObject obs_car;
    [HideInInspector] public GameObject goal;
    [HideInInspector] public Vector3 Center_of_Agent; 

    // 자동차 WheelCol관련 변수
    public Transform frontDriver, frontPass;
    public Transform backDriver, backPass;
    public float distance;
    float a_angle = 20f;     // (허용각)
    int crashcount = 1;     // (장애물 충돌 횟수)
    private init_env init_env;

    public override void Initialize()
    {
        car_rigidbody = GetComponent<Rigidbody>();
        car_controller = GetComponent<CarController>();
        init_env = transform.parent.GetComponent<init_env>();
    }

    public override void OnEpisodeBegin()
    {
        init_env.clear_Parkinglot();
        this.car_rigidbody.velocity = Vector3.zero;
        this.car_rigidbody.angularVelocity = Vector3.zero;
        this.car_controller.CurrentSteeringAngle = 0f;
        this.car_controller.CurrentAcceleration = 0f;
        this.car_controller.CurrentBrakeTorque = 0f;
        this.transform.rotation = Quaternion.Euler(0, Random.Range(90,270), 0);
        this.transform.position = transform.parent.position + new Vector3(Random.Range(-20f, 20f), 0f, 9f);
        crashcount = 1;
        init_env.Init_Parkinglot();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent가 Sensor로 관측값을 받아오는 부분, 총 6개의 센서 3 dimension vector로 구성되어 있으며,
        // Space Size = 3 * 6 = 18

        Vector3 dirToTarget = (goal.transform.position - transform.position).normalized;
        sensor.AddObservation(transform.position.normalized);
        sensor.AddObservation(this.transform.InverseTransformPoint(goal.transform.position));
        sensor.AddObservation(this.transform.InverseTransformVector(car_rigidbody.velocity.normalized));
        sensor.AddObservation(this.transform.InverseTransformDirection(dirToTarget));
        sensor.AddObservation(transform.forward);
        sensor.AddObservation(transform.right);
        float velocityAlignment = Vector3.Dot(dirToTarget, car_rigidbody.velocity);
        AddReward(0.001f * velocityAlignment);
        distance = Vector3.Distance(goal.transform.position, car_rigidbody.position);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // 이전 행동값 저장, Accel, Steer, Brake 3개의 값을 인공신경망 결과값으로 출력
        _lastActions = vectorAction;
        car_controller.CurrentSteeringAngle = vectorAction[0];
        car_controller.CurrentAcceleration = vectorAction[1];
        car_controller.CurrentBrakeTorque = vectorAction[2];

        //매초마다 음수의 보상값을 주어서 목표에 빨리 도달하기 위한 보상값 
        AddReward(-0.001f);

    }

    public override void Heuristic(float[] actionsOut)
    {
        // 추정치로 입력을 받아서 Agent 행동 결정
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-0.007f * crashcount);
            crashcount++;
            Debug.Log("obstacle");
        }
        if (collision.gameObject.CompareTag("OUT"))
        {
            Debug.Log("OUT");
            AddReward(-1f);
            EndEpisode();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-0.001f);
            Debug.Log("Out of Contact");
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        // 목표하는 지점에 도달하기 위해서 goal_line에 도착하면 보상을 주는 코드
        if (other.transform.CompareTag("goal_line"))
        {
            SetReward(0.001f * ((2000 + init_env.geta_angle()) / (20 + init_env.geta_angle())));
            if(init_env.geta_angle()<=2*a_angle&& onetime == true )
            {
                Debug.Log("Nice");
                SetReward(0.2f);
                onetime = false;
             }
        }

        // 목표하는 지점에 도달한 후, 차와 주차공간의 각도를 계산해서 보상을 주는 코드
        if (other.transform.CompareTag("goal"))
        {

            Vector3 swap = new Vector3(0, 0, 0); // 임시로 쓰는 변수           gameobject.position은 x,y,z값을 직접적으로 수정할수 없어서 사용
            Center_of_Agent = frontDriver.position + backPass.position;
            Center_of_Agent = Center_of_Agent * 0.5f;           // 차의 중심좌표
            Center_of_Agent.y = 0;
            swap = goal.transform.GetChild(4).transform.position;
            swap.y = 0f;
            SetReward(0.01f * (1000 + init_env.geta_angle()) / (10 + init_env.geta_angle()));
        }

        if (other.transform.CompareTag("OUT"))
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        // 목표 지점에 들어간 후 정해진 각도에 들어올때까지 판정을 한 뒤에
        // 보상을 주고 에피소드를 끝내는 코드
        if (other.transform.CompareTag("goal"))
        {
            Debug.Log("판정중");
            Vector3 swap = new Vector3(0, 0, 0); // 임시로 쓰는 변수           gameobject.position은 x,y,z값을 직접적으로 수정할수 없어서 사용
            Center_of_Agent = frontDriver.position + backPass.position;
            Center_of_Agent = Center_of_Agent * 0.5f;           // 차의 중심좌표
            Center_of_Agent.y = 0;
            swap = goal.transform.GetChild(4).transform.position;
            swap.y = 0f;

            if (Vector3.Distance(Center_of_Agent, swap) <= 0.5f)
            {
                SetReward(0.03f * (1000 + init_env.geta_angle()) / (10 + init_env.geta_angle()));
                EndEpisode();
            }
        }

    }

}