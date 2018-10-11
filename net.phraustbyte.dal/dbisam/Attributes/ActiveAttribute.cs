using System;

namespace net.phraustbyte.dal.dbisam
{
    /// <summary>
    /// Attribute - indicates to the generator that the value is not to be utilized
    /// </summary>
    public class ActiveAttribute : Attribute
    {
        /// <summary>
        /// indicates to the generator that the value is not to be utilized
        /// </summary>
        public bool Active { get; }
        /// <summary>
        /// default constructor
        /// </summary>
        public ActiveAttribute()
        {
            this.Active = true;
        }
    }
}
