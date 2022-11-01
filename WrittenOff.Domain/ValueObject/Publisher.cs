using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrittenOffManagement.Domain.ValueObject
{
    public class Publisher : BaseValueObject
    {
        public String Name { get; private set; }
        public String Descritption { get; private set; }

        public Publisher(string name, string descritption)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(descritption)) throw new ArgumentNullException(nameof(descritption));

            Name = name;
            Descritption = descritption;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return Descritption;
        }
    }
}
