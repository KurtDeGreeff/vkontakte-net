using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using mshtml;
using Vkontakte;
using Vkontakte.Activity;
using Vkontakte.Constants;
using Vkontakte.Exceptions;
using Vkontakte.MethodResults;
using Vkontakte.Users;

namespace Bumkler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int userId = 59156; // Replace with your ID
        private int appId = 1892435; // Replace with your app id
        private int settingsMask = 255;

        private Uri url;
        private SessionData sessionData;
        private IVkAdapter adapter;

        public List<User> Friends;


        public MainWindow()
        {
            url = new Uri(String.Format("http://vkontakte.ru/login.php?app={0}&layout=popup&type=browser&settings={1}", appId, settingsMask));
            InitializeComponent();
            Authenticate();   
        }

        private void Authenticate()
        {
            webBrowser.Navigated += webBrowser_Navigated;
            webBrowser.Navigate(url);
        }

        private void GetSettings()
        {
            if (adapter == null || !adapter.Authenticated)
            {
                MessageBox.Show("Not authenticated!");
                return;
            }

           
            try
            {
                var res = Users.GetUserSettings();
                MessageBox.Show(res.ToString());
            }
            catch (RemoteMethodException ex)
            {
                MessageBox.Show(String.Format("Error code:{0}\n{1}", ex.ErrorCode, ex.ErrorMessage));
            }
            
            
        }

        

        private void GetActivity()
        {
            if (adapter == null || !adapter.Authenticated)
            {
                MessageBox.Show("Not authenticated!");
                return;
            }
            
           
            try
            {
                var res = Activity.Get();
                MessageBox.Show(res.Activity);
            }
            catch (RemoteMethodException ex)
            {
                MessageBox.Show(String.Format("Error code:{0}\n{1}", ex.ErrorCode, ex.ErrorMessage));
            }
        }


        private void GetFriends()
        {
            if (adapter == null || !adapter.Authenticated)
            {
                MessageBox.Show("Not authenticated!");
                return;
            }

            
            var friendsList = Users.GetFriends();
            var profiles = Users.GetProfiles(friendsList, new List<string>(){ProfileFields.PhotoUrl});
            listView1.ItemsSource = profiles;
            
        }

        SessionData ReRequestSessionData(string cookiesString)
        {
            var req = WebRequest.Create(url) as HttpWebRequest;
            var cookies = new CookieContainer();

            
            cookies.Add(new Cookie("remixchk", "5", "/", "vkontakte.ru"));
            var r = new Regex("remixsid=(.+);*?");
            var result = r.Match(cookiesString);
            
            var remixsid = result.Groups.Count > 0 ? result.Groups[1].Value.ToString() : "";
            
            cookies.Add(new Cookie("remixsid", remixsid, "", "vkontakte.ru"));

            req.CookieContainer = cookies;

            req.UseDefaultCredentials = true;

            var resp = req.GetResponse();
            SessionData s = Utils.ParseLogin(resp.ResponseUri);
            return s;
        }

       
        void webBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            
            
            if (e.Uri.ToString().Contains("http://vkontakte.ru/api/login_success.html"))
            {
                
                sessionData = Utils.ParseLogin(e.Uri) ?? ReRequestSessionData(((sender as WebBrowser).Document as IHTMLDocument2).cookie);

                adapter = VkAdapter.Instance;
                adapter.Authenticate(sessionData, userId, appId);
            }

            else if (e.Uri.ToString().Contains("http://vkontakte.ru/api/login_failure.html"))
            {
                MessageBox.Show("Authenticatin failed!");
            }
            //webBrowser.Visibility = System.Windows.Visibility.Hidden;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            GetActivity();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            GetSettings();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            GetFriends();
        }
    }
}
