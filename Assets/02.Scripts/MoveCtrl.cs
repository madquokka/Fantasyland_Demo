using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCtrl : MonoBehaviour
{
    public enum MoveType
    {
        WAY_POINT,
        LOOK_AT,
        DAYDREAM
    }
    //이동방식
    public MoveType moveType = MoveType.WAY_POINT;

    //이동속도
    public float speed = 1.0f;

    //회전속도
    public float damping = 3.0f;

    //모든 waypoint를 저장할 배열
    public Transform[] points;

    //Transform 컴포넌트를 저장할 변수
    private Transform tr;

    //CharacterController 컴포넌트를 저장할 변수
    private CharacterController cc;

    //MainCamera의 Transform 컴포넌트를 저장할 변수
    private Transform camTr;

    //다음에 이동해야 할 위치 인덱스 변수
    private int nextIdx = 1;

    // Start is called before the first frame update
    private void Start()
    {
        //Transform 컴포넌트 추출 후 변수에 저장
        tr = GetComponent<Transform>();

        //Character Controller 컴포넌트 추출 후 변수에 저장
        camTr = Camera.main.GetComponent<Transform>();

        //waypointgroup 게임오브젝트를 검색해 변수에 저장
        GameObject wayPointGroup = GameObject.Find("WayPointGroup");

        if(wayPointGroup != null)
        {
            //WayPointGroup 하위에 있는 모든 게임오브젝트와 Transform 컴포넌트 추출
            points = wayPointGroup.GetComponentsInChildren<Transform>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        switch (moveType)
        {
            case MoveType.WAY_POINT :
                MoveWayPoint();
                break;

            case MoveType.LOOK_AT:
                break;

            case MoveType.DAYDREAM:
                break;
        }
        
    }

    //웨이포인트 경로로 이동하는 로직
   private void MoveWayPoint()
    {
        //현재 위치에서 다음 웨이포인트로 향하는 벡터를 계산
        Vector3 direction = points[nextIdx].position - tr.position;

        //산출된 벡터의  회전 각도를 쿼터니언 타입으로 산출
        Quaternion rot = Quaternion.LookRotation(direction);

        //현재 각도에서 회전해야 할 각도까지 부드럽게 회전처리
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);

        //전진 방향으로 이동처리
        tr.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider coll)
    {
        //웨이포인트(point 게임오브젝트)에 충돌 여부 판단
        if (coll.CompareTag("WAY_POINT"))
        {
            //맨 마지막 웨이포인트에 도달했을 떄 처음 인덱스로 변경
            nextIdx = (++nextIdx >= points.Length) ? 1 : nextIdx;
        }
    }
}
