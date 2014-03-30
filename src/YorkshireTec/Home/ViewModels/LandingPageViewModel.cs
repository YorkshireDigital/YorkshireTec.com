namespace YorkshireTec.Home.ViewModels
{
    using System.Collections.Generic;
    using TweetSharp;

    public class LandingPageViewModel
    {
        public List<string> Tweets { get; set; }

        public LandingPageViewModel(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            Tweets = new List<string>();

            var twitterClientInfo = new TwitterClientInfo { ConsumerKey = consumerKey, ConsumerSecret = consumerSecret };
            var twitterService = new TwitterService(twitterClientInfo);

            twitterService.AuthenticateWith(accessToken, accessTokenSecret);

            var options = new ListTweetsOnUserTimelineOptions {Count = 3, ScreenName = "YorkshireTec"};

            var currentTweets = twitterService.ListTweetsOnUserTimeline(options);

            if (currentTweets != null)
            {
                foreach (var tweet in currentTweets)
                {
                    Tweets.Add(tweet.TextAsHtml);
                }
            }
        }
    }
}