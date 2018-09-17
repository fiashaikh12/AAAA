namespace Entities
{
    public class States
    {
        private int _stateId;
        private string _state;
        public int StateId
        {
            get{return _stateId;}
            set{ _stateId = value;}
        }

        public string State
        {
            set { _state = value; }
            get { return _state; }
        }
    }
    public class Cities
    {
        private int _cityId;
        private string _city;
        public int CityId
        {
            set { _cityId = value; }
            get { return _cityId; }
        }
        public string City
        {
            set { _city = value; }
            get { return _city; }
        }
    }
    public class Genders
    {
        private int _genderId;
        private string _gender;
        public int GenderId
        {
            get { return _genderId; }
            set { _genderId = value; }
        }
        public string Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }
    }
}
