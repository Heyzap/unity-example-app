using System;
using System.Collections.Generic;
using FyberPlugin.LitJson;

namespace FyberPlugin
{

    public enum UserSexualOrientation {
		straight = 0,
		bisexual,
		gay,
		unknown
	};
	
	public enum UserGender {
		male = 0,
		female,
		other
	};
	
	public enum UserMaritalStatus {
		single = 0,
		relationship,
		married,
		divorced,
		engaged
	};
	
	public enum UserEducation {
		other = 0,	
		none,	
		high_school,	
		in_college,
		some_college,	
		associates,	
		bachelors,	
		masters,	
		doctorate
	};
	
	public enum UserEthnicity {
		asian = 0,
		black,
		hispanic,
		indian,
		middle_eastern,	
		native_american,	
		pacific_islander,	
		white,	
		other
	};
	
	public enum UserConnection {
		wifi = 0,	
		three_g
	};
	
	
	public partial class User
	{
		
		static User()
		{
		}
		
		//  static public void Reset() {
		//  	NativeReset();
		//  }
		
		static public void SetAge(int age) {
			Put(AGE, age);
		}
		
		static public DateTime? GetBirthdate() {
			string value = Get<string>(BIRTHDATE);
			DateTime parsedDate;
			if (DateTime.TryParseExact(value, "yyyy/MM/dd",
			                           System.Globalization.CultureInfo.InvariantCulture,
			                           System.Globalization.DateTimeStyles.None, 
			                           out parsedDate))
			{
				return parsedDate;
			}
			return null;
		}
		
		static public void SetBirthdate(DateTime birthdate) {
			Put(BIRTHDATE, birthdate);
		}
		
		
		static public void SetGender(UserGender gender) {
			Put(GENDER, gender);
		}
		
		static public void SetSexualOrientation(UserSexualOrientation sexualOrientation) {
			Put(SEXUAL_ORIENTATION, sexualOrientation);
		}
		
		static public void SetEthnicity(UserEthnicity ethnicity) {
			Put(ETHNICITY, ethnicity);
		}
		
		static public Location GetLocation() {
			return Get<Location>(LOCATION);
		}
		
		static public void SetLocation(Location location) {
			Put(LOCATION, location);
		}
		
		static public void SetMaritalStatus(UserMaritalStatus maritalStatus) {
			Put(MARITAL_STATUS, maritalStatus);
		}	
		
		static public void SetNumberOfChildrens(int numberOfChildrens) {
			Put(NUMBER_OF_CHILDRENS, numberOfChildrens);
		}
		
		static public void SetAnnualHouseholdIncome(int annualHouseholdIncome) {
			Put(ANNUAL_HOUSEHOLD_INCOME, annualHouseholdIncome);
		}
		
		static public void SetEducation(UserEducation education) {
			Put(EDUCATION, education);
		}
		
		static public string GetZipcode() {
			return Get<string>(ZIPCODE);
		}
		
		static public void SetZipcode(string zipcode) {
			Put(ZIPCODE, zipcode);
		}
		
		static public string[] GetInterests() {
			return Get<string[]>(INTERESTS);
		}
		
		static public void SetInterests(string[] interests) {
			Put(INTERESTS, interests);
		}
		
		static public void SetIap(Boolean iap) {
			Put(IAP, iap);
		}
		
		static public void SetIapAmount(float iap_amount) {
			Put(IAP_AMOUNT, (double)iap_amount);
		}
		
		static public void SetNumberOfSessions(int numberOfSessions) {
			Put(NUMBER_OF_SESSIONS, numberOfSessions);
		}
		
		static public void SetPsTime(long ps_time) {
			Put(PS_TIME, ps_time);
		}
		
		static public void SetLastSession(long lastSession) {
			Put(LAST_SESSION, lastSession);
		}
		
		static public void SetConnection(UserConnection connection) {
			Put(CONNECTION, connection);
		}
		
		static public string GetDevice() {
			return Get<string>(DEVICE);
		}
		
		static public void SetDevice(string device) {
			Put(DEVICE, device);
		}
		
		static public string GetAppVersion() {
			return Get<string>(APP_VERSION);
		}
		
		static public void SetAppVersion(string appVersion) {
			Put(APP_VERSION, appVersion);
		}
		
		static public void PutCustomValue(string key, string value) {
			Put(key, value);
		}
		
		static public string GetCustomValue(string key) {
			return Get<string>(key);
		}
		
		// Helper methods
		static private void Put(string key, object value)
		{
			string json = GeneratePutJsonString(key, value);
			NativePut(json);
		}
		
		static protected T Get<T>(string key)
		{
			string message = GetJsonMessage(key);
			JsonResponse<T> response = JsonMapper.ToObject<JsonResponse<T>>(message);
			if (response.success)
			{
				return response.value;
			}
			UnityEngine.Debug.Log(response.error);
			return default(T);
		}
		
		static private string GeneratePutJsonString(string key, object value)
		{
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("action", "put");
            dictionary.Add("key", key);
            dictionary.Add("type", value.GetType().ToString());
            if (value is DateTime)
            {
                dictionary.Add("value", ((DateTime)value).ToString("yyyy/MM/dd"));
            }
            else
            {
                dictionary.Add("value", value);
            }
            return JsonMapper.ToJson(dictionary);
        }
		
		static protected string GenerateGetJsonString(string key)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("action", "get");
			dictionary.Add("key", key);
			return JsonMapper.ToJson(dictionary);
		}
		
		protected const string AGE = "age";
		protected const string BIRTHDATE = "birthdate";
		protected const string GENDER = "gender";
		protected const string SEXUAL_ORIENTATION = "sexual_orientation";
		protected const string ETHNICITY = "ethnicity";
		protected const string MARITAL_STATUS = "marital_status";
		protected const string NUMBER_OF_CHILDRENS = "children";
		protected const string ANNUAL_HOUSEHOLD_INCOME = "annual_household_income";
		protected const string EDUCATION = "education";
		protected const string ZIPCODE = "zipcode";
		protected const string INTERESTS = "interests";
		protected const string IAP = "iap";
		protected const string IAP_AMOUNT = "iap_amount";
		protected const string NUMBER_OF_SESSIONS = "number_of_sessions";
		protected const string PS_TIME = "ps_time";
		protected const string LAST_SESSION = "last_session";
		protected const string CONNECTION = "connection";
		protected const string DEVICE = "device";
		protected const string APP_VERSION  = "app_version";
		protected const string LOCATION  = "fyberlocation";

#if !UNITY_WP8
   [System.Reflection.Obfuscation(Exclude = true)]
#endif		
		private class JsonResponse<T>
		{
			public Boolean success {get; set;}
			public string key { get; set; }
			public T value { get; set; }
			public string error { get; set;}
		}
		
	}
	
	// this is only needed because of the way we handle 
	// iOS Users
#if (UNITY_EDITOR || UNITY_ANDROID) 
	public partial class User
    {
		
        static public int? GetAge()
        {
            return Get<int?>(AGE);
        }

        static public UserGender? GetGender()
        {
            return Get<UserGender?>(GENDER);
        }

        static public UserSexualOrientation? GetSexualOrientation()
        {
            return Get<UserSexualOrientation?>(SEXUAL_ORIENTATION);
        }

        static public UserEthnicity? GetEthnicity()
        {
            return Get<UserEthnicity?>(ETHNICITY);
        }

        static public UserMaritalStatus? GetMaritalStatus()
        {
            return Get<UserMaritalStatus?>(MARITAL_STATUS);
        }

        static public int? GetNumberOfChildrens()
        {
            return Get<int?>(NUMBER_OF_CHILDRENS);
        }

        static public int? GetAnnualHouseholdIncome()
        {
            return Get<int?>(ANNUAL_HOUSEHOLD_INCOME);
        }

        static public UserEducation? GetEducation()
        {
            return Get<UserEducation?>(EDUCATION);
        }

        static public Boolean? GetIap()
        {
            return Get<Boolean?>(IAP);
        }

        static public float? GetIapAmount()
        {
            return (float?)Get<double?>(IAP_AMOUNT);
        }

        static public int? GetNumberOfSessions()
        {
            return Get<int?>(NUMBER_OF_SESSIONS);
        }

        static public long? GetPsTime()
        {
            return Get<long?>(PS_TIME);
        }

        static public long? GetLastSession()
        {
            return Get<long?>(LAST_SESSION);
        }

        static public UserConnection? GetConnection()
        {
            return Get<UserConnection?>(CONNECTION);
        }
    }
#endif

}


