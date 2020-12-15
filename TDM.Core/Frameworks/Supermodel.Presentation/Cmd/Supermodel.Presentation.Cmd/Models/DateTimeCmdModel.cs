﻿#nullable enable

using System;
using System.Globalization;
using System.Threading.Tasks;
using Supermodel.Presentation.Cmd.ConsoleOutput;
using Supermodel.Presentation.Cmd.Models.Base;

namespace Supermodel.Presentation.Cmd.Models
{
    public class DateTimeCmdModel : DateTimeCmdModelCore
    {
        #region Constructors
        public DateTimeCmdModel()
        {
            Type = typeof(DateTime);
        }
        #endregion

        #region ICmdEditor
        public override object Edit(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue)
        {
            DateTimeValue = ConsoleExt.EditDateTime(DateTimeValue);
            return this;            
        }
        #endregion

        #region ICmdDisplay
        public override void Display(int screenOrderFrom = int.MinValue, int screenOrderTo = int.MaxValue)
        {
            Console.WriteLine(Value);
        }
        #endregion

        #region IRMapperCustom implemtation
        public override async Task MapFromCustomAsync<T>(T other)
        {
            await base.MapFromCustomAsync(other).ConfigureAwait(false);
                
            //Set correct format
            if (DateTimeValue != null) Value = DateTimeValue.Value.ToString(CultureInfo.InvariantCulture);
        }
        #endregion
    }
}
