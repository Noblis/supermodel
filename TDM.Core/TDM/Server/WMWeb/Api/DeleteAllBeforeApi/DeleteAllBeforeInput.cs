#nullable enable

using System;

namespace WMWeb.Api.DeleteAllBeforeApi
{
    public class DeleteAllBeforeInput
    {
        public DateTime OlderThanUtc { get; set; }
    }
}