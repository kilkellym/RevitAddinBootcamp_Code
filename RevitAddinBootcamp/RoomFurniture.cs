using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddinBootcamp
{
    internal class RoomFurniture
    {
        public string RoomName { get; set; }
        public string FamilyName {  get; set; }
        public string FamilyType { get; set; }
        public int Quantity { get; set; }

        public RoomFurniture(string roomName, string familyName, string familyType, int quantity)
        {
            RoomName = roomName;
            FamilyName = familyName;
            FamilyType = familyType;
            Quantity = quantity;
        }
    }
}
