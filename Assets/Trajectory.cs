using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] GameObject parentDot;
    [SerializeField] GameObject prefabDot;
    [SerializeField] int dotNumber;
    private Transform[] dots;

    private Vector3 ballPosition, mousePosition;
    bool showTrajectory
    {
        get
        {
            return showTrajectory;
        }
        set
        {
            showTrajectory = value;
        }
    }
    private float timeStamp = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        Hide();
        GenerateDots();
    }

    // Update is called once per frame
    public void Hide()
    {
        parentDot.SetActive(false);
       
    }
    public void Show()
    {
        parentDot.SetActive(true);
    }
    void GenerateDots()
    {
        dots = new Transform[dotNumber];
        for (int i = 0; i < dotNumber; i++)
        {
            dots[i] = Instantiate(prefabDot, null).transform;
            dots[i].parent = parentDot.transform;
        }
    }
    public void UpdateDots(Vector3 ballPosition, Vector3 mousePosition)
    {
        Vector3 pos = ballPosition;
        for (int i = 0; i < dotNumber; i++)
        {
            // Both mousePosition and ballPosition are screen points
            Vector3 direction = (mousePosition - ballPosition);
            pos.x = ballPosition.x + i * timeStamp * direction.x;
            pos.y = ballPosition.y + i * timeStamp * direction.y;
            pos.z = ballPosition.z;
            dots[i].position = Camera.main.ScreenToWorldPoint(pos);
        }
    }
}
