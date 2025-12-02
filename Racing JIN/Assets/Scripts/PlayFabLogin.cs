using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour
{
    [Header("Registration references")]
    [SerializeField] private TMP_InputField registerName;
    [SerializeField] private TMP_InputField registerEmail;
    [SerializeField] private TMP_InputField registerPassword;
    [SerializeField] private TMP_Text RegistrationStatu;
    
    [Header("Login references")]
    [SerializeField] private TMP_InputField loginEmail;
    [SerializeField] private TMP_InputField loginPassword;
    [SerializeField] private TMP_Text loginStatu;
    
    
    
    private string _customId;
    
    public void Start()
    {
        _customId = SystemInfo.deviceUniqueIdentifier;
        
        /*
        Please change the titleId below to your own titleId from PlayFab Game Manager.
        If you have already set the value in the Editor Extensions, this can be skipped.
        */
        PlayFabSettings.staticSettings.TitleId = "496C4";
        PlayFabSettings.staticSettings.DeveloperSecretKey = "ZR1C7K3ZZHJ4QYRZCRPYR68CHKFZGQEZTHCCUE88F3PYUEZ8XW";
        PlayFabSettings.staticSettings.ProductionEnvironmentUrl = "https://496C4.playfabapi.com";
        
        // Auto login
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = false
        }, OnLoginSuccess, OnAutoLoginFailed);
        
    }

    // If not autologin then Anonymous login
    private void OnAutoLoginFailed(PlayFabError error)
    {
        var request = new LoginWithCustomIDRequest { CustomId = _customId, CreateAccount = true};
        PlayFabClientAPI.LoginWithCustomID(request, OnAnonymousLoginSuccess, OnError);
    }

    // --- Anonymous Login Callbacks -------
    private void OnAnonymousLoginSuccess(LoginResult result)
    {
        Debug.Log("Anonymous Login Success !");
    }
    
    private void OnError(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
    
     //  ----------- Registration and Linking -------------- 
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Registration Success !");
        RegistrationStatu.text = "Registration Successful !";
        
        PlayFabClientAPI.LinkCustomID(new LinkCustomIDRequest {
            CustomId = _customId,
            ForceLink = true
        }, OnLinked, OnError);
    }

    private void OnLinked(LinkCustomIDResult result)
    {
        Debug.Log("Link Successful !");
    }
    
    public void OnRegisterValidation()
    {
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest
        {
            
            Email = registerEmail.text,
            Password = registerPassword.text,
            Username = registerName.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }
    
    // ----- Login And Linking ----------
    public void OnLoginValidation()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = loginEmail.text,
            Password = loginPassword.text,
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }
    
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login Success !");
        RegistrationStatu.text = "Login Successful !";
        
        PlayFabClientAPI.LinkCustomID(new LinkCustomIDRequest {
            CustomId = _customId,
            ForceLink = true
        }, OnLinked, OnError);
    }
    
}
