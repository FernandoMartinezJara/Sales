namespace Sales.Helpers
{
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string tokenType = "TokenType";
        private const string accessToken = "AccessToken";
        private const string userASP = "UserASP";
        private const string isRemembered = "IsRemebered";

        private static readonly string StringDefault = string.Empty;
        private static readonly bool booleanDefault = false;

        #endregion

        public static string UserASP
        {
            get
            {
                return AppSettings.GetValueOrDefault(userASP, StringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(userASP, value);
            }
        }

        public static string TokenType
        {
            get
            {
                return AppSettings.GetValueOrDefault(tokenType, StringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(tokenType, value);
            }
        }

        public static bool IsRemembered
        {
            get
            {
                return AppSettings.GetValueOrDefault(isRemembered, booleanDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(isRemembered, value);
            }
        }

        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(accessToken, StringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(accessToken, value);
            }
        }

    }
}
