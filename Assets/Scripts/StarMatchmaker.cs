using UnityEngine;

public class StarMatchmaker : Photon.MonoBehaviour
{
    private PhotonView myPhotonView;
    private AsteroidManager asteroidManager;
    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    void OnJoinedLobby()
    {
        Debug.Log("JoinRandom");
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
        Debug.Log("CreateRoom");
        
    }

    void OnJoinedRoom()
    {

        GameObject spaceship = PhotonNetwork.Instantiate("spaceship", Vector3.zero, Quaternion.identity, 0);
        myPhotonView = spaceship.GetComponent<PhotonView>();

		if (PhotonNetwork.isMasterClient)
		{
			asteroidManager = gameObject.GetComponent<AsteroidManager>();
			asteroidManager.newAsteroids();
		}
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

        if (PhotonNetwork.connectionStateDetailed == PeerState.Joined)
        {
        }
    }
}
