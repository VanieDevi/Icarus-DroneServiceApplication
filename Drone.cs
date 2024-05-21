using System.Globalization;

namespace Icarus
{
    // Vanie Devi Srinivasan
    // Date: 18/05/2024
    // Version: 1.0
    // Name of the program:  Drone
    // Description: Drone class that stores the properties of Drone with get & set methods.

    public class Drone
    {
        //Private Attributes
        private string ClientName { get; set; }
        private string DroneModel { get; set; }
        private string? ServiceProblem { get; set; }
        private double ServiceCost { get; set; }
        private int ServiceTag { get; set; }

        //Default Constructor
        public Drone() 
        {
            ClientName = "";
            DroneModel = "";
            ServiceProblem = "";
            ServiceCost = 0.0;
            ServiceTag = 0;
        }

        public Drone(string clientName, string droneModel, string? serviceProblem, double serviceCost, int serviceTag)
        {
            ClientName = clientName;
            DroneModel = droneModel;
            ServiceProblem = serviceProblem;
            ServiceCost = serviceCost;
            ServiceTag = serviceTag;
        }

        //Assessor Methods
        public string GetClientName()
        {
            return ClientName;
        }

        public void SetClientName(string clientName)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            this.ClientName = textInfo.ToTitleCase(clientName);
        }

        public string GetDroneModel()
        {
            return DroneModel;
        }

        public void SetDroneModel(string droneModel)
        {
            this.DroneModel = droneModel;
        }

        public string GetServiceProblem()
        {
            return ServiceProblem;
        }

        public void SetServiceProblem(string serviceProblem)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            this.ServiceProblem = textInfo.ToTitleCase(serviceProblem);
        }

        public double GetServiceCost()
        {
            return ServiceCost;
        }

        public void SetServiceCost(double serviceCost)
        {
            this.ServiceCost = serviceCost;
        }

        public int GetServiceTag()
        {
            return ServiceTag;
        }

        public void SetServiceTag(int serviceTag)
        {
            this.ServiceTag = serviceTag;
        }

        public string displayValue()
        {
            return ClientName + " | " + DroneModel + " | " + ServiceTag + " | $" + ServiceCost;
        }
    }
}
