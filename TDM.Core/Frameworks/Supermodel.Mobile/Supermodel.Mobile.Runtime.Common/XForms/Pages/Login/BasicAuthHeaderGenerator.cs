using Supermodel.Encryptor;
using System.Threading.Tasks;
using Supermodel.DataAnnotations.Exceptions;
using Supermodel.Mobile.Runtime.Common.Services;
using Xamarin.Forms;

namespace Supermodel.Mobile.Runtime.Common.XForms.Pages.Login
{
    public class BasicAuthHeaderGenerator : IAuthHeaderGenerator
    {
        #region Constructors
        public BasicAuthHeaderGenerator(string username, string password, byte[] localStorageEncryptionKey = null)
        {
            Username = username;
            Password = password;
            LocalStorageEncryptionKey = localStorageEncryptionKey;
        }
        #endregion
				
        #region Methods
        public virtual AuthHeader CreateAuthHeader()
        {
            return HttpAuthAgent.CreateBasicAuthHeader(Username, Password);
        }

        public virtual void Clear()
        {
            Username = Password = "";
        }
        public virtual async Task ClearAndSaveToPropertiesAsync()
        {
            if (LocalStorageEncryptionKey == null) throw new SupermodelException("ClearAndSaveToPropertiesAsync(): LocalStorageEncryptionKey = null");

            Clear();
            Application.Current.Properties["smUsername"] = null;
            Application.Current.Properties["smPasswordCode"] = null;
            Application.Current.Properties["smPasswordIV"] = null;
            await Application.Current.SavePropertiesAsync();
        }
        public virtual async Task SaveToAppPropertiesAsync()
        {
            if (Pick.RunningPlatform() == Platform.DotNetCore) throw new SupermodelException("SaveToAppPropertiesAsync() is only supported on mobile platforms");
            
            if (LocalStorageEncryptionKey == null) throw new SupermodelException("SaveToAppPropertiesAsync(): LocalStorageEncryptionKey = null");

            if  (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password)) throw new SupermodelException("string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password)");

            var passwordCode = EncryptorAgent.Lock(LocalStorageEncryptionKey, Password, out var passwordIV);

            Application.Current.Properties["smUsername"] = Username;
            Application.Current.Properties["smPasswordCode"] = passwordCode;
            Application.Current.Properties["smPasswordIV"] = passwordIV;
            Application.Current.Properties["smUserLabel"] = UserLabel;
            Application.Current.Properties["smUserId"] = UserId;
            await Application.Current.SavePropertiesAsync();
        }
        public virtual bool LoadFromAppProperties()
        {
            if (Pick.RunningPlatform() == Platform.DotNetCore) throw new SupermodelException("LoadFromAppProperties() is only supported on mobile platforms");
            
            if (LocalStorageEncryptionKey == null) throw new SupermodelException("LoadFromAppProperties(): LocalStorageEncryptionKey = null");

            if (Application.Current.Properties.ContainsKey("smUsername") && 
                Application.Current.Properties.ContainsKey("smPasswordCode") && 
                Application.Current.Properties.ContainsKey("smPasswordIV") && 
                Application.Current.Properties.ContainsKey("smUserLabel") && 
                Application.Current.Properties.ContainsKey("smUserId"))
            {
                if (!(Application.Current.Properties["smUsername"] is string username)) return false;
                if (!(Application.Current.Properties["smPasswordCode"] is byte[] passwordCode)) return false;
                if (!(Application.Current.Properties["smPasswordIV"] is byte[] passwordIV)) return false;

                //User label and userId can be null
                var userLabel = Application.Current.Properties["smUserLabel"] as string;
                var userId = Application.Current.Properties["smUserId"] as long?;

                Password = EncryptorAgent.Unlock(LocalStorageEncryptionKey, passwordCode, passwordIV);
                Username = username;
                UserLabel = userLabel;
                UserId = userId;
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
				
        #region Properties
        public long? UserId { get; set; }
        public string UserLabel { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        private byte[] LocalStorageEncryptionKey { get; }
        #endregion
    }
}
