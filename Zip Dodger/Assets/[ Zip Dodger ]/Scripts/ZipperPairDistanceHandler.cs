using System;
using UnityEngine;

[RequireComponent(typeof(ZipperPairHandler))]
public class ZipperPairDistanceHandler : MonoBehaviour
{
    private ZipperPairHandler zipperPairHandler;

    [SerializeField] float zAxisOffset = 0.3f;

    //[Header("-- SETUP --")]
    //[SerializeField, Tooltip("Speed of opening and closing of zipper pairs.")] private float distanceChangeSpeed = 10f;
    //[SerializeField, Tooltip("Fully closed distance of zipper pairs.")] private float defaultDistance = 1.5f;
    //[SerializeField, Tooltip("Fully opened distance of zipper pairs. This will change according to the zip length.")] private float maxDistance = 3f;
    private float currentDistance;
    public float CurrentDistance
    {
        get
        {
            if (currentDistance <= zipperPairHandler.GapMovement.DefaultDistance)
                currentDistance = zipperPairHandler.GapMovement.DefaultDistance;
            else if (currentDistance >= zipperPairHandler.GapMovement.MaxDistance)
                currentDistance = zipperPairHandler.GapMovement.MaxDistance;

            return currentDistance;
        }
        set
        {
            currentDistance = value;
        }
    }

    // This will be between 0 and 1;
    private float affectRatio;
    public float AffectRatio
    {
        get
        {
            if (affectRatio <= 0)
                affectRatio = 0f;
            else if (affectRatio >= 1f)
                affectRatio = 1f;

            return affectRatio;
        }
        private set
        {
            affectRatio = value;
        }
    }

    private int rowOfThisZipper;

    private void OnEnable()
    {
        zipperPairHandler = GetComponent<ZipperPairHandler>();

        CurrentDistance = zipperPairHandler.GapMovement.DefaultDistance;
    }

    private void Update()
    {
        if (zipperPairHandler.CantMove) return;
        
        if (zipperPairHandler._Type == ZipperPairHandler.Type.Parent)
            rowOfThisZipper = 0;
        else if (zipperPairHandler._Type == ZipperPairHandler.Type.Child)
            rowOfThisZipper = zipperPairHandler.ChildZipper.Row;

        UpdateAffectRatio();

        // Calculate current distance from affectRatio.
        CurrentDistance = zipperPairHandler.GapMovement.MaxDistance- ((1 - AffectRatio) * (zipperPairHandler.GapMovement.MaxDistance - zipperPairHandler.GapMovement.DefaultDistance));
        //AffectRatio = 1 - ((maxDistance - CurrentDistance) / (maxDistance - defaultDistance));

        //Vector3 newZipperPos = new Vector3(CurrentDistance * 0.5f, 0f, 0f);
        //newZipperPos = transform.InverseTransformDirection(newZipperPos);

        if (zipperPairHandler._Side == ZipperPairHandler.Side.Left)
        {
            Vector3 newPos = new Vector3(CurrentDistance * -0.5f, 0f, -zAxisOffset);
            //newPos = transform.InverseTransformDirection(newPos);
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, zipperPairHandler.GapMovement.DistanceChangeSpeed * Time.deltaTime);

        }
        else if (zipperPairHandler._Side == ZipperPairHandler.Side.Right)
        {
            Vector3 newPos = new Vector3(CurrentDistance * 0.5f, 0f, zAxisOffset);
            //newPos = transform.InverseTransformDirection(newPos);
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, zipperPairHandler.GapMovement.DistanceChangeSpeed * Time.deltaTime);
        }
    }

    private void UpdateAffectRatio()
    {
        //if (zipperPairHandler.ChildZipper.Row == zipperPairHandler.Testing.Affect)
        //    AffectRatio = 1;
        //else if (zipperPairHandler.ChildZipper.Row < zipperPairHandler.Testing.Affect - zipperPairHandler.Testing.AffectLimit && zipperPairHandler.ChildZipper.Row > zipperPairHandler.Testing.Affect - zipperPairHandler.Testing.AffectLimit)
        //    AffectRatio = 0;
        //else
        //{
        //    AffectRatio = (zipperPairHandler.ChildZipper.Row - zipperPairHandler.Testing.AffectLimit) / (zipperPairHandler.Testing.Affect - zipperPairHandler.ChildZipper.Row);
        //    Debug.Log("Effecting.");
        //}

        //if (rowOfThisZipper < (Player.CurrentRow - zipperPairHandler.Testing.AffectLimit) || rowOfThisZipper > (Player.CurrentRow + zipperPairHandler.Testing.AffectLimit))
        //    return;

        //if (rowOfThisZipper < (zipperPairHandler.Testing.Affect - zipperPairHandler.Testing.AffectLimit) || rowOfThisZipper > (zipperPairHandler.Testing.AffectLimit + zipperPairHandler.Testing.AffectLimit))
        //    return;

        //AffectRatio = 1 - (Mathf.Abs(zipperPairHandler.Testing.Affect - rowOfThisZipper) / (rowOfThisZipper - zipperPairHandler.Testing.AffectLimit));

        if (rowOfThisZipper >= (zipperPairHandler.GapMovement.Affect - zipperPairHandler.GapMovement.AffectLimit) && rowOfThisZipper <= (zipperPairHandler.GapMovement.Affect + zipperPairHandler.GapMovement.AffectLimit))
            AffectRatio = 1 - (Mathf.Abs(zipperPairHandler.GapMovement.Affect - rowOfThisZipper) / zipperPairHandler.GapMovement.AffectLimit);
        else
            AffectRatio = 0;
    }
}
