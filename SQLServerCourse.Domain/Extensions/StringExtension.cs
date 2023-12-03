using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Extensions
{
    public static class StringExtension
    {
        public static string Join(this List<string> stringCollection)
        {
            var sb = new StringBuilder();

            for (int i = 1; i < stringCollection.Count + 1; i++)
            {
                sb.Append($"{i}: {stringCollection[i - 1]} ");
            }

            return sb.ToString();
        }
    }
}
