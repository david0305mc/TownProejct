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
    //public FG_FirebaseAuth auth;
    // Start is called before the first frame upda
    void Start()
    {
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
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
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
                gameObject.SetActive(true);
            }
            else if (task.IsFaulted)
            {
                Debug.Log("google account is faulted");
                gameObject.SetActive(true);
            }
            else
            {
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

                    Firebase.Auth.FirebaseUser newUser = task.Result;
                    Debug.LogFormat("User signed in successfully: {0} ({1})",
                        newUser.DisplayName, newUser.UserId);
                });
            }
        });
    }
}
