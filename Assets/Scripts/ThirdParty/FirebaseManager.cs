using Google;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    [SerializeField] private Text text;

    public Firebase.FirebaseApp app { get; private set; }
    public Firebase.Auth.FirebaseAuth auth { get; private set; }
    public FirebaseUser user { get; private set; }
    public bool IsGoogleSignIn { get; private set; } = false;
    //public FG_FirebaseAuth auth;
    // Start is called before the first frame upda
    void Start()
    {
        //CheckFirebaseDependencies();
    }

    private void TestGoogleSignIn()
    {
        if (GoogleSignIn.Configuration == null)
        {
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                // Copy this value from the google-service.json file.
                // oauth_client with type == 3
                WebClientId = "35089722117-ai64it5nl0h144rba0imsgdt57l5vl8r.apps.googleusercontent.com"
            };
        }
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        var signIn = GoogleSignIn.DefaultInstance.SignIn();
        signIn.ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("google account is canceled");
            }
            else if (task.IsFaulted)
            {
                Debug.Log("google account is faulted");
            }
            else
            {
                Debug.Log("google account is success");
            }
        });
    }
    private void TestGoogleSignOut()
    {
        GoogleSignIn.DefaultInstance.SignOut();
    }
    private void CheckFirebaseDependencies()
    {
        Debug.Log("CheckFirebaseDependencies");
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;
                InitializeFirebase();
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    void InitializeFirebase()
    {
        Debug.Log("InitializeFirebase");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged -= AuthStateChanged;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        if (user != null)
        {
            Debug.Log("Try Auto Login");
            user.TokenAsync(true).ContinueWithOnMainThread(item => {
                UnityEngine.Debug.Log("Auto Signed in " + user.UserId);
            });
        }
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        Debug.Log("AuthStateChanged");
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                UnityEngine.Debug.Log("Signed out " + user.UserId);
                
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                UnityEngine.Debug.Log("Signed in " + user.UserId);
                text.text = $"Login {user.DisplayName ?? ""} {user.DisplayName ?? ""}";
            }
        }
    }

    public void OnClickBtnLogOut()
    {
        if(auth != null)
            auth.SignOut();

        if (IsGoogleSignIn)
        {
            GoogleSignIn.DefaultInstance.SignOut();
            IsGoogleSignIn = false;
        }
        PlayerPrefs.DeleteAll();
        CheckFirebaseDependencies();
    }

    public void OnClickBtnGoogleLogin()
    {
        SignInGoogle();
    }

    public void SignInGoogle()
    {
        if (GoogleSignIn.Configuration == null)
        {
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                // Copy this value from the google-service.json file.
                // oauth_client with type == 3
                WebClientId = "35089722117-ai64it5nl0h144rba0imsgdt57l5vl8r.apps.googleusercontent.com"
            };
        }
        
        var signIn = GoogleSignIn.DefaultInstance.SignIn();
        signIn.ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("google account is canceled");
            }
            else if (task.IsFaulted)
            {
                Debug.Log("google account is faulted");
            }
            else
            {
                IsGoogleSignIn = true;
                Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(signIn.Result.IdToken, signIn.Result.AuthCode);
                auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("SignInWithCredentialAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                        return;
                    }

                    user = task.Result;
                    Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
                });
            }
        });
    }
}
