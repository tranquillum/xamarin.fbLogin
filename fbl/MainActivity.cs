using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Xamarin.Auth;
using Newtonsoft.Json.Linq;
using System.Json;


namespace fbl
{
    [Activity(Label = "fbl", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button fb;
        TextView fbname;
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
            //
            fbname = FindViewById<TextView>(Resource.Id.name);
            fb = FindViewById<Button>(Resource.Id.fbin);
            fb.Click += Fb_Click;
        }

        

        private void Fb_Click(object sender, EventArgs e)
        {
            var auth = new OAuth2Authenticator(
                 clientId: "1194275697381722",
                 clientSecret: "fa0fcad73e81fb8b7fa3f7903c17f276",
                 scope: "",
                 authorizeUrl: new Uri("https://m.facebook.com/digitel/oauth/"),
                 redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html"),
                 accessTokenUrl: new Uri("https://graph.facebook.com/oauth/access_token"));

            
           

            auth.Completed += FBAuth_Completed;
            StartActivity(auth.GetUI(this));

            //PresentViewController(ui, true, null);

           

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
}

