using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /* 
     
       LookAt() - 카메라가 따라가는 Agent의 방향을 바라보는 함수 
       Follow() - 카메라가 Agent를 따라가는 함수

    */

    public Transform objToFollow;
    [HideInInspector]
    public float lookSpeed = 10.0f;
    [HideInInspector]
    public float followSpeed = 10.0f;
    [HideInInspector]
    public Vector3 offSet;

    // 카메라의 시작 offset 위치 설정
    void Start()
    {
        offSet = new Vector3(0, 5f, -5f);
    }

    // 매 프레임마다 불러올 영역
    private void FixedUpdate()
    {
        LookAt();
        Follow();
    }

    void LookAt()
    {
        Vector3 _lookDir = objToFollow.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(_lookDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, lookSpeed * Time.deltaTime);
    }
    
    void Follow()
    {
        Vector3 _targetPos = objToFollow.position + (objToFollow.forward * offSet.z + objToFollow.right * offSet.x + objToFollow.up * offSet.y);
        transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
    }
}