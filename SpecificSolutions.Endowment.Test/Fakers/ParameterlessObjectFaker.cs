using Bogus;
using System.Runtime.Serialization;

namespace SpecificSolutions.Endowment.Test.Fakers
{
    public class ParameterlessObjectFaker<T> : Faker<T> where T : class
    {
        public ParameterlessObjectFaker()
        {
            CustomInstantiator(_ => Initialize());
        }

        private static T Initialize() =>
            FormatterServices.GetUninitializedObject(typeof(T)) as T ?? throw new TypeLoadException();
    }
}
