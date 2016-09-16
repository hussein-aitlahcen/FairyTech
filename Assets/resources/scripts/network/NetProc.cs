using UnityEngine;
using System.Collections;

public class NetProc : MonoBehaviour
{

    private NetworkClient m_client;

	// Use this for initialization
	void Start () {
	    m_client = new NetworkClient();
	    m_client.Start();
	}

    public void Login(string userName, string password)
    {
        m_client.Login(userName, password);
    }

	// Update is called once per frame
	void Update () {
	    
	}
}
