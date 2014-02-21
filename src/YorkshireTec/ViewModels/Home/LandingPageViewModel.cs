namespace YorkshireTec.ViewModels.Home
{
    using System.Collections.Generic;
    using TweetSharp;

    public class LandingPageViewModel
    {
        public List<string> Tweets { get; set; }

        public LandingPageViewModel()
        {
            Tweets = new List<string>();

            const string consumerKey = "cXTGDjf0MBNQWB1IvcaEnA"; // nothing to see here
            const string consumerSecret = "0Wz3oBFO4D5dn0LGtM7V6ZuIjLFl6Ac1r0Pt7mKA"; // nothing to see here
            const string accessToken = "183268831-C2mwzwFbOe7OPkebbtQKHCJpQM1CnmEW4Ibp4pw3"; // nothing to see here
            const string accessTokenSecret = "mqBOrRRVkxhN7HswBxD4yQdnmQqntQdTNiM1A3T3yP5Kt"; // nothing to see here

            var twitterClientInfo = new TwitterClientInfo { ConsumerKey = consumerKey, ConsumerSecret = consumerSecret };
            var twitterService = new TwitterService(twitterClientInfo);

            twitterService.AuthenticateWith(accessToken, accessTokenSecret);

            var options = new ListTweetsOnUserTimelineOptions {Count = 3, ScreenName = "YorkshireTec"};

            var currentTweets = twitterService.ListTweetsOnUserTimeline(options);
            foreach (var tweet in currentTweets)
            {
                Tweets.Add(tweet.TextAsHtml);
            }
        }
    }
}