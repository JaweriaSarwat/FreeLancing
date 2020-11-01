﻿namespace Generics
{
    public static class Enums
    {
        public enum LogType
        {
            Success = 0,
            Functional = 1,
            Info = 2,
            Sql = 3,
            Exception = 4
        };
        public enum QueryType
        {
            Reterieve = 1,
            NonQuery = 2,
            Scalar = 3
        }
        public enum SessionName
        {
            AccountSetup,
            UserDetails,
            MedicineDetail,
            POSdetail
        };

        public enum AccessLevel
        {
            Admin = 1001,
            CompanyAdmin = 1002,
            Operator = 1003
        }

        public enum ButtonControl
        {
            Save,
            Update,
            Delete
        }

        public enum GridCommand
        {
            EditRow,
            DeleteRow
        }
    }
}
