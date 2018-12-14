using Game.Config;

namespace Game.Shared
{
    public class HostPlayer
    {
        public static PlayerData data = new PlayerData();
    }

    public class Time
    {
        public static double timestamp = 0;
        public static int rtt = 2;
        public static int last_request_rtt = 0;
    }
}
