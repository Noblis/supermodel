#nullable enable

using WebMonk.Session;

namespace Supermodel.Presentation.WebMonk.Extensions.Gateway
{
    public static class SuperNamespaceGateway
    {
        #region TempDataDictionary Gateway Methods
        public static SupermodelNamespaceTempDataDictionaryExtensions Super(this TempDataDictionary tempData)
        {
            return new SupermodelNamespaceTempDataDictionaryExtensions(tempData);
        }
        #endregion
    }
    
    public class SupermodelNamespaceTempDataDictionaryExtensions
    {
        #region Constructors
        public SupermodelNamespaceTempDataDictionaryExtensions(TempDataDictionary tempData)
        {
            _tempData = tempData;
        }
        #endregion

        #region Methods/Properties
        public string? NextPageStartupScript
        {
            get => (string?)_tempData["sm-startupScript"];
            set => _tempData["sm-startupScript"] = value;
        }
        public string? NextPageAlertMessage
        {
            get => (string?)_tempData["sm-alertMessage"];
            set => _tempData["sm-alertMessage"] = value;
        }
        public string? NextPageModalMessage
        {
            get => (string?)_tempData["sm-modalMessage"];
            set => _tempData["sm-modalMessage"] = value;
        }
        #endregion

        #region Fields
        private readonly TempDataDictionary _tempData;
        #endregion
    }
}
