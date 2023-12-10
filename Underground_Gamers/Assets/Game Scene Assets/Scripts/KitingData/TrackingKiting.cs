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

        // 타겟과의 중간지점 계산
        float dis = Vector3.Distance(targetPos, origin);
        Vector3 dirToTarget = targetPos - origin;
        dirToTarget.Normalize();
        Vector3 midPoint = (dirToTarget * (dis * 0.5f)) + origin;

        // 콜라이더 반경, 굳이 해야될까?
        if (coltarget != null)
        {
            var targetpos = target.position;
            targetpos.y = origin.y;
            var coldir = origin - target.position;
            coldir.Normalize();
            var coldis = coldir * coltarget.bounds.extents.x;
            midPoint += coldis;
        }

        Vector3 pointInNavMesh = Vector3.zero;

        if (Utils.RandomPointInNav(midPoint, trackingKitingRange, attemp, out pointInNavMesh))
        {
            ctrl.SetDestination(pointInNavMesh); // 변경 필요
            ctrl.kitingPos = pointInNavMesh;
            GameObject debugPoint = Instantiate(point, pointInNavMesh, Quaternion.identity);
            Destroy(debugPoint, 2f);
        }
        else
        {
            ctrl.SetDestination(pointInNavMesh); // 변경 필요
            ctrl.kitingPos = pointInNavMesh;
            GameObject debugPoint = Instantiate(point, pointInNavMesh, Quaternion.identity);
            Destroy(debugPoint, 2f);
        }



        //kitingRandomPoint = Utils.RandomPointInCircle(trackingKitingRange, ctrl.battleTarget);
        //kitingRandomPoint.y = ctrl.transform.position.y;
        //var colTarget = target.GetComponent<Collider>();
        //var center = ctrl.transform.position;

        //if (colTarget != null)
        //{
        //    var targetPos = target.position;
        //    targetPos.y = center.y;
        //    var colDir = center - target.position;
        //    colDir.Normalize();
        //    var colDis = colDir * colTarget.bounds.extents.x;
        //    kitingRandomPoint += colDis;
        //}
        //kitingRandomPoint += center;
        //while (attempt < 30)
        //{
        //    NavMeshHit hit;
        //    if (NavMesh.SamplePosition(kitingRandomPoint, out hit, ctrl.agent.radius, NavMesh.AllAreas))
        //    {
        //        NavMeshPath path = new NavMeshPath();
        //        if (NavMesh.CalculatePath(center, hit.position, NavMesh.AllAreas, path))
        //        {
        //            if (path.status == NavMeshPathStatus.PathComplete)
        //            {
        //                kitingRandomPoint.y = center.y;
        //                ctrl.SetDestination(kitingRandomPoint); // 변경 필요
        //                ctrl.kitingPos = kitingRandomPoint;
        //                GameObject debugPoint = Instantiate(point, kitingRandomPoint, Quaternion.identity);
        //                Destroy(debugPoint, 2f);
        //                return;
        //                //return hit.position;
        //            }
        //        }
        //    }

        //    attempt++;
        //}

        //// 실패 시 기본값 반환
        //Debug.Log("탐색 실패");
        //GameObject debugFailedPoint = Instantiate(point, kitingRandomPoint, Quaternion.identity);



        //int attempt = 0;
        //Vector3 kitingRandomPoint = Vector3.zero;
        //var colTarget = target.GetComponent<Collider>();

        //while (attempt < 30)
        //{
        //    if (colTarget != null)
        //    {
        //        var colDir = ctrl.transform.position - target.position;
        //        colDir.Normalize();
        //        var colDis = colDir * colTarget.bounds.extents.magnitude;
        //        kitingRandomPoint += colDis;
        //    }

        //    kitingRandomPoint = Utils.RandomPointInCircle(trackingKitingRange, ctrl.battleTarget);
        //    kitingRandomPoint.y = ctrl.transform.position.y;

        //    NavMeshHit hit;
        //    if (NavMesh.SamplePosition(kitingRandomPoint, out hit, 1.0f, NavMesh.AllAreas))
        //    {
        //        kitingRandomPoint = hit.position;
        //        ctrl.SetDestination(kitingRandomPoint); // 변경 필요
        //        ctrl.kitingPos = kitingRandomPoint;
        //        GameObject debugPoint = Instantiate(point, kitingRandomPoint, Quaternion.identity);
        //        Destroy(debugPoint, 2f);
        //        return;
        //    }

        //    attempt++;
        //}

        //ctrl.SetState(States.Kiting);
    }
}
