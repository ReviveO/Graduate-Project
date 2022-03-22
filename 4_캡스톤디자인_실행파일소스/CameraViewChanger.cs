using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class CameraViewChanger : MonoBehaviour
{
    // ChangeCamera() - MainCam과 AgentCam을 변경하는 함수

    [HideInInspector] public Camera MainCam;
    [HideInInspector] public Camera AgentCam;

    List<GameObject> sortedList = new List<GameObject>();
    [HideInInspector] public GameObject[] env_list;
    [HideInInspector] public UIController uc;

    // 시작할 때에 관찰할 환경의 MainCam ON, AgentCam OFF     , 나머지 카메라는 OFF
    void Start()
    {
        env_list = GameObject.FindGameObjectsWithTag("ENV");
        uc = GameObject.Find("UI").GetComponent<UIController>();
        uc.agent_num = uc.agent_num % env_list.Length;

        sortedList = env_list.OrderBy(go => go.name).ToList();


        foreach (var a in sortedList)
        {
            a.transform.GetChild(5).GetChild(1).GetComponent<Camera>().enabled = false;
            a.transform.GetChild(5).GetChild(0).GetComponent<Camera>().enabled = false;
        }
        MainCam = sortedList[uc.agent_num].transform.GetChild(5).GetChild(1).GetComponent<Camera>();
        AgentCam = sortedList[uc.agent_num].transform.GetChild(5).GetChild(0).GetComponent<Camera>();
        MainCam.enabled = true;
        AgentCam.enabled = false;
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        ChangeCamera();
    }

    public void ChangeCamera()
    {
        if (MainCam.GetComponent<Camera>().enabled == true && AgentCam.GetComponent<Camera>().enabled == false)
            {
                MainCam.GetComponent<Camera>().enabled = false;
                AgentCam.GetComponent<Camera>().enabled = true;
            }
        else
            {
                MainCam.GetComponent<Camera>().enabled = true;
                AgentCam.GetComponent<Camera>().enabled = false;
            }
    }
}
