using System.Threading.Tasks;
using Supermodel.Mobile.Runtime.Common.Models;

namespace Supermodel.Mobile.Runtime.Common.XForms.ViewModels
{
    public abstract class XFModelForModel<TModel> : XFModelForModelBase<TModel> where TModel : class, IModel, ISupermodelNotifyPropertyChanged, new()
    {
        #region Constructors
        public virtual Task<XFModelForModel<TModel>> InitAsync(TModel model)
        {
            Model = model;
            return Task.FromResult(this);
        }
        #endregion
    }
}