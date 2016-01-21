using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OrientationFeed
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InstagramFeed : Page
    {
        public InstagramFeed()
        {
            this.InitializeComponent();
            BackgroundTb.Text = "#"+ hashvalue;
        }

        Uri targetUri2 = new Uri(@"https://api.instagram.com/oauth/authorize/?client_id=" + client_id + "&redirect_uri=https://api.instagram.com&response_type=code");

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            Uri targetUri = new Uri(@"https://instagram.com/accounts/logout/");
            ProgressBar.IsIndeterminate = true;
            ProgressBar.Visibility = Visibility.Visible;
            //AuthBrowser.Navigate(targetUri);
            AuthBrowser.Navigate(targetUri2);
        }

        private string access_token;
        string hashvalue = "ImagineHack2014";
        private async void AuthBrowser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs e)
        {
            //access token is a Url fragment and these fragments start with '#'

            //string uri = e.ToString();
            MessageDialog messageDialog1;
            MessageDialog messageDialog2;

            try 
            { 
            //MessageDialog messageDialog2 = new MessageDialog(uri);
            //await messageDialog2.ShowAsync();
                if (e.Uri.AbsoluteUri.Contains("?code"))
                {

                    string uri = e.Uri.AbsoluteUri.ToString();

                    string code = uri.Split('?').Last().Replace("code=", string.Empty);
                    NameValueCollection parameters = new NameValueCollection()
                    {
                        client_id = "",
                        client_secret = "",
                        grant_type = "authorization_code",
                        redirect_uri = "https://api.instagram.com",
                        code = code,
                    };

                    //string consumerKey = System.Net.WebUtility.UrlEncode("pRESOJWfLJ40fumYxGq9g");     //Put your own consumer key here.
                    //string consumerSecret = System.Net.WebUtility.UrlEncode("8Bblw0EQ6RAvcHW2B9w4lD7BkpvUZGuhWLd5A9Eo");   // Put your own consumer secret here.
                    //byte[] tokenBytes = System.Text.Encoding.UTF8.GetBytes(consumerKey + ":" + consumerSecret);
                    //string tokenCredentials = System.Convert.ToBase64String(tokenBytes);



                    //HttpClient client = new HttpClient();
                    //var result = client.GetStringAsync("https://api.instagram.com/oauth/access_token", parameters);

                    //var response = System.Text.Encoding.Default.GetString(result);

                    //URL link to POST the "code" and retrieve access token
                    //GetPictures(parameters);
                    Uri authUri = new Uri("https://api.instagram.com/oauth/access_token");

                    var client = new HttpClient();

                    //Add Post Method
                    var authContent = new HttpRequestMessage(HttpMethod.Post, authUri);

                    //Add App Parameters to content
                    var values = new List<KeyValuePair<string, string>>();

                    values.Add(new KeyValuePair<string, string>("client_id", parameters.client_id));
                    values.Add(new KeyValuePair<string, string>("client_secret", parameters.client_secret));
                    values.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
                    values.Add(new KeyValuePair<string, string>("redirect_uri", parameters.redirect_uri));
                    values.Add(new KeyValuePair<string, string>("code", parameters.code));
                    authContent.Content = new FormUrlEncodedContent(values);
                    //authContent.Headers.Add("Authorization", "Basic " + tokenCredentials);

                    //Get Response from URL with parameters
                    HttpResponseMessage authResponse = await client.SendAsync(authContent);

                    //Store response into string
                    string authResponseString = await authResponse.Content.ReadAsStringAsync();

                    AuthBrowser.Visibility = Visibility.Collapsed;
                    //Deserialize responseString into JsonObject
                    JObject authJsonObject = await JsonConvert.DeserializeObjectAsync<JObject>(authResponseString);
                    if (access_token == null)
                    {
                        //Retrieve access token
                        access_token = authJsonObject["access_token"].ToString();
                        AuthBrowser.Width = 0;
                        AuthBrowser.Visibility = Visibility.Collapsed;

                        ProgressBar.IsIndeterminate = true;
                        ProgressBar.Visibility = Visibility.Visible;

                    }

                    //var client = new HttpClient();
                    string hashUri = "https://api.instagram.com/v1/tags/" + hashvalue +"/media/recent?access_token=" + access_token;
                    Uri getUri = new Uri(hashUri);  //%23blabla means we get the tweets with #blabla hashtag, replace with anything you like
                    var getContent = new HttpRequestMessage(HttpMethod.Get, getUri);
                    //getContent.Headers.Add("Authorization", "Bearer " + access_token);
                    HttpResponseMessage getResponse = await client.SendAsync(getContent);
                    // .....
                    string getResponseString = await getResponse.Content.ReadAsStringAsync();


                    JObject getJsonObject = await JsonConvert.DeserializeObjectAsync<JObject>(getResponseString);
                    var instaObjects = getJsonObject["data"].ToList();
                    List<Instapost> instaList = new List<Instapost>();
                    foreach (JObject insta in instaObjects)
                    {
                        Instapost d = new Instapost();
                        //d.comments.count =
                        d.Username = insta["user"]["username"].ToString();
                        d.FullName = insta["user"]["full_name"].ToString();
                        d.ImageUrl = insta["images"]["standard_resolution"]["url"].ToString();
                        d.ProfilePicture = insta["user"]["profile_picture"].ToString();
                        if (insta["caption"].Children().Any())
                            d.Message = insta["caption"]["text"].ToString();
                        if (insta["likes"] != null)
                            d.LoveCount = insta["likes"]["count"].ToString();
                        instaList.Add(d);

                    }
                    InstaListView.ItemsSource = instaList;
                    AuthBrowser.Width = 0;
                    AuthBrowser.Visibility = Visibility.Collapsed;
                    ProgressBar.Visibility = Visibility.Collapsed;
                    ProgressBar.IsIndeterminate = false;
                }
            }
            catch(Exception error)
            {
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
                //RefreshButton.IsEnabled = true;
                messageDialog2 = new MessageDialog("Could not get tweets: " + error.Message);
                messageDialog2.ShowAsync();
            }
            
        }

        private void Image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

            if (ValueStack.Width == 200)
            {
                TextIn.Stop();
                TextOut.Stop();
                TextOut.Begin();
                arrow.Source = new BitmapImage(new Uri("ms-appx:///Assets/arrow-right3.png"));
                
            }
            else
            {
                TextIn.Stop();
                TextOut.Stop();
                TextIn.Begin();                
                arrow.Source = new BitmapImage(new Uri("ms-appx:///Assets/left-arrow.png"));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(hashval.Text != " " || hashval.Text != null)
            {
                hashvalue = hashval.Text;
                Title.Text = "#" + hashval.Text;
                BackgroundTb.Text = "#" + hashval.Text;
                ProgressBar.IsIndeterminate = true;
                ProgressBar.Visibility = Visibility.Visible;
                AuthBrowser.Navigate(targetUri2);
            }
        }
        
        /*
        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void InstaListView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
        */

    }
}
