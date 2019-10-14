using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enum_CurrentPosition
{
    LEFT,
    MIDDLE,
    RIGHT
}

public class IconManager : MonoBehaviour
{
    public Enum_CurrentPosition Position = Enum_CurrentPosition.MIDDLE;

    public Vector3 CurrentV3;// = Vector3.zero;
    public Vector3 DestinationV3;// = Vector3.zero;

    public bool IsMoving = false;

    public float Timer;

    public int IconMovementSpeed; 

    // Start is called before the first frame update
    void Start()
    {
        Timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;

        if (IsMoving)
        {
            transform.position = Vector3.Lerp(CurrentV3, DestinationV3, Timer * IconMovementSpeed); 
            if (Timer == 1 || CurrentV3 == DestinationV3)
            {
                IsMoving = false; 
            }
        }
    }

    public void Move(Enum_CurrentPosition currentPosition)
    {
        IsMoving = true;
        Timer = 0.0f; 
        switch(currentPosition)
        {
            case Enum_CurrentPosition.LEFT:
                if (Position != Enum_CurrentPosition.LEFT)                
                    Position--;                 
                break;
            case Enum_CurrentPosition.RIGHT:
                if (Position != Enum_CurrentPosition.RIGHT)
                    Position++; 
                break;
        }
    }


}
