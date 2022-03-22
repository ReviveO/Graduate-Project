using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using System.Linq;

[RequireComponent(typeof(CarController))]
[RequireComponent(typeof(Rigidbody))]

public class UIController : MonoBehaviour
{
    public Text motorTorque;
    public Text steeringAngle;
    public Text brake;
    public Text step;
    public Text episode;
    public RawImage w;
    public RawImage s;
    public RawImage a;
    public RawImage d;

    GameObject[] env;
    CarAgent agent = null;
    CarController car_state = null;
    CameraViewChanger cvc;
    CameraViewChanger cvc2;
    public int agent_num;

    public Canvas Canvas
    {
        get;
        private set;
    }

    void Awake()
    {
        Canvas = GetComponent<Canvas>();
    }
    void Start()
    {
        w = GameObject.Find("w").GetComponent<RawImage>();
        s = GameObject.Find("s").GetComponent<RawImage>();
        a = GameObject.Find("a").GetComponent<RawImage>();
        d = GameObject.Find("d").GetComponent<RawImage>();

        env = GameObject.FindGameObjectsWithTag("ENV");
        var sortedList = env.OrderBy(go => go.name).ToList();
        car_state = sortedList[agent_num].transform.GetChild(2).GetComponent<CarController>();
        agent = sortedList[agent_num].transform.GetChild(2).GetComponent<CarAgent>();
    }

    void OnValidate()                // 편집기에서만 호출되는 함수 .. 처음 스크립트가 실행될때, 값이 변경될때. 관찰할 환경에 해당하는 카메라/UI 설정
    {
         
        env = GameObject.FindGameObjectsWithTag("ENV");
        var sortedList = env.OrderBy(go => go.name).ToList();
        agent_num = agent_num % env.Length;

        car_state = sortedList[agent_num].transform.GetChild(2).GetComponent<CarController>();
        agent = sortedList[agent_num].transform.GetChild(2).GetComponent<CarAgent>();

        cvc = GameObject.Find("Camera_control").GetComponent<CameraViewChanger>();
        cvc2 = GameObject.Find("CameraChangeButton").GetComponent<CameraViewChanger>();


        foreach (var a in sortedList)
        {
            a.transform.GetChild(5).GetChild(1).GetComponent<Camera>().enabled = false;
            a.transform.GetChild(5).GetChild(0).GetComponent<Camera>().enabled = false;
        }

        cvc.MainCam = sortedList[agent_num].transform.GetChild(5).GetChild(1).GetComponent<Camera>();
        cvc.AgentCam = sortedList[agent_num].transform.GetChild(5).GetChild(0).GetComponent<Camera>();
        cvc.MainCam.enabled = true;
        cvc.AgentCam.enabled = false;
        cvc2.MainCam = sortedList[agent_num].transform.GetChild(5).GetChild(1).GetComponent<Camera>();
        cvc2.AgentCam = sortedList[agent_num].transform.GetChild(5).GetChild(0).GetComponent<Camera>();
        cvc2.MainCam.enabled = true;
        cvc2.AgentCam.enabled = false;


    }

    void Update()
    {
        /*car_state = GameObject.Find("Agent").GetComponent<CarController>();
        agent = GameObject.Find("Agent").GetComponent<CarAgent>();
     */



        float motor = car_state.maxMotorTorque * car_state.CurrentAcceleration;
        float steering = car_state.maxSteeringAngle * car_state.CurrentSteeringAngle;

   
        // UI wasd 색상 변경 부분
        // 앞뒤 (ws부분)
        if (car_state.CurrentAcceleration > 0)
        {
            w.color = new Color(227 / 255f, 71 / 255f, 71 / 255f);
            s.color = new Color(1, 1, 1);
        }
        else if (car_state.CurrentAcceleration == 0)
        {
            w.color = new Color(1, 1, 1);
            s.color = new Color(1, 1, 1);
        }
        else
        {
            w.color = new Color(1, 1, 1);
            s.color = new Color(227 / 255f, 71 / 255f, 71 / 255f);
        }

        // 좌우 (ad부분)
        if (car_state.CurrentSteeringAngle > 0)
        {
            d.color = new Color(227 / 255f, 71 / 255f, 71 / 255f);
            a.color = new Color(1, 1, 1);
        }
        else if (car_state.CurrentSteeringAngle == 0)
        {
            d.color = new Color(1, 1, 1);
            a.color = new Color(1, 1, 1);
        }
        else
        {
            d.color = new Color(1, 1, 1);
            a.color = new Color(227 / 255f, 71 / 255f, 71 / 255f);
        }

        // 차 motor, steer 출력부분
        if (car_state != null && agent != null)
        {
            motorTorque.text = "Motor: " + car_state.CurrentAcceleration.ToString();
            steeringAngle.text = "Steer: " + car_state.CurrentSteeringAngle.ToString();
            brake.text = "Brake: " + car_state.CurrentBrakeTorque.ToString();
            step.text = "Step: " + agent.StepCount.ToString();
            episode.text = "Episode: " + agent.CompletedEpisodes.ToString();
        }
        else
        {
            Debug.Log("GameObject Connection Error!");
        }
    }
}
