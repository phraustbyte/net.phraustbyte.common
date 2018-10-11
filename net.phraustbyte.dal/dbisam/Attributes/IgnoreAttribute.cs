using System;

namespace net.phraustbyte.dal.dbisam
{
    /// <summary>
    /// Attribute - indicates to the generator that the value is not to be utilized
    /// </summary>
    public class IgnoreAttribute : Attribute
    {
        /// <summary>
        /// indicates to the generator that the value is not to be utilized
        /// </summary>
        public bool Ignore { get; }
        /// <summary>
        /// default constructor
        /// </summary>
        public IgnoreAttribute ()
        {
            this.Ignore = true;
        }
    }
}
