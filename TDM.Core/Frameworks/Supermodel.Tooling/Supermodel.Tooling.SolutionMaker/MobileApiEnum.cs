using System.ComponentModel;

#nullable enable

namespace Supermodel.Tooling.SolutionMaker
{
    public enum MobileApiEnum 
    { 
        [Description("Xamarin.Forms")] XamarinForms, 
        [Description("Platform's Native API")] Native 
    }
}