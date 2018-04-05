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
    [Activity(Label = "fbl", MainLauncher = true)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "fbLogin",
        DataHost = "tranquillum.eu.auth0.com",
        DataPathPrefix = "/android/fbLogin/callback")]
    public class MainActivity : Activity
    {
        private Auth0Client client;
        private Button loginButton;
        private TextView userDetailsTextView;
        private AuthorizeState authorizeState;
        ProgressDialog progress;

        static OAuth2Authenticator auth;
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
                Domain = Resources.GetString(Resource.String.auth0_domain),
                ClientId = Resources.GetString(Resource.String.auth0_client_id),
                Activity = this
            });


        }

        

        private void Fb_Click(object sender, EventArgs e)
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

