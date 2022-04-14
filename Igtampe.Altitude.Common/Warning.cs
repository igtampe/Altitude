namespace Igtampe.Altitude.Common {

    /// <summary>A Warning of some sort</summary>
    public class Warning {

        /// <summary>Level of a warning</summary>
        public enum WarningLevel{
            /// <summary>Information that the user should know</summary>
            INFO = 0, 

            /// <summary>Warning that the user needs to know about</summary>
            WARNING = 1, 
            
            /// <summary>Error that the user needs to know about immediately</summary>
            ERROR = 2
        }

        /// <summary>Level of this warning</summary>
        public WarningLevel Level { get; set; } = WarningLevel.INFO;

        /// <summary>Item this warning was generated from</summary>
        public string Item { get; set; } = "";

        /// <summary>Message of this warning</summary>
        public string Message { get; set; } = "";

    }
}
