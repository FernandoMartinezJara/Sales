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
        private const string isRemebered = "IsRemebered";

        private static readonly string StringDefault = string.Empty;
        private static readonly bool booleanDefault = false;

        #endregion

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

        public static bool IsRemebered
        {
            get
            {
                return AppSettings.GetValueOrDefault(isRemebered, booleanDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(isRemebered, value);
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
