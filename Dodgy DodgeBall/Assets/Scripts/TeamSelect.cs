using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//enum Enum_CurrentPosition
//{
//    LEFT,
//    MIDDLE,
//    RIGHT
//}

///// <summary>
///// 
///// </summary>
//class PlayerManager
//{
//    public Enum_CurrentPosition Position = Enum_CurrentPosition.MIDDLE;

//    public Vector3 CurrentV3;
//    public Vector3 DestinationV3;

//    public bool IsMoving = false;

//    public float Timer;

//    public void Start()
//    {
//        Timer = 0.0f; 
//    }
//    public void Update()
//    {
//        Timer += Time.deltaTime; 

//        if (IsMoving)
//        {

//        }
//    }
//}

[System.Serializable] public class Location
{
    public Vector3 Position;
    [HideInInspector] public bool IsUsed;
}


/// <summary>
/// Description:    Team select controls and making it look pretty 
/// Author:         Connor Young 
/// Creation Date:  14/10/19
/// Modified:       14/10/19
/// </summary>
public class TeamSelect : MonoBehaviour
{
    //Publically assigned Gameobjects and Positions---------------
    //public GameObject RedTeam;
    //public GameObject BlueTeam;

    public Image RedIcon;
    public Image BlueIcon;

    //public GameObject RedTeamLocation;
    //public GameObject BlueTeamLocation;
    public GameObject RedTeamStart;
    public GameObject BlueTeamStart;

    public List<Location> RedTeam = new List<Location>();
    public List<Location> Blueteam = new List<Location>();
    public List<Location> Middle = new List<Location>();


    //-------------------------------------------------------------


    //List Of Players----------------------------------------------
    //private PlayerManager PlayerOne = new PlayerManager();
    //private PlayerManager PlayerTwo = new PlayerManager();
    //private PlayerManager PlayerThree = new PlayerManager();
    //private PlayerManager PlayerFour = new PlayerManager();
    public List<IconManager> Players = new List<IconManager>();
    
    //-------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        //RedTimer = 0.0f;
        //BlueTimer = 0.0f;
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].GetComponent<IconManager>();
        }

        //Players[0].CurrentV3 = Middle[0].Position;
        //Players[0].DestinationV3 = Players[0].CurrentV3;
        //Players[1].CurrentV3 = Middle[1].Position;
        //Players[1].DestinationV3 = Players[1].CurrentV3;
    }

    #region Attempt 1
    // Update is called once per frame
    //void Update()
    //{
    //    PlayerOne.Update();
    //    PlayerTwo.Update(); 

    //    if (Input.GetKeyDown(KeyCode.LeftArrow))
    //    {
    //        if (PlayerOne.Position != Enum_CurrentPosition.LEFT)
    //        {
    //            PlayerOne.Position--;
    //        }
    //        if (PlayerOne.Position == Enum_CurrentPosition.MIDDLE)
    //        {
    //            PlayerOne.IsMoving = true;
    //            PlayerOne.Timer = 0.0f;
    //            PlayerOne.CurrentV3 = RedIcon.transform.position;
    //            PlayerOne.DestinationV3 = RedTeamLocation.transform.position;
    //            PlayerOne.Position = Enum_CurrentPosition.LEFT;
    //        }
    //        else if (PlayerOne.Position == Enum_CurrentPosition.RIGHT)
    //        {
    //            PlayerOne.IsMoving = true;
    //            PlayerOne.Timer = 0.0f;
    //            PlayerOne.CurrentV3 = RedIcon.transform.position;
    //            PlayerOne.DestinationV3 = RedTeamStart.transform.position;
    //            PlayerOne.Position = Enum_CurrentPosition.MIDDLE;
    //        }
    //    }
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        if (PlayerTwo.Position == Enum_CurrentPosition.MIDDLE)
    //        {
    //            PlayerTwo.IsMoving = true;
    //            PlayerTwo.Timer = 0.0f;
    //            PlayerTwo.CurrentV3 = BlueIcon.transform.position;
    //            PlayerTwo.DestinationV3 = RedTeamLocation.transform.position;
    //            PlayerTwo.Position = Enum_CurrentPosition.LEFT;
    //        }
    //        else if (PlayerTwo.Position == Enum_CurrentPosition.RIGHT)
    //        {
    //            PlayerTwo.IsMoving = true;
    //            PlayerTwo.Timer = 0.0f;
    //            PlayerTwo.CurrentV3 = BlueIcon.transform.position;
    //            PlayerTwo.DestinationV3 = BlueTeamStart.transform.position;
    //            PlayerTwo.Position = Enum_CurrentPosition.MIDDLE;

    //        }
    //    }
    //    if (Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        if (PlayerOne.Position == Enum_CurrentPosition.MIDDLE)
    //        {
    //            PlayerOne.IsMoving = true;
    //            PlayerOne.Timer = 0.0f;
    //            PlayerOne.CurrentV3 = RedIcon.transform.position;
    //            PlayerOne.DestinationV3 = BlueTeamLocation.transform.position;
    //            PlayerOne.Position = Enum_CurrentPosition.RIGHT;
    //        }
    //        else if (PlayerOne.Position == Enum_CurrentPosition.LEFT)
    //        {
    //            PlayerOne.IsMoving = true;
    //            PlayerOne.Timer = 0.0f;
    //            PlayerOne.CurrentV3 = RedIcon.transform.position;
    //            PlayerOne.DestinationV3 = RedTeamStart.transform.position;
    //            PlayerOne.Position = Enum_CurrentPosition.MIDDLE;
    //        }
    //    }
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        if (PlayerTwo.Position == Enum_CurrentPosition.MIDDLE)
    //        {
    //            PlayerTwo.IsMoving = true;
    //            PlayerTwo.Timer = 0.0f;
    //            PlayerTwo.CurrentV3 = BlueIcon.transform.position;
    //            PlayerTwo.DestinationV3 = BlueTeamLocation.transform.position;
    //            PlayerTwo.Position = Enum_CurrentPosition.RIGHT;
    //        }
    //        else if (PlayerTwo.Position == Enum_CurrentPosition.LEFT)
    //        {
    //            PlayerTwo.IsMoving = true;
    //            PlayerTwo.Timer = 0.0f;
    //            PlayerTwo.CurrentV3 = BlueIcon.transform.position;
    //            PlayerTwo.DestinationV3 = BlueTeamStart.transform.position;
    //            PlayerTwo.Position = Enum_CurrentPosition.MIDDLE; 
    //        }
    //    }

    //    if (PlayerOne.IsMoving)
    //    {
    //        RedIcon.transform.position = Vector3.Lerp(PlayerOne.CurrentV3, PlayerOne.DestinationV3, PlayerOne.Timer * 2);
    //        if (PlayerOne.Timer == 1)
    //        {
    //            PlayerOne.IsMoving = false;
    //        }
    //    }
    //    if (PlayerTwo.IsMoving)
    //    {
    //        BlueIcon.transform.position = Vector3.Lerp(PlayerTwo.CurrentV3, PlayerTwo.DestinationV3, PlayerTwo.Timer * 2);
    //        if (PlayerTwo.Timer == 1)
    //        {
    //            PlayerTwo.IsMoving = false; 
    //        }
    //    }
    //}
    #endregion

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Test1();
            Players[0].Move(Enum_CurrentPosition.LEFT);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Test1();
            Players[1].Move(Enum_CurrentPosition.LEFT);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Test1();
            Players[0].Move(Enum_CurrentPosition.RIGHT);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Test1();
            Players[1].Move(Enum_CurrentPosition.RIGHT);
        }

        


        //Players[0].DestinationV3 = GetValidPosition(Players[0].Position);
        //Players[1].DestinationV3 = GetValidPosition(Players[1].Position); 

    }

    public void UnsetLocation(Vector3 position)
    {
        foreach(Location current in RedTeam)
        {
            if (current.Position == position)
            {
                current.IsUsed = false;
                return;
            }
        }
        foreach (Location current in Blueteam)
        {
            if (current.Position == position)
            {
                current.IsUsed = false;
                return;
            }
        }
        foreach (Location current in Middle)
        {
            if (current.Position == position)
            {
                current.IsUsed = false;
                return;
            }
        }

    }

    public Vector3 GetValidPosition(Enum_CurrentPosition destination)
    {
        if (destination == Enum_CurrentPosition.LEFT)
        {
            foreach (Location current in RedTeam)
            {
                if (!current.IsUsed)
                {
                    current.IsUsed = true;
                    return current.Position;
                }
            }
        }
        else if (destination == Enum_CurrentPosition.RIGHT)
        {
            foreach (Location current in Blueteam)
            {
                if (!current.IsUsed)
                {
                    current.IsUsed = true;
                    return current.Position;
                }
            }
        }
        else if (destination == Enum_CurrentPosition.MIDDLE)
        {
            foreach (Location current in Middle)
            {
                if (!current.IsUsed)
                {
                    current.IsUsed = true;
                    return current.Position;
                }
            }
        }
        return Vector3.zero;
    }

    public void Test1()
    {
        foreach (IconManager current in Players)
        {
            Vector3 newDestination = GetValidPosition(current.Position);
            if (newDestination != Vector3.zero)
            {
                UnsetLocation(current.CurrentV3);
                current.DestinationV3 = newDestination;
            }
        }
    }
}

