#nullable enable

using System;

namespace Supermodel.Presentation.Cmd
{
    public class ModelStateInvalidException : Exception
    {
        public ModelStateInvalidException(object model)
        {
            Model = model;
        }

        public object Model { get; protected set; }
    }
}
