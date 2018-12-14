using UnityEngine;

namespace Game.Config
{
    public class PathInfo
    {
        private static string _url_head
        {
            get
            {
                string ret = "";
                if (Application.platform == RuntimePlatform.Android)
                {
                    ret = "";
                }
                if (Application.platform == RuntimePlatform.WindowsEditor ||
                    Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    ret = "file:///";
                }
                return ret;
            }
        }

        public static readonly string STREAM_DIR = Application.streamingAssetsPath + "/";
        public static readonly string STREAM_URL = _url_head + STREAM_DIR;

        private static readonly string BUNDLES_R_URL =
        STREAM_URL + "bundles/";
        private static string _bundle_url = BUNDLES_R_URL;
        public static string BUNDLE_URL { get { return _bundle_url; } }
    }
}