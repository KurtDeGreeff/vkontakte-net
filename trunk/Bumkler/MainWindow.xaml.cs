using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using mshtml;
using Vkontakte;
using Vkontakte.Activity;
using Vkontakte.MethodResults;

namespace Bumkler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int userId = 1; // Replace with your ID
        private int appId = 1; // Replace with your app id
        private int settingsMask = 255;

        private Uri url;
        private SessionData sessionData;
        private bool authenticated = false;
        private IVkAdapter adapter;

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

        private void GetActivity()
        {
            if(!authenticated || adapter == null)
            {
                MessageBox.Show("Not authenticated!");
                return;
            }
            
            var activity = new Activity(adapter);

            var res = activity.Get();
            if (res is ActivityResult)
            {
                MessageBox.Show((res as ActivityResult).Activity);
            }
            else if (res is ErrorResult)
            {
                MessageBox.Show((res as ErrorResult).ErrorCode.ToString() + "\n" + (res as ErrorResult).ErrorMessage);
            }
        }

        SessionData reRequestSessionData(string cookiesString)
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
                MessageBox.Show("Authenticatin succeded.");
                
                sessionData = Utils.ParseLogin(e.Uri) ?? reRequestSessionData(((sender as WebBrowser).Document as IHTMLDocument2).cookie);

                adapter = new VkAdapter(sessionData, userId, appId);
                authenticated = true;
               
            }

            else if (e.Uri.ToString().Contains("http://vkontakte.ru/api/login_failure.html"))
            {
                MessageBox.Show("Authenticatin failed!");
                authenticated = false;
               
            }
            webBrowser.Visibility = System.Windows.Visibility.Hidden;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            GetActivity();
        }
    }
}
