using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace azvmctrl
{
    [Activity(Label = "azvmctrl", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        public const string resource = "00000002-0000-0000-c000-000000000000";
        public const string clientId = "b78054de-7478-45a6-be1c-09f696a91d64";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
            button.Click += delegate { doAuth(button); };
        }

        public void doAuth(Button button)
        {
            AuthenticationContext ctx = new AuthenticationContext("https://login.microsoftonline.com/common");
            doAuth2(ctx, button);
        }

        public AuthenticationResult doAuth2(AuthenticationContext ctx, Button button)
        {
            AuthenticationResult result = null;
            try
            {
                DeviceCodeResult codeResult = ctx.AcquireDeviceCodeAsync(resource, clientId).Result;
                Console.ResetColor();
                Console.WriteLine("You need to sign in.");
                Console.WriteLine("Message: " + codeResult.Message + "\n");

                button.Text = string.Format("Message: " + codeResult.Message + "\n", count++);

                result = ctx.AcquireTokenByDeviceCodeAsync(codeResult).Result;
            }
            catch (Exception exc)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong.");
                Console.WriteLine("Message: " + exc.Message + "\n");
            }
            return result;
        }
    }
}

