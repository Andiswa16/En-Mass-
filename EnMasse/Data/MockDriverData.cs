using EnMasse.Models;

namespace EnMasse.Data
{
    public static class MockDriverData
    {
        public static List<Driver> Drivers = new List<Driver>
        {
            new Driver
            {
                Id = 1001,
                DUsername = "Homer Simpson",
                DPassword = "Simpsons1234"
            },
            new Driver
            {
                Id = 1002,
                DUsername = "Sofia First",
                DPassword = "GirlinVillage001"
            },
            new Driver
            {
                Id = 1003,
                DUsername = "Chris Brown",
                DPassword = "Brown123!"
            }
        };

        public static Driver Authenticate(string username, string password)
        {
            return Drivers.FirstOrDefault(d =>
                d.DUsername == username && d.DPassword == password);
        }
        public static Driver? GetDriverByUsername(string username)
        {
            return Drivers.FirstOrDefault(d => d.DUsername == username);
        }
        public static bool UpdatePassword(string username, string newPassword)
        {
            var driver = Drivers.FirstOrDefault(d => d.DUsername == username);

            if (driver != null)
            {
                driver.DPassword = newPassword;
                return true;
            }

            return false;
        }
    }
}