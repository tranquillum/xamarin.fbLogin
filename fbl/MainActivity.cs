using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Text.Method;
using Android.Widget;
using Auth0.OidcClient;
using IdentityModel.OidcClient;
using System;
using System.Json;
using System.Text;
using Xamarin.Auth;

namespace fbl
{
    [Activity(Label = "fbl", MainLauncher = true,  LaunchMode = LaunchMode.SingleTask)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "fbl.fbl",
        DataHost = "tranquillum.eu.auth0.com",
        DataPathPrefix = "/android/fbl.fbl/callback")]
    public class MainActivity : Activity
    {
        private Auth0Client client;
        //private Button loginButton;
        //private TextView userDetailsTextView;
        private AuthorizeState authorizeState;
        //ProgressDialog progress;

        //static OAuth2Authenticator auth;
        private Button fb;
        private TextView fbname;
        protected override void OnCreate(Bundle savedInstanceState)
        {

           
            //FacebookSdk.SdkInitialize(getApplicationContext());
            //AppEventsLogger.ActivateApp(this);

            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ///////////////COde to get hash key for devi	
            //PackageInfo info= this.PackageManager.GetPackageInfo("Magic_8_Ball_KAE.Magic_8_Ball_KAE", PackageInfoFlags.Signatures);	
            //foreach (Android.Content.PM.Signature signature in info.Signatures)	
            //{	
            //    MessageDigest mb = MessageDigest.GetInstance("SHA");	
            //    mb.Update(signature.ToByteArray());	

            //    string keyhash = Convert.ToBase64String(mb.Digest());	
            //    Console.WriteLine("KeyHash", keyhash);	

            
            fbname = FindViewById<TextView>(Resource.Id.name);
            fb = FindViewById<Button>(Resource.Id.fbin);
            fb.Click += Fb_Click;

            client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = "tranquillum.eu.auth0.com",
                ClientId = "wjBZFDNJV2fHU76V03EuFT7cLIfES1Bi",
                Activity = this
            });


        }

        protected override void OnResume()
        {
            base.OnResume();

            //if (progress != null)
            //{
            //    progress.Dismiss();

            //    progress.Dispose();
            //    progress = null;
            //}
        }

        protected override async void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            var loginResult = await client.ProcessResponseAsync(intent.DataString, authorizeState);

            var sb = new StringBuilder();
            if (loginResult.IsError)
            {
                sb.AppendLine($"An error occurred during login: {loginResult.Error}");
            }
            else
            {
                sb.AppendLine($"ID Token: {loginResult.IdentityToken}");
                sb.AppendLine($"Access Token: {loginResult.AccessToken}");
                sb.AppendLine($"Refresh Token: {loginResult.RefreshToken}");

                sb.AppendLine();

                sb.AppendLine("-- Claims --");
                foreach (var claim in loginResult.User.Claims)
                {
                    sb.AppendLine($"{claim.Type} = {claim.Value}");
                }
            }

            //userDetailsTextView.Text = sb.ToString();
            fbname.Text = sb.ToString();
        }

        private async void Fb_Click(object sender, EventArgs e)
        {
            //auth = new OAuth2Authenticator(
            //     clientId: "1194275697381722",
            //     clientSecret: "fa0fcad73e81fb8b7fa3f7903c17f276",
            //     scope: "",
            //     authorizeUrl: new Uri("https://m.facebook.com/digitel/oauth/"),
            //     redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html"),
            //     accessTokenUrl: new Uri("https://graph.facebook.com/oauth/access_token"));

            //auth.Completed += FBAuth_Completed;
            //StartActivity(auth.GetUI(this));

            //progress = new ProgressDialog(this);
            //progress.SetTitle("Log In");
            //progress.SetMessage("Please wait while redirecting to login screen...");
            //progress.SetCancelable(false); // disable dismiss by tapping outside of the dialog
            //progress.Show();

            // Prepare for the login
            authorizeState = await client.PrepareLoginAsync();

            // Send the user off to the authorization endpoint
            var uri = Android.Net.Uri.Parse(authorizeState.StartUrl);
            var intent = new Intent(Intent.ActionView, uri);
            intent.AddFlags(ActivityFlags.NoHistory);
            StartActivity(intent);







        }

        private async void FBAuth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {

            

            if (e.IsAuthenticated)
                {
                     OAuth2Request request = new OAuth2Request(
                        "GET",
                        new Uri("https://graph.facebook.com/me?fields=name"),
                        null,
                        e.Account);

                    var fbResponse = await request.GetResponseAsync();

                    var fbUser = JsonValue.Parse(fbResponse.GetResponseText());

                    var name = fbUser["name"];
                    var id = fbUser["id"];

                         fbname.Text += name;
                   

                    //NameLabel.Text += name;
                    //IdLabel.Text += id;
                    //PictureImage.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(picture)));
                    //CoverImage.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(cover)));
                }

                //DismissViewController(true, null);

        }
    }

    //https://github.com/auth0-community/auth0-xamarin-oidc-samples/blob/master/Quickstart/01-Login/Android/AndroidSample/MainActivity.cs
}

