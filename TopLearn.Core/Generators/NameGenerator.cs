using System;
using System.Collections.Generic;
using System.Text;

namespace TopLearn.Core.Generators
{
    public class NameGenerator
    {
        public static string GenerateName() =>
            Guid.NewGuid().ToString().Replace("-", "");

    }
}
