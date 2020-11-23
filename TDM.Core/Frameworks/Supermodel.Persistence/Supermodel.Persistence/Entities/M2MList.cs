#nullable enable

using System.Collections.Generic;

namespace Supermodel.Persistence.Entities
{
    public class M2MList<TM2M> : List<TM2M> where TM2M : IM2M { }
}
