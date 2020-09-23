using System;

namespace AirconLib
{
    public class FanOutput
    {
        // Private instance fields
        private int _id;
        private string _name;
        private int _temp;
        private int _humidity;

        /// <summary>
        /// Instantiates a new FanOutput Object.
        /// When a new FanOutput object is created, initialize given arguments
        /// </summary>
        /// <param name="id">The ID of the current FanOutput object</param>
        /// <param name="name">The name of the current FanOutput object. Name cannot be less than 2 characters</param>
        /// <param name="temp">The temperature of the current FanOutput object. Any number between 15 and 25</param>
        /// <param name="humid">The humidity of the current FanOutput object. Any number between 30 and 80</param>
        public FanOutput(int id, string name, int temp, int humid)
        {
            _id = id;
            _name = name;
            _temp = temp;
            _humidity = humid;
        }
        public FanOutput()
        {

        }

        /// <summary>
        /// The ID of the current FanOutput object.
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// The name of the current FanOutput object.
        /// Name cannot be less than 2 characters.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name is null or empty");
                }
                else if (value.Length < 2)
                {
                    throw new ArgumentOutOfRangeException("Name cannot be less than 2 characters.");
                }
                _name = value;
            }
        }
        /// <summary>
        /// The temperature of the current FanOutput object.
        /// Temperature has to be between 15 and 25.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int Temp
        {
            get { return _temp; }
            set
            {
                if (value < 15 || value > 25)
                {
                    throw new ArgumentOutOfRangeException("Temperature must be between 15 and 25.");
                }
                else
                {
                    _temp = value;
                }
            }
        }
        /// <summary>
        /// The humidity of the current FanOutput object.
        /// Humidity has to be between 30 and 80.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int Humidity
        {
            get { return _humidity; }
            set
            {
                if (value < 30 || value > 80)
                {
                    throw new ArgumentOutOfRangeException("Humidity must be between 30 and 80.");
                }
                else
                {
                    _humidity = value;
                }
            }
        }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Temperature {Temp}, Humidity: {Humidity}";
        }
    }
}
