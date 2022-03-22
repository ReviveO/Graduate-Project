using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float maxBrakeTorque;

    // 현재 Accel 값 설정
    public float CurrentAcceleration
    {
        get => m_currentAcceleration;
        set => m_currentAcceleration = Mathf.Clamp(value, -1f, 1f);
    }

    // 현재 Brake 값 설정
    public float CurrentBrakeTorque
    {
        get => m_currentBrakeTorque;
        set
        {
            m_currentBrakeTorque = value <= 0.8f ? 0f : value;
        }
    }

    // 현재 Steer 값 설정
    public float CurrentSteeringAngle
    {
        get => m_currentSteeringAngle;
        set => m_currentSteeringAngle = Mathf.Clamp(value, -1f, 1f);
    }

    private float m_currentSteeringAngle = 0f;
    private float m_currentAcceleration = 0f;
    private float m_currentBrakeTorque = 0f;

    // Input값을 Steer, Accel, Brake 순으로 가져옴
    public double[] CurrentInputs
    {
        get { return new double[] { m_currentSteeringAngle, m_currentAcceleration, m_currentBrakeTorque }; }
    }

    // 변경되는 Steer, Accel, Brake 값 객체에 반영
    public void FixedUpdate()
    {
        float motor = maxMotorTorque * m_currentAcceleration;
        float steering = maxSteeringAngle * m_currentSteeringAngle;
        float brake = maxBrakeTorque * m_currentBrakeTorque;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            if (axleInfo.brake)
            {
                axleInfo.leftWheel.brakeTorque = brake;
                axleInfo.rightWheel.brakeTorque = brake;
            }
        }
    }
}

// 차량 전단부, 후단부로 나눠서 Wheel, motor 등 조절
[System.Serializable]
public class AxleInfo  
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
    public bool brake;
}