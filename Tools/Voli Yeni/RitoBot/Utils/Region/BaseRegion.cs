namespace RitoBot.Utils.Region
{
    using LoLLauncher;
    using System;
    using System.Net;

    public abstract class BaseRegion
    {
        protected BaseRegion()
        {
        }

        public static BaseRegion GetRegion(string requestedRegion)
        {
            requestedRegion = requestedRegion.ToUpper();
            Type type = Type.GetType("RitoBot.Utils.Region." + requestedRegion);
            if (type != null)
            {
                return (BaseRegion) Activator.CreateInstance(type);
            }
            type = Type.GetType("RitoBot.Utils.Region.Garena." + requestedRegion);
            if (type != null)
            {
                return (BaseRegion) Activator.CreateInstance(type);
            }
            return null;
        }

        public abstract string ChatName { get; }

        public abstract bool Garena { get; }

        public abstract string InternalName { get; }

        public abstract string Locale { get; }

        public abstract string Location { get; }

        public abstract Uri NewsAddress { get; }

        public abstract IPAddress[] PingAddresses { get; }

        public abstract Region PVPRegion { get; }

        public abstract string RegionName { get; }

        public abstract string SpectatorIpAddress { get; set; }

        public abstract Uri SpectatorLink { get; }
    }
}

