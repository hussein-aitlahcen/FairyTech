using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    private Text m_accountInput;
    private Text m_passwordInput;
    private Button m_connectButton;
    private NetProc m_netProc;

	// Use this for initialization
	void Start ()
	{
	    m_netProc = GameObject.Find("NetProc").GetComponent<NetProc>();
	    m_accountInput = transform.FindChild("AccountInput").transform.FindChild("Text").GetComponent<Text>();
        m_passwordInput = transform.FindChild("PasswordInput").transform.FindChild("Text").GetComponent<Text>();
	    m_connectButton = transform.FindChild("ConnectButton").GetComponent<Button>();
	    m_connectButton.onClick.AddListener(() => 
	    {
	        m_netProc.Login(m_accountInput.text, m_passwordInput.text);
	    });
	}


	
	// Update is called once per frame
	void Update () {
	    
	}
}
