using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorCart : MonoBehaviour
{
    public TrackPoint OriginTrackPoint { get; private set; }
    public TrackPoint TargetTrackPoint { get; private set; }

    Coroutine movementCoroutine;

    public Vector3 Position
    {
        get => transform.position;
        private set => transform.position = value;
    }

    [SerializeField, Min(0.1f)] float MovementSpeed = 1;

    public void StartRunning(TrackPoint startPoint)
    {
        OriginTrackPoint = startPoint;
        MoveTowards(startPoint.GetNextNode(OriginTrackPoint));
    }

    private void MoveTowards(TrackPoint nextAim)
    {
        if (movementCoroutine != null)
            Stop();

        OriginTrackPoint = TargetTrackPoint;
        TargetTrackPoint = nextAim;

        movementCoroutine = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        Vector3 startPosition = Position;
        Vector3 targetPosition = TargetTrackPoint.Position;

        float movementTime = Vector3.Distance(startPosition, targetPosition) / MovementSpeed;

        //Debug.Log($"[{GetType().Name}] Moves from {OriginTrackPoint.gameObject.name} to {TargetTrackPoint.gameObject.name} in {movementTime}.", this);

        float elapsed = 0;

        float lerp;

        while (elapsed < movementTime)
        {
            lerp = Mathf.Lerp(0, 1, elapsed / movementTime);

            Position = lerp * targetPosition + (1f - lerp) * startPosition;

            elapsed += Time.deltaTime;
            yield return null;
        }
        Position = targetPosition;

        ReachedPoint();
    }

    private void Stop()
    {
        StopCoroutine(movementCoroutine);
        movementCoroutine = null;
    }

    private void ReachedPoint()
    {
        if (TargetTrackPoint.TrackType == ETrackType.End)
        {
            VisitorCartController.Active.ReachedEnd(this);
            return;
        }
        TrackPoint aim = TargetTrackPoint.GetNextNode(OriginTrackPoint);

        Debug.Log($"Moving towards {aim.gameObject.name}");
        MoveTowards(aim);
    }
}
