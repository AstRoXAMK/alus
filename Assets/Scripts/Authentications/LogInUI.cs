using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;
using Firebase;
using TMPro;
using Unity.VisualScripting;


public class SignInUser : MonoBehaviour
{
    //? Control the windows
    public RectTransform SignUpSignInWindow;
    public RectTransform SignInWindow;
    public RectTransform SignUpWindow;
    
    //? Sign In inputs
    public InputField emailSignIn;
    public InputField passwordSignIn;
    
    //? Sign Up inputs
    public InputField emailCreateUser;
    public InputField passwordCreateUser;
    public Button BackToSignIn;
    
    //? Enter game buttons
    public Button signInBtn;
    public Button signUpBtn;
    public Button signOutBtn;

    [SerializeField] private MenuManager menuManager;
    
    //? Other UI elements
    public Button CreateNewUserBtn;
    public TextMeshProUGUI errorLogIn;
    public TextMeshProUGUI errorSignUp;
    public Button BackBtn;
    public RectTransform successfulSignIn;
    public Button SuccessfulSignInBtn;
    public Button PasswordResetBtn;
    //? Firebase variables
    FirebaseAuth auth;
    FirebaseUser user;

    private void Awake(){
        //? Firebase Initialization
        InitializeFirebase();
    }

    private void Start()
    { 
        //? Button functionality using Listeners 
        signInBtn.onClick.AddListener(HandleSignInButton);
        signUpBtn.onClick.AddListener(HandleSignUpButton);
        signOutBtn.onClick.AddListener(HandleSignOutButton);
        CreateNewUserBtn.onClick.AddListener(HandleCreateUserBtn);
        BackToSignIn.onClick.AddListener(HandleBackToSignIn);
        BackBtn.onClick.AddListener(HandleOnBackBtn);
        SuccessfulSignInBtn.onClick.AddListener(HandleOnBackBtn);
        PasswordResetBtn.onClick.AddListener(HandlePasswordResetBtn);
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase() {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    //? Handle different button clicks
    private void HandleSignInButton(){SignIn(emailSignIn.text, passwordSignIn.text);}
    private void HandleSignUpButton(){CreateUser(emailCreateUser.text, passwordCreateUser.text);}
    private void HandleCreateUserBtn(){
        SignInWindow.gameObject.SetActive(false);
        SignUpWindow.gameObject.SetActive(true);
    }
    private void HandleBackToSignIn(){
        SignInWindow.gameObject.SetActive(true);
        SignUpWindow.gameObject.SetActive(false);
    }

    private void HandleSignOutButton(){
        auth.SignOut();
        Debug.Log(user.Email + " signed out successfully!");
    }

    private void HandleOnBackBtn(){menuManager.LogInScreen.SetActive(false);}

    private void HandlePasswordResetBtn(){
        string emailReset = emailSignIn.text;
        if(!string.IsNullOrEmpty(emailReset)){
            auth.SendPasswordResetEmailAsync(emailReset).ContinueWith(task => {
                if (task.IsCanceled) {
                    Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                    return;
                }
                if (task.IsFaulted) {
                    Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                    return;
                }
                Debug.Log("Password reset email sent to " + emailReset);
            });
        }
    }

    //* Sign in user with an email and password
    public async void SignIn(string email, string password)
    {
        auth = FirebaseAuth.DefaultInstance;
        //* used to sign in a user
        Task<AuthResult> signInUserTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        try
        {
            await signInUserTask;
        }
        catch (FirebaseException fbe)
        {
            Debug.LogError(fbe.Message);
            errorLogIn.text = "Email or Password is incorrect";
            errorLogIn.gameObject.SetActive(true);
        }
        finally
        {
            AuthResult signInUserResult = signInUserTask.Result;
            user = signInUserResult.User;
            Debug.LogFormat("User: " + user.Email + " signed in successfully!");
            menuManager.LogInScreen.SetActive(false);
        }
    }

    //* Create a new user with an email and password
    public async void CreateUser(string email, string password)
    {
        auth = FirebaseAuth.DefaultInstance;
        //* used to create a new user
        Task<AuthResult> addUserTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

        try
        {
            await addUserTask;
        }
        catch (FirebaseException fbe)
        {
            Debug.LogError(fbe.Message);
            errorSignUp.text = fbe.Message;
            errorSignUp.gameObject.SetActive(true);
        }
        finally
        {
            AuthResult addUserResult = addUserTask.Result;
            user = addUserResult.User;
            Debug.LogFormat("User: " + user.Email + " created successfully!");
            successfulSignIn.gameObject.SetActive(true);
        }
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, EventArgs eventArgs) {
        if (auth.CurrentUser != user) {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn) {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    // Handle removing subscription and reference to the Auth instance.
    // Automatically called by a Monobehaviour after Destroy is called on it.
    void OnDestroy() {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }
}