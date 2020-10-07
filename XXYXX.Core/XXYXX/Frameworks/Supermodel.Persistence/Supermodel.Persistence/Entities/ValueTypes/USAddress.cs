#nullable enable

namespace Supermodel.Persistence.Entities.ValueTypes
{
    public class USAddress : ValueObject
    {
        #region Properties
        public string Street { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Zip { get; set; } = "";
        #endregion
    }
}
