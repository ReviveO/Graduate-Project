using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
*
* Init_Parkinglot()     -     에피소드가 시작할때 호출되어, 주차된 차/ 목표지점을 초기화해주는 함수
*     set_target()      -     목표지점을 기능적으로 "목표" 기능을 하게 도와주는 함수
*     geta_angle()      -     각을 0~90범위로 변환
*     GetAngle()        -     각도를 구하는 함수. (원점에서부터 각 점까지의 직선, 그 사이각)
*     cal_angle()       -     차의 주차 각도를 재는 함수
* clear_Parkinglot()    -     에피소드가 시작할 때 제일먼저 호출되어 주차된 차/목표지점이 있다면 제거하는 함수
*
*
*/

public class init_env : MonoBehaviour
{

    private GameObject obs_car;
    private List<GameObject> slot = new List<GameObject>();
    GameObject child = null;
    private CarAgent car_agent;

    [SerializeField]
    public float Pos;
    [SerializeField]
    bool flag;

    // Start is called before the first frame update
    void Start()
    {
        car_agent = this.transform.GetChild(2).GetComponent<CarAgent>();
    }

    public void Init_Parkinglot()
    {
        // 임시변수
        float z_val = 0;
        float x_val = 0;                                  // 차가 생성되는 시작점 좌표가 들어가는 변수. 
        List<float> stored_x = new List<float>();         // 생성되는 차의 좌표를 저장할 변수
        List<float> stored_z = new List<float>();
        int num = 0;               // random값을 저장하는 변수


        #region Parking First Line Code
        for (int i = 0; i < 2; i++)
        {
            num = Random.Range(1, 40);
            if (num < 25)
            {
                obs_car = Instantiate(Resources.Load("Car_18C"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
                obs_car.transform.parent = this.transform.GetChild(1).GetChild(0).GetChild(1);
                obs_car.transform.localPosition = new Vector3(32.3f + x_val, -0.8f, 2.7f);
                obs_car.tag = "Obstacle";
                slot.Add(obs_car);
                stored_x.Add(obs_car.transform.localPosition.x);
                stored_z.Add(obs_car.transform.localPosition.z);
            }
            x_val = x_val - (float)2.01;
        }
        x_val = 0;

        x_val = 0;
        for (int i = 0; i < 18; i++)
        {
            num = Random.Range(1, 40);
            if (num < 25)
            {
                obs_car = Instantiate(Resources.Load("Car_18C"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
                obs_car.transform.parent = this.transform.GetChild(1).GetChild(0).GetChild(1);
                obs_car.transform.localPosition = new Vector3(16.1f + x_val, -0.8f, 2.7f);
                obs_car.tag = "Obstacle";
                slot.Add(obs_car);
                stored_x.Add(obs_car.transform.localPosition.x);
                stored_z.Add(obs_car.transform.localPosition.z);
            }
            x_val = x_val - (float)2.01;
        }


        x_val = 0;
        for (int i = 0; i < 2; i++)
        {
            num = Random.Range(1, 40);
            if (num < 25)
            {
                obs_car = Instantiate(Resources.Load("Car_14I"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
                obs_car.transform.parent = this.transform.GetChild(1).GetChild(0).GetChild(1);
                obs_car.transform.localPosition = new Vector3(-28.35f + x_val, -0.8f, 2.7f);
                obs_car.tag = "Obstacle";
                slot.Add(obs_car);
                stored_x.Add(obs_car.transform.localPosition.x);
                stored_z.Add(obs_car.transform.localPosition.z);
            }
            x_val = x_val - (float)2.01;
        }
        #endregion  

        #region Parking Second Line Code

        for (int j = 1; j < 2; j++)
        {
            x_val = 0;

            for (int i = 0; i < 2; i++)
            {
                num = Random.Range(1, 40);
                if (num < 25)
                {
                    obs_car = Instantiate(Resources.Load("Car_18C"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
                    obs_car.transform.parent = this.transform.GetChild(1).GetChild(0).GetChild(1);
                    obs_car.transform.localPosition = new Vector3(30.47f + x_val, -0.8f, 13.7f + z_val);
                    obs_car.tag = "Obstacle";
                    slot.Add(obs_car);
                    stored_x.Add(obs_car.transform.localPosition.x);
                    stored_z.Add(obs_car.transform.localPosition.z);
                }
                x_val = x_val + (float)2.01;
            }

            x_val = 0;
            for (int i = 4; i < 12; i++)
            {
                num = Random.Range(1, 40);
                if (num < 25)
                {
                    obs_car = Instantiate(Resources.Load("Car_18C"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
                    obs_car.transform.parent = this.transform.GetChild(1).GetChild(0).GetChild(1);
                    obs_car.transform.localPosition = new Vector3(2.18f + x_val, -0.8f, 13.7f + z_val);
                    obs_car.tag = "Obstacle";
                    slot.Add(obs_car);
                    stored_x.Add(obs_car.transform.localPosition.x);
                    stored_z.Add(obs_car.transform.localPosition.z);
                }
                x_val = x_val + (float)2.01;
            }


            x_val = 0;
            for (int i = 0; i < 9; i++)
            {
                num = Random.Range(1, 40);
                if (num < 25)
                {
                    obs_car = Instantiate(Resources.Load("Car_14I"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
                    obs_car.transform.parent = this.transform.GetChild(1).GetChild(0).GetChild(1);
                    obs_car.transform.localPosition = new Vector3(-22.38f + x_val, -0.8f, 13.7f + z_val);
                    obs_car.tag = "Obstacle";
                    slot.Add(obs_car);
                    stored_x.Add(obs_car.transform.localPosition.x);
                    stored_z.Add(obs_car.transform.localPosition.z);
                }
                x_val = x_val + (float)2.01;
            }

            x_val = 0;

            for (int i = 0; i < 2; i++)
            {
                num = Random.Range(1, 40);
                if (num < 25)
                {
                    obs_car = Instantiate(Resources.Load("Car_14I"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
                    obs_car.transform.parent = this.transform.GetChild(1).GetChild(0).GetChild(1);
                    obs_car.transform.localPosition = new Vector3(-30.47f + x_val, -0.8f, 13.7f + z_val);
                    obs_car.tag = "Obstacle";
                    slot.Add(obs_car);
                    stored_x.Add(obs_car.transform.localPosition.x);
                    stored_z.Add(obs_car.transform.localPosition.z);
                }
                x_val = x_val + (float)2.01;
            }




            z_val -= 4f;
        }

        #endregion Parking Seconde Line Code

        #region Flag Setting Code
        if (flag)
        {
            num = Random.Range(0, slot.Count);
            Destroy(slot[num]);
            car_agent.goal = Instantiate(Resources.Load("Target_Area"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
            car_agent.goal.transform.parent = this.transform.GetChild(1).GetChild(0).GetChild(1);
            car_agent.goal.transform.localPosition = new Vector3(stored_x[num], -0.5f, stored_z[num]);
            switch (stored_z[num])
            {
                case 2.7f:
                    car_agent.goal.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 13.7f:
                    car_agent.goal.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;

            }
            slot.Add(car_agent.goal);
            set_target(car_agent.goal);
        }
        else
        {
            if (Pos <= 1 | Pos > 50)
            {
                Debug.Log("Input 1~50!");

                if (UnityEditor.EditorApplication.isPlaying)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
                else
                {
                    Application.Quit();
                }

            }
            else if (Pos <= 2)
            {
                x_val = 32.3f + (-2.01f * (Pos - 1));
                z_val = 2.7f;
            }
            else if (Pos <= 20)
            {
                x_val = 16.1f + (-2.01f * (Pos - 3));
                z_val = 2.7f;
            }
            else if (Pos <= 22)
            {
                x_val = -28.35f + (-2.01f * (Pos - 21));
                z_val = 2.7f;
            }
            else if (Pos <= 24)
            {
                x_val = 32.48f + (-2.01f * (Pos - 23));
                z_val = 13.7f;
            }
            else if (Pos <= 36)
            {
                x_val = 24.29f + (-2.01f * (Pos - 25));
                z_val = 13.7f;
            }
            else if (Pos <= 48)
            {
                x_val = -0.27f + (-2.01f * (Pos - 37));
                z_val = 13.7f;
            }
            else if (Pos <= 50)
            {
                x_val = -28.46f + (-2.01f * (Pos - 49));
                z_val = 13.7f;
            }

            car_agent.goal = Instantiate(Resources.Load("Target_Area"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
            car_agent.goal.transform.parent = this.transform.GetChild(1).GetChild(0).GetChild(1);
            car_agent.goal.transform.localPosition = new Vector3(x_val, -0.8f, z_val);


            switch (z_val)
            {
                case 2.7f:
                    car_agent.goal.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 13.7f:
                    car_agent.goal.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;

            }

            foreach (var k in slot)
            {
                if (k.transform.position == car_agent.goal.transform.position)
                {
                    Destroy(k);
                }
            }
            slot.Add(car_agent.goal);
            set_target(car_agent.goal);
        }
        #endregion
    }

    public void clear_Parkinglot()
    {

        foreach (var k in slot)
        {
            Destroy(k);
        }
        slot.Clear();
    }


    public void set_target(GameObject target)
    {
        child = car_agent.goal.transform.GetChild(4).gameObject; // 자식 object 연결
        child.tag = "goal";

        for (int i = 0; i < 4; i = i + 2)
        {
            child = car_agent.goal.transform.GetChild(i).gameObject;
            child.tag = "goal_line";
        }
    }

    public float cal_angle()
    {
        Vector3 car_angle = new Vector3(0, 0, 0); // 임시변수
        Vector3 goal_angle = new Vector3(0, 0, 0); // 임시변수
        car_angle = car_agent.frontDriver.position - car_agent.backDriver.position;
        car_angle.y = 0f;
        goal_angle = car_agent.goal.transform.GetChild(0).transform.position - car_agent.goal.transform.GetChild(2).transform.position;
        goal_angle.y = 0f;
        return GetAngle(car_angle, goal_angle);
    }

    public static float GetAngle(Vector3 point1, Vector3 point2)              // 각도 구하기 (원점에서부터 각 점까지의 직선, 그 사이각)
    {
        Vector3 k = point1;
        Vector3 l = point2;

        return (Mathf.Atan2(k.x, k.z) * Mathf.Rad2Deg - Mathf.Atan2(l.x, l.z) * Mathf.Rad2Deg);
    }

    public float geta_angle()           // 각을 0~90범위로 변환 
    {
        float k = cal_angle();

        if (k >= -90 && k < 0)       // -90<k>0
        {
            return -k;
        }
        else if (k >= 0 && k < 90)   //   0<k<90
        {
            return k;
        }
        else if (k >= 90 && k < 180)    //  90 < k < 180
        {
            return (180 - k);
        }
        else if (k > -180 && k < -90)     // -180 < k > -90
        {
            return (-k + 180);
        }
        else
        {
            return 0;
        }
    }
}