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

            populate();
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
    
        

        public void populate()
        {
            dataGridView1.Rows.Add("blah", "blah", "blah", "blah");
            dataGridView1.Rows.Add("blah", "blah", "blah", "blah");
            dataGridView1.Rows.Add("blah", "blah", "blah", "blah");
            dataGridView1.Rows.Add("blah", "blah", "blah", "blah");
            dataGridView1.Rows.Add("blah", "blah", "blah", "blah");
        }

    }
}
