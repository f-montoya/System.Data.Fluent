﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Fluent.Abstraction
{
    public interface IFunctionBuilder
    {
        IFunctionBuilder Parameters(Action<IParameterBuilder> parametersAction);

        T Execute<T>();
    }
}
