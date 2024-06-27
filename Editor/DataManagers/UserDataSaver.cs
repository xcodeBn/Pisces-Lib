using UnityEngine;

namespace PiData
{
    public static class UserDataSaver
    {
        private const string UserNameKey = "UserName";
        private const string EmailKey = "Email";
        private const string AgeKey = "Age";
        private const string AddressKey = "Address";
        private const string LocationKey = "Location";
        private const string PremiumKey = "Premium";

        public static void SaveUserInfo(string userName, string email, int age, string address, string location, bool premium)
        {
            PlayerPrefs.SetString(UserNameKey, userName);
            PlayerPrefs.SetString(EmailKey, email);
            PlayerPrefs.SetInt(AgeKey, age);
            PlayerPrefs.SetString(AddressKey, address ?? string.Empty);
            PlayerPrefs.SetString(LocationKey, location ?? string.Empty);
            PlayerPrefs.SetInt(PremiumKey, premium ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log("UserInfo saved successfully.");
        }

        public static void LoadUserInfo(out string userName, out string email, out int age, out string address, out string location, out bool premium)
        {
            userName = PlayerPrefs.GetString(UserNameKey, string.Empty);
            email = PlayerPrefs.GetString(EmailKey, string.Empty);
            age = PlayerPrefs.GetInt(AgeKey, 0);
            address = PlayerPrefs.GetString(AddressKey, null);
            location = PlayerPrefs.GetString(LocationKey, null);
            premium = PlayerPrefs.GetInt(PremiumKey, 0) == 1;

            Debug.Log("UserInfo loaded successfully.");
        }

        public static void DeleteUserInfo()
        {
            PlayerPrefs.DeleteKey(UserNameKey);
            PlayerPrefs.DeleteKey(EmailKey);
            PlayerPrefs.DeleteKey(AgeKey);
            PlayerPrefs.DeleteKey(AddressKey);
            PlayerPrefs.DeleteKey(LocationKey);
            PlayerPrefs.DeleteKey(PremiumKey);
            PlayerPrefs.Save();
            Debug.Log("UserInfo deleted successfully.");
        }
    }
}
