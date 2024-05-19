using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace Icarus
{
    // Vanie Devi Srinivasan
    // Date: 18/05/2024
     // Version: 1.0
    // Name of the program:  The Service Application
    // Description: This Service application is for Icarus Pty Ltd to be used by front desk staff to log drones for service and repair.
    // This app enables the user to enter the drone details and add to the specified servicy delivery queue and added to the finished queue once the service is completed.

    /// Interaction logic for MainWindow.xaml

    public partial class MainWindow : Window
    {
        // Create  the regular and express queues.
        Queue<Drone> RegularServiceQueue = new Queue<Drone>();
        Queue<Drone> ExpressServiceQueue = new Queue<Drone>();

        List<Drone> FinishedDrones = new List<Drone>();

        private ObservableCollection<DroneView> rDroneCollection;
        private ObservableCollection<DroneView> eDroneCollection;
        private ObservableCollection<DroneView> fDroneCollection;

        private int lastServiceTag = 100;

        public MainWindow()
        {
            InitializeComponent();
            
            // Initialize the regular and express queues. 
            rDroneCollection = new ObservableCollection<DroneView>(); 
            eDroneCollection = new ObservableCollection<DroneView>();
            fDroneCollection = new ObservableCollection<DroneView>();

            LVRegular.ItemsSource = rDroneCollection;
            LVExpress.ItemsSource = eDroneCollection;
            LBoxFinish.ItemsSource = fDroneCollection;
        }        

        // Method to dispaly the service details.
        private void DisplayAllService()
        {
            rDroneCollection.Clear();
            foreach (Drone drone in RegularServiceQueue)
            {
                rDroneCollection.Add(new DroneView(drone));
            }

            eDroneCollection.Clear();
            foreach (Drone drone in ExpressServiceQueue)
            {
                eDroneCollection.Add(new DroneView(drone));
            }

            fDroneCollection.Clear();
            foreach (Drone drone in FinishedDrones)
            {
                fDroneCollection.Add(new DroneView(drone));
            }
        }

        // Method to clear all the textboxes and radiobuttons.
        private void ClearTextBoxes()
        {
            ClientName_TxtBox.Clear();
            DroneModelTxtBox.Clear();
            ServiceProblemTxtBox.Clear();
            ServiceCostTxtBox.Clear();
            ServiceTag.Text = lastServiceTag.ToString();
            RegularRdoBtn.IsChecked = false;
            ExpressRdoBtn.IsChecked = false;
        }
    
        // To add the service details to the regular or express service queue.
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ClientName_TxtBox.Text) &&
                !string.IsNullOrEmpty(DroneModelTxtBox.Text) &&
                !string.IsNullOrEmpty(ServiceProblemTxtBox.Text) &&
                !string.IsNullOrEmpty(ServiceCostTxtBox.Text))

            {
                var res = validateServiceCost(ServiceCostTxtBox.Text);
                if (!res)
                {
                    MessageBox.Show("Invalid Service Cost. Should be a valid decimal with two decimal digits.");
                    return;
                }

                Drone drone = new Drone();
                drone.SetClientName(ClientName_TxtBox.Text);
                drone.SetDroneModel(DroneModelTxtBox.Text);
                drone.SetServiceProblem(ServiceProblemTxtBox.Text);
                drone.SetServiceCost(double.Parse(ServiceCostTxtBox.Text));
                drone.SetServiceTag(incrementServiceTag());

                if (GetServicePriority() == "RegularRdoBtn")
                {
                    RegularServiceQueue.Enqueue(drone);
                }
                else
                {
                    var newServiceCost = Math.Round(drone.GetServiceCost() + (drone.GetServiceCost() * 0.15), 2);
                    drone.SetServiceCost(newServiceCost);
                    ExpressServiceQueue.Enqueue(drone);
                }
                
                DisplayAllService();
                
                ClearTextBoxes();
            }
        }

        //Exit Button:Closes the application when clicked.
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Custom method to increment the service tag control.
        private int incrementServiceTag()
        {            
            int newServiceTag = int.Parse(ServiceTag.Text) + 10;
            lastServiceTag = newServiceTag;

            ServiceTag.Text = newServiceTag.ToString();

            return newServiceTag;
        }

        //Custom method to ensure the service cost only accepts a double value with two decimal point. 
        private bool validateServiceCost(string serviceCost)
        {
            string pattern = @"^\d+(\.\d{2})$";

            return Regex.IsMatch(serviceCost, pattern);            
        }

        // Custom method to return the value of the priority radio group.
        private string GetServicePriority()
        {
            string radioText = "";

            if (RegularRdoBtn.IsChecked == true)
            {
                radioText = RegularRdoBtn.Name;
            }
            else if (ExpressRdoBtn.IsChecked == true)
            {
                radioText = ExpressRdoBtn.Name;
            }

            return radioText;
        }

        //To dispaly the drone details of the regular service in the related textboxes.
        private void LVRegular_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LVRegular.SelectedIndex != -1)
            {
                int indx = LVRegular.SelectedIndex;
                ClientName_TxtBox.Text = RegularServiceQueue.ElementAt(indx).GetClientName();
                DroneModelTxtBox.Text = RegularServiceQueue.ElementAt(indx).GetDroneModel();
                ServiceProblemTxtBox.Text = RegularServiceQueue.ElementAt(indx).GetServiceProblem();
                ServiceCostTxtBox.Text = RegularServiceQueue.ElementAt(indx).GetServiceCost().ToString();
                ServiceTag.Text = RegularServiceQueue.ElementAt(indx).GetServiceTag().ToString();
                RegularRdoBtn.IsChecked = true;
                ExpressRdoBtn.IsChecked = false;
            }
        }

        //To dispaly the drone details of the express service in the related textboxes.
        private void LVExpress_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LVExpress.SelectedIndex != -1)
            {
                int indx = LVExpress.SelectedIndex;
                ClientName_TxtBox.Text = ExpressServiceQueue.ElementAt(indx).GetClientName();
                DroneModelTxtBox.Text = ExpressServiceQueue.ElementAt(indx).GetDroneModel();
                ServiceProblemTxtBox.Text = ExpressServiceQueue.ElementAt(indx).GetServiceProblem();
                ServiceCostTxtBox.Text = ExpressServiceQueue.ElementAt(indx).GetServiceCost().ToString();
                ServiceTag.Text = ExpressServiceQueue.ElementAt(indx).GetServiceTag().ToString();
                ExpressRdoBtn.IsChecked = true;
                RegularRdoBtn.IsChecked = false;
            }
        }

        //Finish Button 1: Details of the finished service in the regular queue added to the finshed queue
        private void FinishBtn1_Click(object sender, RoutedEventArgs e)
        {
            if (LVRegular.SelectedIndex != -1)
            {
                int indx = LVRegular.SelectedIndex;

                var result = MessageBox.Show("Regular Service Finished - Service Tag:" + RegularServiceQueue.ElementAt(indx).GetServiceTag().ToString(), "Service Completion Confirmation!", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.OK)
                {
                    Drone drone = RegularServiceQueue.ElementAt(indx);
                    FinishedDrones.Add(drone);

                    Queue<Drone> tmpRegularServiceQueue = new Queue<Drone>();

                    foreach (Drone tDrone in RegularServiceQueue)
                    {
                        if (tDrone.GetServiceTag() != drone.GetServiceTag())
                        {
                            tmpRegularServiceQueue.Enqueue(tDrone);
                        }
                    }

                    RegularServiceQueue.Clear();

                    foreach (Drone tDrone in tmpRegularServiceQueue)
                    {
                        RegularServiceQueue.Enqueue(tDrone);
                    }

                    tmpRegularServiceQueue.Clear();

                    DisplayAllService();

                    ClearTextBoxes();
                }
            }
        }

        // Finish Button 2: Details of the finished service in the express queue added to the finshed queue
        private void FinishBtn2_Click(object sender, RoutedEventArgs e)
        {
            if (LVExpress.SelectedIndex != -1)
            {
                int indx = LVExpress.SelectedIndex;

                var result = MessageBox.Show("Express Service Finished - Service Tag:" + ExpressServiceQueue.ElementAt(indx).GetServiceTag().ToString(), "Service Completion Confirmation!", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.OK)
                {
                    Drone drone = ExpressServiceQueue.ElementAt(indx);
                    FinishedDrones.Add(drone);

                    Queue<Drone> tmpExpressServiceQueue = new Queue<Drone>();

                    foreach (Drone tDrone in ExpressServiceQueue)
                    {
                        if (tDrone.GetServiceTag() != drone.GetServiceTag())
                        {
                            tmpExpressServiceQueue.Enqueue(tDrone);
                        }
                    }

                    ExpressServiceQueue.Clear();

                    foreach (Drone tDrone in tmpExpressServiceQueue)
                    {
                        ExpressServiceQueue.Enqueue(tDrone);
                    }

                    tmpExpressServiceQueue.Clear();

                    DisplayAllService();

                    ClearTextBoxes();
                }
            }
        }

        // Double mouse click method to delete a service item from the finished listbox and remove the same item from the List.
        private void LBoxFinish_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int indx = LBoxFinish.SelectedIndex;

            if (indx != -1)
            {
                var result = MessageBox.Show("Delivering Drone - Service Tag:" + FinishedDrones.ElementAt(indx).GetServiceTag().ToString(), "Delivery Confirmation!", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.OK)
                {
                    FinishedDrones.RemoveAt(indx);

                    DisplayAllService();
                }                
            }
        }        
    }    
}