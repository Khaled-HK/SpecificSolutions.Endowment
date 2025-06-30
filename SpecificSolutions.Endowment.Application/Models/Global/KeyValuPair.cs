namespace SpecificSolutions.Endowment.Application.Models.Global
{
    public class KeyValuPair
    {
        private string _value;
        private Guid key;

        public Guid Key
        {
            get
            {
                return key;
            }

            set => key = value;
        }

        public string Value
        {
            get => _value;

            set
            {
                _value = value;

                //OnPropertyChange(this, nameof(Value));
            }
        }
    }
}
