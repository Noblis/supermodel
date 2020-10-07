#nullable enable

using System.Collections.Generic;

namespace Supermodel.Persistence.Entities
{
    public class M2MTo1Lis2<TM2M> : M2MList<TM2M> where TM2M : IM2M
    {

    }
    public class M2MTo1List<TM2M> : M2MList<TM2M> where TM2M : IM2M
    {

    }
    
    public abstract class M2MList<TM2M> : List<TM2M> where TM2M : IM2M
    {
    }
}
