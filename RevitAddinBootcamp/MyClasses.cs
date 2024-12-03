using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddinBootcamp
{
    // 4. create another class
    public class Neighborhood
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public List<Building> BuildingList { get; set; }

        public Neighborhood(string _name, string _city, string _state, 
            List<Building> _buildingList)
        {
            Name = _name;
            City = _city;
            State = _state;
            BuildingList = _buildingList;
        }

        public int GetBuildingCount()
        {
            return BuildingList.Count;
        }
    }

    // 1. create class 
    public class Building
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int NumberOfFloors { get; set; }
        public double Area { get; set; }

        // 3. add constuctor to class
        public Building(string _name, string _address, int _numberOfFloors, double _area)
        {
            Name = _name;
            Address = _address;
            NumberOfFloors = _numberOfFloors;
            Area = _area;
        }
    }
}
