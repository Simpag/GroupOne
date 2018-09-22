using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

public class LoginManager : MonoBehaviour
{
    //Debug Flag to simulate a reset
    public bool ClearPlayerPrefs;

    //Meta fields for objects in the UI
    [Header("Authentication")]
    [SerializeField]
    private GameObject LoginPanel;
    [SerializeField]
    private InputField usernameInputField;
    [SerializeField]
    private InputField passwordInputField;
    [SerializeField]
    private Button LoginButton;
    [SerializeField]
    private Button registerPanelButton;
    [SerializeField]
    private Toggle RememberMe;

    [Header("Registration")]
    [SerializeField]
    private GameObject RegisterPanel;
    [SerializeField]
    private InputField registerUsernameInputField;
    [SerializeField]
    private InputField registerEmailInputField;
    [SerializeField]
    private InputField registerPasswordInputField;
    [SerializeField]
    private InputField registerConfirmPasswordInputField;   
    [SerializeField]
    private Button RegisterButton;
    [SerializeField]
    private Button CancelRegisterButton;

    //Reference to our Authentication service
    private GameSparksAuthService _AuthService = GameSparksAuthService.Instance;

    public void Awake()
    {
        //If you want to clear playerprefs to re-login
        if (ClearPlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
            _AuthService.ClearRememberMe();
            _AuthService.AuthType = Authtypes.None;
        }

        //Set our remember me button to our remembered state.
        RememberMe.isOn = _AuthService.RememberMe;

        //Subscribe to our Remember Me toggle
        RememberMe.onValueChanged.AddListener((toggle) =>
        {
            _AuthService.RememberMe = toggle;
        });
    }

    public void Start()
    {
        //Prevent random bugs
        if (_AuthService.AuthType == Authtypes.RegisterAccount)
            _AuthService.AuthType = Authtypes.None;

        //Hide all our panels until we know what UI to display
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(false);

        //Subscribe to events that happen after we authenticate
        GameSparksAuthService.OnDisplayAuthentication += OnDisplayAuthentication;
        GameSparksAuthService.OnLoginSuccess += OnLoginSuccess;

        //This will trigger the authentication process
        GameSparks.Core.GS.Instance.GameSparksAuthenticated += RememberMeAuth;
        GameSparks.Core.GS.Instance.GameSparksAvailable += StartAuth;

        //Bind to UI buttons to perform actions when user interacts with the UI.
        LoginButton.onClick.AddListener(OnLoginButtonClicked);
        registerPanelButton.onClick.AddListener(OnRegisterPanelButtonClicked);
        RegisterButton.onClick.AddListener(OnRegisterButtonClicked);
        CancelRegisterButton.onClick.AddListener(OnCancelRegisterButtonClicked);
    }

    private void RememberMeAuth(string _auth)
    {
        if (!_AuthService.RememberMe)
        {
            //Debug.Log("DON'T REMEMBER ME!"); //Debugging
            GameSparks.Core.GS.GameSparksAuthenticated = null;
            return;
        }

        //Debug.Log("Start RememberMe Auth Process!"); //Debugging
        if (!string.IsNullOrEmpty(_auth))
        {
            _AuthService.isAuthenticated = true;
            
            //Start the authentication process.
            _AuthService.Authenticate();
            GameSparks.Core.GS.Instance.GameSparksAvailable = null;
            GameSparks.Core.GS.GameSparksAuthenticated = null;
        }
    }

    private void StartAuth(bool _isAvailable)
    {
        if (_isAvailable && !_AuthService.RememberMe)
        {
            Debug.Log("Start Auth Process!");
            _AuthService.Authenticate();
            GameSparks.Core.GS.Instance.GameSparksAvailable = null;
            GameSparks.Core.GS.GameSparksAuthenticated = null;
        }
        else if (!_isAvailable)
        {
            Debug.Log("Can't authenticate!");
        }
    }

    /// <summary>
    /// Login Successfully - Goes to next screen.
    /// </summary>
    private void OnLoginSuccess()
    {
        DatabaseManager.UpdateDatabase();

        //Show our next screen if we logged in successfully.
        SceneManager.LoadScene(GameConstants.MAIN_MENU_SCENE);
    }

    /// <summary>
    /// Choose to display the Auth UI or any other action.
    /// </summary>
    private void OnDisplayAuthentication()
    {
        //Here we have choses what to do when AuthType is None.
        RegisterPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    /// <summary>
    /// Login Button means they've selected to submit a username (email) / password combo
    /// </summary>
    private void OnLoginButtonClicked()
    {
        //ProgressBar.UpdateLabel(string.Format("Logging In As {0} ...", emailInputField.text));
        //ProgressBar.UpdateProgress(0f);
        //ProgressBar.AnimateProgress(0, 1, () => {
        //    //second loop
        //    ProgressBar.UpdateProgress(0f);
        //});

        _AuthService.Email = usernameInputField.text;
        _AuthService.Username = usernameInputField.text;
        _AuthService.Password = passwordInputField.text;
        _AuthService.Authenticate(Authtypes.UsernameAndPassword);
    }

    /// <summary>
    /// Register Panel Button will open the registration panel
    /// </summary>
    private void OnRegisterPanelButtonClicked()
    {
        //ProgressBar.UpdateLabel(string.Empty);

        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }

    /// <summary>
    /// Register user with username and password
    /// </summary>
    private void OnRegisterButtonClicked()
    {
        if (registerPasswordInputField.text != registerConfirmPasswordInputField.text)
        {
            //ProgressBar.UpdateLabel("Passwords do not Match.");
            Debug.Log("Passwords do not match!");
            return;
        }

        //ProgressBar.UpdateLabel(string.Format("Registering User {0} ...", registerEmailInputField.text));
        //ProgressBar.UpdateProgress(0f);
        //ProgressBar.AnimateProgress(0, 1, () => { ProgressBar.UpdateProgress(0f); });

        _AuthService.Email = registerEmailInputField.text;
        _AuthService.Password = registerPasswordInputField.text;
        _AuthService.Username = registerUsernameInputField.text;
        _AuthService.Authenticate(Authtypes.RegisterAccount);
    }

    /// <summary>
    /// They have opted to cancel the Registration process.
    /// Possibly they typed the email address incorrectly.
    /// </summary>
    private void OnCancelRegisterButtonClicked()
    {
        //Reset all forms
        registerEmailInputField.text = string.Empty;
        registerUsernameInputField.text = string.Empty;
        registerPasswordInputField.text = string.Empty;
        registerConfirmPasswordInputField.text = string.Empty;

        //ProgressBar.UpdateLabel(string.Empty);

        //Show authentication panel
        OnDisplayAuthentication();
    }

}
