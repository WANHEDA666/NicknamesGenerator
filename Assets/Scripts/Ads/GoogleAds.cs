using GoogleMobileAds.Api;

namespace Ads
{
    public class GoogleAds
    {
        private BannerView bannerView;
        
        public GoogleAds()
        {
            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(initStatus => { });
#if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3889811573373736/2019820533";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3889811573373736/1808538949";
#else
            string adUnitId = "unexpected_platform";
#endif

            // Create a 320x50 banner at the top of the screen.
            this.bannerView = new BannerView(adUnitId, AdSize.Leaderboard, AdPosition.Bottom);

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();

            // Load the banner with the request.
            this.bannerView.LoadAd(request);
        }
    }
}