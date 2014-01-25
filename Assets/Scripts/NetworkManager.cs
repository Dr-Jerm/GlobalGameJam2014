using UnityEngine;
using System.Collections;



public class NetworkManager : MonoBehaviour {

	private float btnX;
	private float btnY;
	private float btnW;
	private float btnH;
	private HostData[] hostData = new HostData[0];
	private string gameName = "space_combat";

	public GameObject playerPrefab;
	public Transform spawnObject;

	// Use this for initialization
	void Start () {
		this.btnX = Screen.width * 0.1F;
		this.btnY = Screen.width * 0.1F;
		this.btnH = Screen.height * 0.1F;
		this.btnW = Screen.width * 0.1F;
	}

	void OnServerInitialized() {
		Debug.Log ("initialized server");
		this.spawnPlayer();
	}

	void spawnPlayer() {
		Debug.Log ("Player spawning");
		Network.Instantiate(playerPrefab, spawnObject.position, Quaternion.identity, 0);
	}

	void OnConnectedToServer() {
		this.spawnPlayer ();
	}

	void startServer() {
		Debug.Log("starting server");
		Network.InitializeServer(32, 25001, false);
		Debug.Log("Server initialized");
		MasterServer.RegisterHost(this.gameName, "gamename", "This is our test game");
	}

	void refreshHostList() {
		Debug.Log("refreshing hosts");
		MasterServer.RequestHostList(this.gameName);
		Debug.Log ("got hosts");
	}

	void OnMasterServerEvent(MasterServerEvent mse) {
		if (mse == MasterServerEvent.RegistrationSucceeded) {
			Debug.Log("Registration succeeded.");
		}
	}

	// Update is called once per frame
	void Update () {
		if (MasterServer.PollHostList().Length != 0) {
			this.hostData = MasterServer.PollHostList();
			int i = 0;
			while (i < hostData.Length) {
				Debug.Log("Game name: " + hostData[i].gameName);
				i++;
			}
			MasterServer.ClearHostList();
		}
	}

	void OnGUI() {
		if (!Network.isServer) {
			Rect r1 = new Rect(btnX, btnY, btnH, btnW);
			Rect r2 = new Rect(btnX, btnY * 2, btnH, btnW);
			
			if (GUI.Button (r1, "Start Server")) {
				this.startServer();
			}
			if (GUI.Button (r2, "Refresh Host")) {
				Debug.Log ("refreshing hosts");
				this.refreshHostList();
			}
			for (int i = 0; i < this.hostData.Length; i++) {
				if (GUI.Button(new Rect(this.btnX * 1.5F, this.btnY * i * 1.5F, this.btnW, this.btnH), this.hostData[i].gameName)) {
					Debug.Log("ip is: " + this.hostData[i].ip[0]);
					Debug.Log("network status is: " + Network.TestConnection());
					Network.Connect(this.hostData[i]);
				}
			}
		}
	}
}
