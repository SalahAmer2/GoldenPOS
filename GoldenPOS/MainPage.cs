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
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();
        }

        #region Menu Items Actions

        private void DashBtn_Click(object sender, EventArgs e)
        {
            DashBtn.BackColor = Color.FromArgb(51, 122, 255);
            CustomersBtn.BackColor = Color.White;
            VendorsBtn.BackColor = Color.White;
            DelegatesBtn.BackColor = Color.White;
            ProductsBtn.BackColor = Color.White;
            CategoriesBtn.BackColor = Color.White;
            UsersBtn.BackColor = Color.White;
            AppPages.SelectTab("dashTab");
        }

        private void CustomersBtn_Click(object sender, EventArgs e)
        {
            CustomersBtn.BackColor = Color.FromArgb(51, 122, 255);
            DashBtn.BackColor = Color.White;
            VendorsBtn.BackColor = Color.White;
            DelegatesBtn.BackColor = Color.White;
            ProductsBtn.BackColor = Color.White;
            CategoriesBtn.BackColor = Color.White;
            UsersBtn.BackColor = Color.White;
            AppPages.SelectTab("customersTab");
            LoadCustomersDGV();

        }

        private void VendorsBtn_Click(object sender, EventArgs e)
        {
            VendorsBtn.BackColor = Color.FromArgb(51, 122, 255);
            CustomersBtn.BackColor = Color.White;
            DashBtn.BackColor = Color.White;
            DelegatesBtn.BackColor = Color.White;
            ProductsBtn.BackColor = Color.White;
            CategoriesBtn.BackColor = Color.White;
            UsersBtn.BackColor = Color.White;
            AppPages.SelectTab("vendorsTab");
            LoadVendorsDGV();
        }

        private void DelegatesBtn_Click(object sender, EventArgs e)
        {
            DelegatesBtn.BackColor = Color.FromArgb(51, 122, 255);
            CustomersBtn.BackColor = Color.White;
            DashBtn.BackColor = Color.White;
            VendorsBtn.BackColor = Color.White;
            ProductsBtn.BackColor = Color.White;
            CategoriesBtn.BackColor = Color.White;
            UsersBtn.BackColor = Color.White;
            AppPages.SelectTab("delegatesTab");
            LoadVendorsDelegatesDGV();
            LoadCustomersDelegatesDGV();
        }

        private void ProductsBtn_Click(object sender, EventArgs e)
        {
            ProductsBtn.BackColor = Color.FromArgb(51, 122, 255);
            CustomersBtn.BackColor = Color.White;
            DashBtn.BackColor = Color.White;
            VendorsBtn.BackColor = Color.White;
            DelegatesBtn.BackColor = Color.White;
            CategoriesBtn.BackColor = Color.White;
            UsersBtn.BackColor = Color.White;
            AppPages.SelectTab("productsTab");
            LoadProductsDGV();
        }

        private void CategoriesBtn_Click(object sender, EventArgs e)
        {
            CategoriesBtn.BackColor = Color.FromArgb(51, 122, 255);
            CustomersBtn.BackColor = Color.White;
            DashBtn.BackColor = Color.White;
            VendorsBtn.BackColor = Color.White;
            DelegatesBtn.BackColor = Color.White;
            ProductsBtn.BackColor = Color.White;
            UsersBtn.BackColor = Color.White;
            AppPages.SelectTab("categoriesTab");
        }

        private void UsersBtn_Click(object sender, EventArgs e)
        {
            UsersBtn.BackColor = Color.FromArgb(51, 122, 255);
            CustomersBtn.BackColor = Color.White;
            DashBtn.BackColor = Color.White;
            VendorsBtn.BackColor = Color.White;
            DelegatesBtn.BackColor = Color.White;
            ProductsBtn.BackColor = Color.White;
            CategoriesBtn.BackColor = Color.White;
            AppPages.SelectTab("usersTab");
        }

        #endregion

        #region DGV Loaders

        public void LoadCustomersDGV()
        {
            Dictionary<string, string> Filters = new Dictionary<string, string>()
            {
                {"nameOrPhones","" },
                {"type","" },
                {"deleted","" }
            };
            customerDGV.DataSource = DataManager.GetCustomersBasicInfo(Filters);
        }

        public void LoadVendorsDGV()
        {
            Dictionary<string, string> Filters = new Dictionary<string, string>()
            {
                {"nameOrPhones","" },
                {"otherProducts","" },
                {"deleted","" }
            };
            vendorDGV.DataSource = DataManager.GetVendorsBasicInfo(Filters);
        }

        public void LoadVendorsDelegatesDGV()
        {
            Dictionary<string, string> Filters = new Dictionary<string, string>()
            {
                {"nameOrPhones","" },
                {"vendor","" },
                {"deleted","" }
            };
            vendorDelegateDGV.DataSource = DataManager.GetVendorsDelegatesBasicInfo(Filters);
        }
       
        public void LoadCustomersDelegatesDGV()
        {
            Dictionary<string, string> Filters = new Dictionary<string, string>()
            {
                {"nameOrPhones","" },
                {"customer","" },
                {"deleted","" }
            };
            customerDelegateDGV.DataSource = DataManager.GetCustomersDelegatesBasicInfo(Filters);
        }

        public void LoadProductsDGV()
        {
            Dictionary<string, string> Filters = new Dictionary<string, string>()
            {
                {"pCode","" },
                {"pCategory","" },
                {"deleted","" }
            };
            productsDGV.DataSource = DataManager.GetProductsBasicInfo(Filters);
        }



        #endregion

        #region Customers Actions

        private void CustomerAddBtn_Click(object sender, EventArgs e)
        {
            AddCustomer form = new AddCustomer();
            form.ShowDialog();
        }

        private void CustomerUpdateBtn_Click(object sender, EventArgs e)
        {
            UpdateCustomer form = new UpdateCustomer(Convert.ToInt32(customerDGV.SelectedRows[0].Cells[0]));
        }

        

        #endregion
    }
}
