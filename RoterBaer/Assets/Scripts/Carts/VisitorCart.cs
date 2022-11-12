using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorCart : MonoBehaviour
{
    [SerializeField, Min(0.1f)] float MovementSpeed = 1;

    [SerializeField] Transform[] passengerPositions = new Transform[1];

    public int PassengerSeatCount => passengerPositions.Length;

    public TrackPoint OriginTrackPoint { get; private set; }
    public TrackPoint TargetTrackPoint { get; private set; }

    Coroutine movementCoroutine;

    Passenger[] passengers;

    bool isScareEndagered;

    public Vector3 Position
    {
        get => transform.position;
        private set => transform.position = value;
    }

    public void SetupCart(TrackPoint startPoint, Passenger[] passengers)
    {
        PlacePassengers(passengers);

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

        //Debug.Log($"Moving towards {aim.gameObject.name}");

        AudioManager.instance.WagonOverTrackpoint(this);

        MoveTowards(aim);
    }

    private void PlacePassengers(Passenger[] passengers)
    {
        this.passengers = passengers;

        for (int i = 0; i <Mathf.Min( passengers.Length, passengerPositions.Length); i++)
        {
            passengers[i].transform.SetParent(passengerPositions[i]);
            passengers[i].transform.localPosition = Vector3.zero;
        }
    }

    public void ScarePassengers(EMonsterType monsterType)
    {
        for (int i = 0; i < passengers.Length; i++)
            passengers[i].ScarePassenger(monsterType);
    }

    public void IsScareEndangered(bool isHovered)
    {
        if (isScareEndagered == isHovered) return;
        isScareEndagered = isHovered;

        if (isHovered)
            transform.localScale *= 1.1f;
        else
            transform.localScale /= 1.1f;
    }
}
