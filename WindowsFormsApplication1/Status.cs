using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHKITMonitoring1
{
    public class Status
    {
        public Status()
        {
            this.state = null;
            this.value1 = null;
            this.color = null;
            this.name = null;
            this.state_S1 = null;
            this.value1_S1 = null;
            this.color_S1 = null;
            this.group = null;
        }

        public Status(string state, string value, string color, string name)
        {
            this.state = state;
            this.value1 = value;
            this.color = color;
            this.name = name;
        }

        public string State
        {
            get { return state; }
            set { state = value; }
        } private string state;

        public string Name
        {
            get { return name; }
            set { name = value; }
        } private string name;

        public string Value
        {
            get { return value1; }
            set { value1 = value;  }
        } private string value1;

        public string Color
        {
            get { return color; }
            set { color = value; }
        } private string color;

        public string State_S1
        {
            get { return state_S1; }
            set { state_S1 = value; }
        } private string state_S1;

        public string Value_S1
        {
            get { return value1_S1; }
            set { value1_S1 = value; }
        } private string value1_S1;

        public string Color_S1
        {
            get { return color_S1; }
            set { color_S1 = value; }
        } private string color_S1;

        public string Group
        {
            get { return group; }
            set { group = value; }
        } private string group;

        static internal List<Status> GetAllStatus()
        {
//            if (Status.AllStatus.Count == 0)
//                Status.AllStatus = Status.Create();
            return Status.AllStatus;
        }
        static private List<Status> AllStatus = new List<Status>();
    }
}
