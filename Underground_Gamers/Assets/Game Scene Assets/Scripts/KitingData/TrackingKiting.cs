using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "TrackingKiting.Asset", menuName = "KitingData/TrackingKiting")]
public class TrackingKiting : KitingData
{
    public GameObject point;

    [Tooltip("근접 카이팅 범위")]
    public float trackingKitingRange;
    public override void UpdateKiting(Transform target, AIController ctrl)
    {
        // 타겟과의 중간 지점에서 랜덤한 카이팅 위치 생성
        var coltarget = target.GetComponent<Collider>();
        var origin = ctrl.transform.position;
        Vector3 kitingRandomPoint = Vector3.zero;


        // 타겟과의 Y좌표 통일하여, XZ평면상에서의 계산
        Vector3 targetPos = target.position;
        targetPos.y = ctrl.transform.position.y;

        float colDis = 0;
        if (coltarget != null)
        {
            if (coltarget.bounds.extents.x >= coltarget.bounds.extents.z)
                colDis = coltarget.bounds.extents.x;
            else
                colDis = coltarget.bounds.extents.z;
        }


        // 타겟과의 중간지점 계산
        float dis = Vector3.Distance(targetPos, origin) - colDis;
        Vector3 dirToTarget = targetPos - origin;
        dirToTarget.Normalize();
        Vector3 midPoint = (dirToTarget * (dis * 0.5f)) + origin;

        // 콜라이더 반경, 굳이 해야될까?
        //if (coltarget != null)
        //{
        //    var targetpos = target.position;
        //    targetpos.y = origin.y;
        //    var coldir = origin - target.position;
        //    coldir.Normalize();
        //    var coldis = coldir * coltarget.bounds.extents.x;
        //    midPoint += coldis;
        //}

        Vector3 pointInNavMesh = Vector3.zero;

        if (Utils.RandomPointInNav(midPoint, trackingKitingRange, attemp, out pointInNavMesh))
        {
            ctrl.SetDestination(pointInNavMesh); // 변경 필요
            ctrl.kitingPos = pointInNavMesh;
            //GameObject debugPoint = Instantiate(point, pointInNavMesh, Quaternion.identity);
            //Destroy(debugPoint, 2f);
        }
        else
        {
            ctrl.SetDestination(pointInNavMesh); // 변경 필요
            ctrl.kitingPos = pointInNavMesh;
            //GameObject debugPoint = Instantiate(point, pointInNavMesh, Quaternion.identity);
            //Destroy(debugPoint, 2f);
        }
    }
}
