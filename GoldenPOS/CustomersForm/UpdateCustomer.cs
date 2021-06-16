using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoldenPOS
{
    public partial class UpdateCustomer : Form
    {
        int customerID;
        public UpdateCustomer(int ID)
        {
            InitializeComponent();
            customerID = ID;
        }

        private void UpdateCustomer_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> customer = DataManager.GetOneCustomerAllInfoByID(customerID);

            textBox1.Text = customer["name"];

        }
    }
}
