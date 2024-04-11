using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;
using Firebase;
using TMPro;
using Firebase.Database;


public class logInUI : MonoBehaviour
{
    [Header ("Window Controllers")] 
    public RectTransform SignUpSignInWindow;
    public RectTransform SignInWindow;
    public RectTransform SignUpWindow;
    [Header ("Sign In Inputs")]
    public InputField emailSignIn;
    public InputField passwordSignIn;
    [Header ("Sign Up Inputs")]
    public InputField emailCreateUser;
    public InputField passwordCreateUser;
    public Button BackToSignIn;
    [Header ("Sign In / Out Buttons")]
    public Button signInBtn;
    public Button signUpBtn;
    public Button signOutBtn;
    public Button CreateNewUserBtn;
    [Header ("OtherScripts")]
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private PlayerController playerController;
    [Header ("Error Messages")]
    public TextMeshProUGUI errorLogIn;
    public TextMeshProUGUI errorSignUp;
    public TextMeshProUGUI SignedInUser;
    [Header ("Other Elements")]
    public Button BackBtn;
    public RectTransform successfulSignIn;
    public Button SuccessfulSignInBtn;
    public Button PasswordResetBtn;
    [Header("Firebase References")]
    FirebaseAuth auth;
    FirebaseUser user;
    DatabaseReference DBReference;

    private void Awake(){
        //? Firebase Initialization
        InitializeFirebase();
        checkSignIn();
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

    void Update()
    {
        checkSignIn();
    }

    //* Handle initialization of the necessary firebase modules:
    void InitializeFirebase() {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        DBReference = FirebaseDatabase.DefaultInstance.RootReference;
        if (auth.CurrentUser != null) {
            user = auth.CurrentUser;
            readWriteUserDetails();
            Debug.Log("Signed in " + user.UserId);
        } else {
            Debug.Log("Not signed in");
        }
        AuthStateChanged(this, null);
    }

    private void readWriteUserDetails(){
        UserDetails userDetails = new UserDetails(user.Email, playerController.getHighScore());
        string json = JsonUtility.ToJson(userDetails);
        string userId = auth.CurrentUser.UserId;
        DBReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    private void CheckHighscorePerUser(){
        
    }

    //? Handle different button clicks
    private void HandleSignInButton(){SignIn(emailSignIn.text, passwordSignIn.text);}
    private void HandleSignUpButton(){CreateUser(emailCreateUser.text, passwordCreateUser.text);}
    private void HandleCreateUserBtn(){
        SignInWindow.gameObject.SetActive(false);
        SignUpWindow.gameObject.SetActive(true);
        BackBtn.gameObject.SetActive(false);
    }
    private void HandleBackToSignIn(){
        SignInWindow.gameObject.SetActive(true);
        SignUpWindow.gameObject.SetActive(false);
        BackBtn.gameObject.SetActive(true);
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
            SignedInUser.text = "Signed in as " + user.Email;
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
            SignedInUser.text = "Not signed in";
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

    void checkSignIn(){
        //? Check if the user is signed in
        if (user != null)
        {
            SignedInUser.text = "Signed in as " + user.Email;
        }
        else
        {
            SignedInUser.text = "Not signed in";
        }
    }
}

public class UserDetails{
    public string email;
    public int highScore;
    public UserDetails(string email, int highScore){
        this.email = email;
        this.highScore = highScore;
    }
}