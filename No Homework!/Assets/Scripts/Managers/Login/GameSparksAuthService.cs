using UnityEngine;

public enum Authtypes
{
    None,
    UsernameAndPassword,
    RegisterAccount,
}

public class GameSparksAuthService  {

    //Events to subscribe to for this service
    public delegate void DisplayAuthenticationEvent();
    public static event DisplayAuthenticationEvent OnDisplayAuthentication;

    public delegate void LoginSuccessEvent();
    public static event LoginSuccessEvent OnLoginSuccess;

    //These are fields that we set when we are using the service.
    public string Email;
    public string Username;
    public string Password;

    //Store authentication type and remember me in playerprefs
    private const string _GameSparksAuthTypeKey = "GameSparksAuthType";
    private const string _LoginRememberKey = "GameSparksLoginRemember";

    public bool isAuthenticated = false;

    public static GameSparksAuthService Instance {
        get
        {
            if(_instance == null)
            {
                _instance = new GameSparksAuthService();
            }
            return _instance;
        }
    }

    private static GameSparksAuthService _instance;
    public GameSparksAuthService()
    {
        _instance = this;
    }


    /// <summary>
    /// Remember the user next time they log in
    /// This is used for Auto-Login purpose.
    /// </summary>
    public bool RememberMe {
        get {
            return PlayerPrefs.GetInt(_LoginRememberKey, 0) == 0 ? false : true;
        }
        set {
            PlayerPrefs.SetInt(_LoginRememberKey, value ? 1 : 0);
        }
    }  
    
    /// <summary>
    /// Remember the type of authenticate for the user
    /// </summary>
    public Authtypes AuthType {
        get {
            return (Authtypes)PlayerPrefs.GetInt(_GameSparksAuthTypeKey, 0);
        }
        set {

            PlayerPrefs.SetInt(_GameSparksAuthTypeKey, (int) value);
        }
    }

    public void ClearRememberMe()
    {
        PlayerPrefs.DeleteKey(_LoginRememberKey);
    }

    /// <summary>
    /// Kick off the authentication process by specific authtype.
    /// </summary>
    /// <param name="authType"></param>
    public void Authenticate(Authtypes authType)
    {
        AuthType = authType;
        Authenticate();
    }

    /// <summary>
    /// Authenticate the user by the Auth Type that was defined.
    /// Called when the game starts
    /// </summary>
    public void Authenticate()
    {
        var authType = AuthType;
        //Debug.Log(authType); //Debugging
        switch (authType)
        {
            case Authtypes.None:
                if (OnDisplayAuthentication != null)
                {
                    OnDisplayAuthentication.Invoke();
                }
                break;

            case Authtypes.UsernameAndPassword:
                AuthenticateEmailPassword();
                break;
            case Authtypes.RegisterAccount:
                AddAccountAndPassword();
                break;
        }
    }


    /// <summary>
    /// Authenticate a user in PlayFab using an Email & Password combo
    /// </summary>
    private void AuthenticateEmailPassword()
    {
        //Check if the users has opted to be remembered.
        if (RememberMe && isAuthenticated == true)
        {
            new GameSparks.Api.Requests.AccountDetailsRequest()
            .Send((response) =>
            {
                if (response.HasErrors)
                {
                    Debug.LogError(response.Errors);
                    return;
                }

                AccountInfo.UpdateAccountInfo();

                if (OnLoginSuccess != null)
                {
                    //report login result back to subscriber
                    OnLoginSuccess.Invoke();
                }
            });

            return;
        }

        //a good catch: If username & password is empty, then do not continue, and Call back to Authentication UI Display 
        if (!RememberMe && (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Username)))
        {
            OnDisplayAuthentication.Invoke();
            return;
        }


        //We have not opted for remember me in a previous session, so now we have to login the user with email & password.
        new GameSparks.Api.Requests.AuthenticationRequest()
        .SetPassword(Password)
        .SetUserName(Username)
        .Send((response) => {
            if (!response.HasErrors)
            {
                if (OnLoginSuccess != null)
                {
                    //report login result back to subscriber
                    OnLoginSuccess.Invoke();
					
					Debug.Log("Player Authenticated");
					if (RememberMe)
						AuthType = Authtypes.UsernameAndPassword;

					AccountInfo.UpdateAccountInfo();
                }
            }
            else
            {
                Debug.Log("Error Authenticating Player");
            }
        });
    }

    /// <summary>
    /// Register a user with an Email & Password
    /// Note: We are not using the RegisterPlayFab API
    /// </summary>
    private void AddAccountAndPassword()
    {
        new GameSparks.Api.Requests.RegistrationRequest()
            .SetDisplayName(Username)
            .SetPassword(Password)
            .SetUserName(Username)
        .Send((response) => {
            if (response.HasErrors)
            {
                Debug.LogError(response.Errors);
                return;
            }
            else
            {
                Debug.Log("Successfully registred: " + response.UserId);
                OnDisplayAuthentication.Invoke();
            }
        });
    }
}
