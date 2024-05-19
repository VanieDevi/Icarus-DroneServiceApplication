using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icarus
{
    public class DroneView
    {
        private readonly Drone drone;

        public DroneView(Drone drone)
        {
            this.drone = drone;
        }
        public string ClientName => drone.GetClientName();
        public string DroneModel => drone.GetDroneModel();
        public string? ServiceProblem => drone.GetServiceProblem();
        public double ServiceCost => drone.GetServiceCost();
        public int ServiceTag => drone.GetServiceTag();
    }
}
