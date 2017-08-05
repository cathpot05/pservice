using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Data;
using System.Windows.Media.Effects;
using System.Configuration;
using System.Reflection;
using System.Text.RegularExpressions;
using KINPO_Product_Service;
using System.IO;

namespace try1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        #region loginverification

        public bool AdminLogin(String Email, String PW)
        {
            var appSettings = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location).AppSettings;
            String appConf_Email = appSettings.Settings["UID"].Value;
            String appConf_Pass = appSettings.Settings["Password"].Value;
            return Email.Equals(appConf_Email, StringComparison.Ordinal) && PW.Equals(appConf_Pass, StringComparison.Ordinal);
            // key Email = admin@gmail.com Password = admin12345!
        }

        #endregion

        #region SQL test connection

        public bool IsServerConnected(String uid,String pass, String serverName, String databaseName) //for sql connection testing
        {
            using (var l_oConnection = new SqlConnection("Data Source="+serverName+";Initial Catalog="+ databaseName + ";UID="+uid+";password="+pass+""))
            {
                try
                {
                    l_oConnection.Open();
                    return true;
                }

                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }

            }
        }
        #endregion
        private void btnlogout_Click(object sender, RoutedEventArgs e)
        {
            pnlMember.Visibility = Visibility.Collapsed;
            pnlDeviceInfo.Visibility = Visibility.Collapsed;
            pnlProduct.Visibility = Visibility.Collapsed;
            pnlServiceAccess.Visibility = Visibility.Collapsed;
            pnlNewDatabase.Visibility = Visibility.Collapsed;     
            Storyboard sb = Resources["logout"] as Storyboard;
            sb.Begin();
            ListDB.Items.Clear();
        }
   
        private void btnclose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnMember_Click(object sender, RoutedEventArgs e)
        {
            pnlMember.Visibility = Visibility.Visible;
            pnlDeviceInfo.Visibility = Visibility.Hidden;
            pnlProduct.Visibility = Visibility.Hidden;
            pnlServiceAccess.Visibility = Visibility.Hidden;
            tbClearProduct();
            tbClearServAccess();
            tbClearDevInfo();
            tbClearMember();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString()))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Select * from [member]", con);
                    SqlDataAdapter DAdapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable("Employee");
                    DAdapter.Fill(dt);
                    tblMember.ItemsSource = dt.DefaultView;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 53)
                    {
                        MessageBox.Show("Could not connect to database.");
                    }
                    if (ex.Number == 208)
                    {
                         MessageBox.Show("Could not find table in " + ListDB.SelectedValue + " database.");
                    }
                    else { MessageBox.Show(ex.Message); }
                }
                catch (Exception ex)// All other non sql errors
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        private void btnDeviceInfo_Click(object sender, RoutedEventArgs e)
        {
            pnlMember.Visibility = Visibility.Hidden;
            pnlDeviceInfo.Visibility = Visibility.Visible;
            pnlProduct.Visibility = Visibility.Hidden;
            pnlServiceAccess.Visibility = Visibility.Hidden;

            tbClearProduct();
            tbClearServAccess();
            tbClearDevInfo();
            tbClearMember();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString()))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Select * from [deviceinfo]", con);
                    SqlDataAdapter DAdapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable("DeviceInformation");
                    DAdapter.Fill(dt);
                    tblDevInfo.ItemsSource = dt.DefaultView;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 53)
                    {
                        MessageBox.Show("Could not connect to database.");
                    }
                    if (ex.Number == 208)
                    {
                         MessageBox.Show("Could not find table in " +ListDB.SelectedValue + " database.");
                    }
                    else { MessageBox.Show(ex.Message); }
                }
                catch (Exception ex)// All other non sql errors
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }               
        }
        private void btnProduct_Click(object sender, RoutedEventArgs e)
        {
            pnlMember.Visibility = Visibility.Hidden;
            pnlDeviceInfo.Visibility = Visibility.Hidden;
            pnlProduct.Visibility = Visibility.Visible;
            pnlServiceAccess.Visibility = Visibility.Hidden;
            tbClearProduct();
            tbClearServAccess();
            tbClearDevInfo();
            tbClearMember();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString()))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Select * from [product]", con);
                    SqlDataAdapter DAdapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable("Product");
                    DAdapter.Fill(dt);
                    tblProduct.ItemsSource = dt.DefaultView;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 53)
                    {
                        MessageBox.Show("Could not connect to database.");
                    }
                    if (ex.Number == 208)
                    {
                         MessageBox.Show("Could not find table in " + ListDB.SelectedValue + " database.");
                    }
                    else { MessageBox.Show(ex.Message); }
                }
                catch (Exception ex)// All other non sql errors
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }           
        }
        private void btnServiceAccess_Click(object sender, RoutedEventArgs e)
        {
            tbClearProduct();
            tbClearServAccess();
            tbClearDevInfo();
            tbClearMember();
            pnlMember.Visibility = Visibility.Hidden;
            pnlDeviceInfo.Visibility = Visibility.Hidden;
            pnlProduct.Visibility = Visibility.Hidden;
            pnlServiceAccess.Visibility = Visibility.Visible;
            Refresh_Product();           
        }
        void Refresh_Product()
        {
           using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString()))
           {
              try
              {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT distinct [product].ProductID, [product].ProductName from serviceaccess inner join product on serviceaccess.productid = product.productid", con);
                SqlDataAdapter DAdapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable("Employee");
                DAdapter.Fill(dt);
                MyProducttbl.ItemsSource = dt.DefaultView;
              }
                catch (SqlException ex)
                {
                    if (ex.Number == 53)
                    {
                        MessageBox.Show("Could not connect to database.");
                    }
                    if(ex.Number == 208)
                    {
                         MessageBox.Show("Could not find table in " +ListDB.SelectedValue + " database.");
                    }
                }
                catch (Exception ex)// All other non sql errors
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }                  
        }
        private void logomenu_Click(object sender, RoutedEventArgs e)
        {
            pnlMember.Visibility = Visibility.Hidden;
            pnlDeviceInfo.Visibility = Visibility.Hidden;
            pnlProduct.Visibility = Visibility.Hidden;
            pnlServiceAccess.Visibility = Visibility.Hidden;
        }


        private void MyProducttbl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Refresh_ServiceAccess();
        }
        #region Control content clearing
        void ClearTables()
        {
            MyProducttbl.IsEnabled.Equals(false);
            MyProducttbl.Columns.Clear();
            MyProducttbl.ItemsSource = null;
            MyProducttbl.Items.Refresh();

            tblDevInfo.IsEnabled.Equals(false);
            tblDevInfo.Columns.Clear();
            tblDevInfo.ItemsSource = null;
            tblDevInfo.Items.Refresh();

            tblMember.IsEnabled.Equals(false);
            tblMember.Columns.Clear();
            tblMember.ItemsSource = null;
            tblMember.Items.Refresh();

            tblNoAcc.IsEnabled.Equals(false);
            tblNoAcc.Columns.Clear();
            tblNoAcc.ItemsSource = null;
            tblNoAcc.Items.Refresh();

            tblProduct.IsEnabled.Equals(false);
            tblProduct.Columns.Clear();
            tblProduct.ItemsSource = null;
            tblProduct.Items.Refresh();

            tblServAcc.IsEnabled.Equals(false);
            tblServAcc.Columns.Clear();
            tblServAcc.ItemsSource = null;
            tblServAcc.Items.Refresh();
        }
        void tbClearProduct()
        {
            tblServAcc.IsEnabled.Equals(false);
            tblServAcc.Columns.Clear();
            tblServAcc.ItemsSource = null;
            tblServAcc.Items.Refresh();
            tb_p_prodID.Clear();
            tb_p_prodname.Clear();
            tb_p_publicIPadd.Clear();
            tb_p_hname.Clear();
            tb_p_type.Clear();
            tb_p_dCreated.Clear();
            tb_p_dDeactivated.Clear();
            tb_p_stat.Clear();
        }
        void tbClearServAccess()
        {
            tblServAcc.Columns.Clear();
            tblServAcc.ItemsSource = null;
            tblServAcc.Items.Refresh();
            tblNoAcc.Columns.Clear();
            tblNoAcc.ItemsSource = null;
            tblNoAcc.Items.Refresh();
            tb_sa_id.Clear();
            tb_sa_memID.Clear();
            tb_sa_EmAdd.Clear();
            tb_sa_dCreated.Clear();
            tb_sa_dModified.Clear();
            tb_sa_uname.Clear();
            tb_sa_stat.Clear();
            tb_sa_prodID.Clear();
            tb_sa_prodname.Clear();
        }
        void tbClearDevInfo()
        {
            tblServAcc.IsEnabled.Equals(false);
            tblServAcc.Columns.Clear();
            tblServAcc.ItemsSource = null;
            tblServAcc.Items.Refresh();
            tb_di_controlNum.Clear();
            tb_di_memID.Clear();
            tb_di_emAdd.Clear();
            tb_di_swver.Clear();
            tb_di_serialnum.Clear();
            tb_di_modname.Clear();
            tb_di_uuid_emmc.Clear();
            tb_di_bID.Clear();
            tb_di_bname.Clear();
            tb_di_macAdd.Clear();
            tb_di_uuid2_cp.Clear();
            tb_di_uuid2_pln.Clear();
            tb_di_grpID.Clear();
            tb_di_dCreated.Clear();
            tb_di_dModified.Clear();
            tb_di_stat.Clear();
            tb_di_latestver.Clear();
            tb_di_updatestat.Clear();
        }
        void tbClearMember()
        {
            tblServAcc.IsEnabled.Equals(false);
            tblServAcc.Columns.Clear();
            tblServAcc.ItemsSource = null;
            tblServAcc.Items.Refresh();
            tb_m_memID.Clear();
            tb_m_emAdd.Clear();
            tb_m_prodID.Clear();
            tb_m_uname.Clear();
            tb_m_pass.Clear();
            tb_m_bday.Clear();
            tb_m_gender.Clear();
            tb_m_cty.Clear();
            tb_m_lang.Clear();
            tb_m_dCreated.Clear();
            tb_m_dModified.Clear();
            tb_m_stat.Clear();
        }
        #endregion
        void Refresh_ServiceAccess()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString()))
            {
                try
                {
                    if (MyProducttbl.SelectedIndex != -1)
                    {
                        DataRowView row = (DataRowView)MyProducttbl.SelectedItems[0];
                        SqlCommand cmd = new SqlCommand("Select distinct [serviceaccess].ID as 'Access ID', [serviceaccess].MemberID as 'Member ID', [member].EmailAdd as 'Email Address',[serviceaccess].DateCreated as 'Date Created', [serviceaccess].DateModified as  'Date Modified',[member].Username as 'Username', [serviceaccess].status from [serviceaccess] inner join member on [member].memberid = [serviceaccess].memberid where [serviceaccess].productID = " + row[0].ToString() + " and [serviceaccess].status = 1", con);
                        SqlDataAdapter DAdapter = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable("tblHasAccess1");
                        DAdapter.Fill(dt);
                        tblServAcc.ItemsSource = dt.DefaultView;
                        tblServAcc.IsEnabled = true;
                        sa_hasacc.Content = "Users who has access to " + row[1].ToString() + ":";
                        sa_Noacc.Content = "Users who has NO access to " + row[1].ToString() + ":";
                        SqlCommand cmd2 = new SqlCommand("Select distinct [serviceaccess].ID, [serviceaccess].MemberID, [member].EmailAdd ,[serviceaccess].DateCreated, [serviceaccess].DateModified,[member].Username, [serviceaccess].status from [serviceaccess] inner join member on [member].memberid = [serviceaccess].memberid where [serviceaccess].productID = " + row[0].ToString() + " and [serviceaccess].status = 0", con);
                        SqlDataAdapter DAdapter2 = new SqlDataAdapter(cmd2);
                        DataSet ds2 = new DataSet();
                        DataTable dt2 = new DataTable("NoAccess");
                        DAdapter2.Fill(dt2);
                        tblNoAcc.ItemsSource = dt2.DefaultView;
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 53)
                    {
                        MessageBox.Show("Could not connect to database.");
                    }
                    if (ex.Number == 208)
                    {
                         MessageBox.Show("Could not find table in " +ListDB.SelectedValue + " database.");
                    }
                    else { MessageBox.Show(ex.Message); }
                }
                catch (Exception ex)// All other non sql errors
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void btnadd_Click(object sender, RoutedEventArgs e)
        {
            if (tb_sa_id.Text.ToString().Length < 1)
            {
                MessageBox.Show("None selected.");
            }
            else if (Convert.ToInt32(tb_sa_stat.Text.ToString()) == 1)
            {
                MessageBox.Show("Selected member already has access to this product.");
            }
            else
            {
                ConfirmDialog.Content = "       Are you sure you want to \n     GIVE " + tb_sa_uname.Text + " access to: " + tb_sa_prodname.Text + "?";
                CODE.Content = "ACCESS";
                Storyboard sb = Resources["areyousure1"] as Storyboard;
                sb.Begin();
                var blur = new BlurEffect();
                Menu.Effect = blur;
                Menu.IsEnabled = false;
                pnlServiceAccess.Effect = blur;
                pnlServiceAccess.IsEnabled = false;
            }
        }

        private void tblNoAcc_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (tblNoAcc.Items.Count != 0)
                {
                    if (tblNoAcc.SelectedIndex != -1)
                    {
                        DataRowView row = (DataRowView)tblNoAcc.SelectedItems[0];
                        tb_sa_id.Text = row[0].ToString();
                        tb_sa_memID.Text = row[1].ToString();
                        tb_sa_EmAdd.Text = row[2].ToString();
                        tb_sa_dCreated.Text = row[3].ToString();
                        tb_sa_dModified.Text = row[4].ToString();
                        tb_sa_uname.Text = row[5].ToString();
                        tb_sa_stat.Text = row[6].ToString();

                        DataRowView prow = (DataRowView)MyProducttbl.SelectedItems[0];
                        tb_sa_prodID.Text = prow[0].ToString();
                        tb_sa_prodname.Text = prow[1].ToString();
                    }

                }

            }
            catch (SqlException ex)
            {
                if (ex.Number == 53)
                {
                    MessageBox.Show("Could not connect to database.");
                }
                if (ex.Number == 208)
                {
                     MessageBox.Show("Could not find table in " +ListDB.SelectedValue + " database.");
                }
                else { MessageBox.Show(ex.Message); }
            }
            catch (Exception ex)// All other non sql errors
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btndelete_Click(object sender, RoutedEventArgs e)
        {
            if (tb_sa_id.Text.ToString().Length < 1)
            {
                MessageBox.Show("None selected.");
            }
            else if (Convert.ToInt32(tb_sa_stat.Text.ToString()) == 0)
            {
                MessageBox.Show("Selected member already has no access to this product.");
            }
            else
            {
                ConfirmDialog.Content = "       Are you sure you want to \n     REVOKE " + tb_sa_uname.Text + " access to: " + tb_sa_prodname.Text + "?";
                CODE.Content = "REVOKE";
                Storyboard sb = Resources["areyousure1"] as Storyboard;
                sb.Begin();
                var blur = new BlurEffect();
                Menu.Effect = blur;
                Menu.IsEnabled = false;
                pnlServiceAccess.Effect = blur;
                pnlServiceAccess.IsEnabled = false;
            }
        }
        private void btnno_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["no"] as Storyboard;
            sb.Begin();
            Menu.Effect = null;
            Menu.IsEnabled = true;
            pnlServiceAccess.Effect = null;
            pnlServiceAccess.IsEnabled = true;
        }

        void Give_Revoke(String AccessID, int type) // 1 - Access  0 - Revoke
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString()))
            {
                try
                {
                    con.Open();
                    string sql = "UPDATE [serviceaccess] SET [DateModified] = '" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff") + "' ,[Status] = " + type.ToString() + " WHERE ID = " + AccessID + "";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.ExecuteNonQuery();
                    Refresh_ServiceAccess();
                    tbClearServAccess();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 53)
                    {
                        MessageBox.Show("Could not connect to database.");
                    }
                    if (ex.Number == 208)
                    {
                         MessageBox.Show("Could not find table in " +ListDB.SelectedValue + " database.");
                    }
                    else { MessageBox.Show(ex.Message); }
                }
                catch (Exception ex)// All other non sql errors
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void MyProducttbl_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            Refresh_ServiceAccess();
        }
        public class WaitCursor : IDisposable
        {
            private Cursor _previousCursor;

            public WaitCursor()
            {
                _previousCursor = Mouse.OverrideCursor;

                Mouse.OverrideCursor = Cursors.Wait;
            }

            #region IDisposable Members

            public void Dispose()
            {
                Mouse.OverrideCursor = _previousCursor;
            }

            #endregion
        }
        void refreshComboBoxDB()
        {
            using (SqlConnection con = new SqlConnection("Data Source=crkpph-dbtest-limuel.database.windows.net;Initial Catalog=master;User ID=dbtestadmin;Password=crkpph4DB"))
            {
                try
                {
                    con.Open();
                    ListDB.Items.Clear();
                    SqlCommand cmd = new SqlCommand("select * FROM sysdatabases", con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ListDB.Items.Add(dr.GetValue(0));
                    }
                    dr.Close();
                    ListDB.SelectedIndex = 0;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 53)
                    {
                        MessageBox.Show("Could not connect to database.");
                    }
                    if (ex.Number == 208)
                    {
                        MessageBox.Show("Could not find table in " + ListDB.SelectedValue + " database.");
                    }
                    else
                    { MessageBox.Show(ex.Message); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }


            }
        }
        private void LoginAttempt(object sender, RoutedEventArgs e)
        {
            using (new WaitCursor())
            {
                if (AdminLogin(textBoxEmail.Text, passwordBox.Password))
                {
                    Storyboard sb = this.FindResource("LoginSuccess") as Storyboard;
                    sb.Begin();
                    refreshComboBoxDB();
                }
                else
                    MessageBox.Show("Invalid information");
            }
            
           
        }

        private void btnyes_Click(object sender, RoutedEventArgs e)
        {
            if (CODE.Content.ToString().Equals("ACCESS", StringComparison.Ordinal))
            {
                Give_Revoke(tb_sa_id.Text, 1);
                MessageBox.Show("Successfully granted access.");
            }
            else
            {
                Give_Revoke(tb_sa_id.Text, 0);
                MessageBox.Show("Successfully revoked.");
            }
            btnno.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void tblProduct_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (tblProduct.SelectedIndex != -1)
            {
                try
                {
                    DataRowView row = (DataRowView)tblProduct.SelectedItems[0];
                    tb_p_prodID.Text = row[0].ToString();
                    tb_p_prodname.Text = row[1].ToString();
                    tb_p_publicIPadd.Text = row[2].ToString();
                    tb_p_hname.Text = row[3].ToString();
                    tb_p_type.Text = row[4].ToString();
                    tb_p_dCreated.Text = row[5].ToString();
                    tb_p_dDeactivated.Text = row[6].ToString();
                    tb_p_stat.Text = row[7].ToString();

                }
                catch (SqlException ex)
                {
                    if (ex.Number == 53)
                    {
                        MessageBox.Show("Could not connect to database.");
                    }
                    if (ex.Number == 208)
                    {
                         MessageBox.Show("Could not find table in " +ListDB.SelectedValue + " database.");
                    }
                    else { MessageBox.Show(ex.Message); }            
                }
                catch (Exception ex)// All other non sql errors
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void tblDevInfo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (tblDevInfo.SelectedIndex != -1)
            {
                try
                {
                    DataRowView row = (DataRowView)tblDevInfo.SelectedItems[0];
                    tb_di_controlNum.Text = row[0].ToString();
                    tb_di_memID.Text = row[1].ToString();
                    tb_di_emAdd.Text = row[2].ToString();
                    tb_di_swver.Text = row[3].ToString();
                    tb_di_serialnum.Text = row[4].ToString();
                    tb_di_modname.Text = row[5].ToString();
                    tb_di_uuid_emmc.Text = row[6].ToString();
                    tb_di_bID.Text = row[7].ToString();
                    tb_di_bname.Text = row[8].ToString();
                    tb_di_macAdd.Text = row[9].ToString();
                    tb_di_uuid2_cp.Text = row[10].ToString();
                    tb_di_uuid2_pln.Text = row[11].ToString();
                    tb_di_grpID.Text = row[12].ToString();
                    tb_di_dCreated.Text = row[13].ToString();
                    tb_di_dModified.Text = row[14].ToString();
                    tb_di_stat.Text = row[15].ToString();
                    tb_di_latestver.Text = row[16].ToString();
                    tb_di_updatestat.Text = row[17].ToString();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 53)
                    {
                        MessageBox.Show("Could not connect to database.");
                    }
                    if (ex.Number == 208)
                    {
                         MessageBox.Show("Could not find table in " +ListDB.SelectedValue + " database.");
                    }
                    else { MessageBox.Show(ex.Message); }
                }
                catch (Exception ex)// All other non sql errors
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void tblMember_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (tblMember.SelectedIndex != -1)
            {
                try
                {
                    DataRowView row = (DataRowView)tblMember.SelectedItems[0];
                    tb_m_memID.Text = row[0].ToString();
                    tb_m_emAdd.Text = row[1].ToString();
                    tb_m_prodID.Text = row[2].ToString();
                    tb_m_uname.Text = row[4].ToString();
                    tb_m_pass.Text = row[3].ToString();
                    tb_m_bday.Text = row[5].ToString();
                    tb_m_gender.Text = row[6].ToString();
                    tb_m_cty.Text = row[7].ToString();
                    tb_m_lang.Text = row[8].ToString();
                    tb_m_dCreated.Text = row[9].ToString();
                    tb_m_dModified.Text = row[10].ToString();
                    tb_m_stat.Text = row[11].ToString();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 53)
                    {
                        MessageBox.Show("Could not connect to database.");
                    }
                    if (ex.Number == 208)
                    {
                         MessageBox.Show("Could not find table in " +ListDB.SelectedValue + " database.");
                    }
                    else { MessageBox.Show(ex.Message); }
                }
                catch (Exception ex)// All other non sql errors
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void tblServAcc_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {             
                    if (tblServAcc.SelectedIndex != -1)
                    {
                        DataRowView row = (DataRowView)tblServAcc.SelectedItems[0];
                        tb_sa_id.Text = row[0].ToString();
                        tb_sa_memID.Text = row[1].ToString();
                        tb_sa_EmAdd.Text = row[2].ToString();
                        tb_sa_dCreated.Text = row[3].ToString();
                        tb_sa_dModified.Text = row[4].ToString();
                        tb_sa_uname.Text = row[5].ToString();
                        tb_sa_stat.Text = row[6].ToString();

                        DataRowView prow = (DataRowView)MyProducttbl.SelectedItems[0];
                        tb_sa_prodID.Text = prow[0].ToString();
                        tb_sa_prodname.Text = prow[1].ToString();
                    }             
            }
            catch (SqlException ex)
            {
                if (ex.Number == 53)
                {
                    MessageBox.Show("Could not connect to database.");
                }
                if (ex.Number == 208)
                {
                     MessageBox.Show("Could not find table in " +ListDB.SelectedValue + " database.");
                }
                else { MessageBox.Show(ex.Message); }
            }
            catch (Exception ex)// All other non sql errors
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnlogin.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
        private void MyProducttbl_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            Refresh_ServiceAccess();
            tb_sa_prodID.Text = "";
            tb_sa_memID.Text = "";
            tb_sa_dCreated .Text = "";
            tb_sa_dModified.Text = "";
            tb_sa_EmAdd.Text = "";
            tb_sa_id.Text = "";
            tb_sa_uname.Text = "";
            tb_sa_stat.Text = "";
            tb_sa_prodname.Text = "";
        }

        private void tblServAcc_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            try
            {     
                if (tblServAcc.Items.Count != 0)
                {
                    if (tblServAcc.SelectedIndex != -1)
                    {
                        DataRowView row = (DataRowView)tblServAcc.SelectedItems[0];
                        tb_sa_id.Text = row[0].ToString();
                        tb_sa_memID.Text = row[1].ToString();
                        tb_sa_EmAdd.Text = row[2].ToString();
                        tb_sa_dCreated.Text = row[3].ToString();
                        tb_sa_dModified.Text = row[4].ToString();
                        tb_sa_uname.Text = row[5].ToString();
                        tb_sa_stat.Text = row[6].ToString();
                        DataRowView prow = (DataRowView)MyProducttbl.SelectedItems[0];
                        tb_sa_prodID.Text = prow[0].ToString();
                        tb_sa_prodname.Text = prow[1].ToString();
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 53)
                {
                    MessageBox.Show("Could not connect to database.");
                }
                if (ex.Number == 208)
                {
                    MessageBox.Show("Could not find table in " +ListDB.SelectedValue + " database.");
                }
                else { MessageBox.Show(ex.Message); }
            }
            catch (Exception ex)// All other non sql errors
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void btndelete_Click_1(object sender, RoutedEventArgs e)
        {
            if (tb_sa_id.Text.ToString().Length < 1)
            {
                MessageBox.Show("None selected.");
            }
            else if (Convert.ToInt32(tb_sa_stat.Text.ToString()) == 0)
            {
                MessageBox.Show("Selected member already has no access to this product.");
            }
            else
            {
                ConfirmDialog.Content = "       Are you sure you want to \n     REVOKE " + tb_sa_uname.Text + " access to: " + tb_sa_prodname.Text + "?";
                CODE.Content = "REVOKE";
                Storyboard sb = Resources["areyousure1"] as Storyboard;
                sb.Begin();
                var blur = new BlurEffect();
                Menu.Effect = blur;
                Menu.IsEnabled = false;
                pnlServiceAccess.Effect = blur;
                pnlServiceAccess.IsEnabled = false;
            }
        }
        private void btnadd_Click_1(object sender, RoutedEventArgs e)
        {
            if (tb_sa_id.Text.ToString().Length < 1)
            {
                MessageBox.Show("None selected.");
            }
            else if (Convert.ToInt32(tb_sa_stat.Text.ToString()) == 1)
            {
                MessageBox.Show("Selected member already has access to this product.");
            }
            else
            {
                ConfirmDialog.Content = "       Are you sure you want to \n     GIVE " + tb_sa_uname.Text + " access to: " + tb_sa_prodname.Text + "?";
                CODE.Content = "ACCESS";
                Storyboard sb = Resources["areyousure1"] as Storyboard;
                sb.Begin();
                var blur = new BlurEffect();
                Menu.Effect = blur;
                Menu.IsEnabled = false;
                pnlServiceAccess.Effect = blur;
                pnlServiceAccess.IsEnabled = false;
            }
        }

        private void tblNoAcc_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (tblNoAcc.Items.Count != 0)
                {
                    if (tblNoAcc.SelectedIndex != -1)
                    {
                        DataRowView row = (DataRowView)tblNoAcc.SelectedItems[0];
                        tb_sa_id.Text = row[0].ToString();
                        tb_sa_memID.Text = row[1].ToString();
                        tb_sa_EmAdd.Text = row[2].ToString();
                        tb_sa_dCreated.Text = row[3].ToString();
                        tb_sa_dModified.Text = row[4].ToString();
                        tb_sa_uname.Text = row[5].ToString();
                        tb_sa_stat.Text = row[6].ToString();

                        DataRowView prow = (DataRowView)MyProducttbl.SelectedItems[0];
                        tb_sa_prodID.Text = prow[0].ToString();
                        tb_sa_prodname.Text = prow[1].ToString();
                    }

                }

            }
            catch (SqlException ex)
            {
                if (ex.Number == 53)
                {
                    MessageBox.Show("Could not connect to database.");
                }
                if (ex.Number == 208)
                {
                     MessageBox.Show("Could not find table in " +ListDB.SelectedValue + " database.");
                }
                else { MessageBox.Show(ex.Message); }
            }
            catch (Exception ex)// All other non sql errors
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void textBoxEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (passwordBox.Password.Length > 0) { btnlogin.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
                else { MessageBox.Show("Enter password."); passwordBox.Focus(); }                
            }
        }
        private void btncancel_Click(object sender, RoutedEventArgs e)
        {
            textBoxEmail.Text = null;
            passwordBox.Password = null;
        }
        
        private void ListDB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ListDB.SelectedIndex != -1)
            {
                tbClearProduct();
                tbClearServAccess();
                tbClearDevInfo();
                tbClearMember();
                ClearTables();

                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
                connectionStringsSection.ConnectionStrings["ConnectionString"].ConnectionString = "Data Source=crkpph-dbtest-limuel.database.windows.net;Initial Catalog=" + ListDB.SelectedValue.ToString()+ ";UID=dbtestadmin;password=crkpph4DB";
                config.Save();
                ConfigurationManager.RefreshSection("connectionStrings");
            }           
        }

     
        private void tb_p_publicIPadd_TextChanged(object sender, TextChangedEventArgs e)
        {
            watermarke_ip.Text = tb_p_publicIPadd.Text;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (IPtextbox.Text.ToString().Length > 1)
            {
                MessageBox.Show(IPtextbox.Text);
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString()))
                {
                    con.Open();

                }
            }
        }
        #region watermarks & textboxes

        private void tb_newDB_Pass_LostFocus_1(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(tb_newDB_Pass.Password))
            {
                tb_newDB_Pass.Visibility = Visibility.Collapsed;
                watermarkPass.Visibility = Visibility.Visible;
            }
        }
        private void watermark_GotFocus(object sender, RoutedEventArgs e)
        {
            textBoxEmail.Visibility = Visibility.Visible;
            watermarke.Visibility = Visibility.Collapsed;
            textBoxEmail.Focus();
        }
        private void watermark2_GotFocus(object sender, RoutedEventArgs e)
        {
            IPtextbox.Visibility = Visibility.Visible;
            watermarke_ip.Visibility = Visibility.Collapsed;
            IPtextbox.Focus();
        }
        private void IP_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(IPtextbox.Text))
            {
                IPtextbox.Visibility = Visibility.Collapsed;
                watermarke_ip.Visibility = Visibility.Visible;
            }
        }

        private void passwordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                passwordBox.Visibility = Visibility.Collapsed;
                watermarkp.Visibility = Visibility.Visible;
            }
        }
        private void watermarkp_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordBox.Visibility = Visibility.Visible;
            watermarkp.Visibility = Visibility.Collapsed;
            passwordBox.Focus();
        }
        private void textBoxEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxEmail.Text))
            {
                textBoxEmail.Visibility = Visibility.Collapsed;
                watermarke.Visibility = Visibility.Visible;
            }
        }
        private void newDB_Click(object sender, RoutedEventArgs e)
        {
            pnlMember.Visibility = Visibility.Hidden;
            pnlDeviceInfo.Visibility = Visibility.Hidden;
            pnlProduct.Visibility = Visibility.Hidden;
            pnlServiceAccess.Visibility = Visibility.Hidden;
            pnlNewDatabase.Visibility = Visibility.Visible;
        }
        private void watermarkDBname_GotFocus(object sender, RoutedEventArgs e)
        {
            tb_newDB_DBname.Visibility = Visibility.Visible;
            watermarkDBname.Visibility = Visibility.Collapsed;
            tb_newDB_DBname.Focus();
        }
        private void tb_newDB_DBname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tb_newDB_DBname.Text))
            {
                tb_newDB_DBname.Visibility = Visibility.Collapsed;
                watermarkDBname.Visibility = Visibility.Visible;
            }
        }
        private void tb_newDB_UID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tb_newDB_UID.Text))
            {
                tb_newDB_UID.Visibility = Visibility.Collapsed;
                watermarkUID.Visibility = Visibility.Visible;
            }
        }
        private void watermarkUID_GotFocus(object sender, RoutedEventArgs e)
        {
            tb_newDB_UID.Visibility = Visibility.Visible;
            watermarkUID.Visibility = Visibility.Collapsed;
            tb_newDB_UID.Focus();
        }
        private void tb_newDB_Pass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tb_newDB_UID.Text))
            {
                tb_newDB_Pass.Visibility = Visibility.Collapsed;
                watermarkPass.Visibility = Visibility.Visible;
            }
        }

        private void watermarkPass_GotFocus(object sender, RoutedEventArgs e)
        {
            tb_newDB_Pass.Visibility = Visibility.Visible;
            watermarkPass.Visibility = Visibility.Collapsed;
            tb_newDB_Pass.Focus();
        }
        #endregion



        private void btn_cancelNewDB_Click(object sender, RoutedEventArgs e)
        {
            tb_newDB_DBname.Text = String.Empty;
            tb_newDB_Pass.Password = String.Empty;
            tb_newDB_UID.Text = String.Empty;
        }

        private void btn_confirmNewDB_Click(object sender, RoutedEventArgs e)
        {
            string serverName = "crkpph-dbtest-limuel.database.windows.net"; //change this to change server
            if (!tb_newDB_DBname.Text.Equals(String.Empty, StringComparison.Ordinal))
            {
                MessageBoxResult done = MessageBox.Show("Create database named " + tb_newDB_DBname.Text + "?", "Create Database", MessageBoxButton.YesNo);
                if (done == MessageBoxResult.Yes)
                {
                    createNewDatabase(tb_newDB_UID.Text, tb_newDB_Pass.Password, serverName, tb_newDB_DBname.Text);
                }
            }
            else
                MessageBox.Show("Please enter database name.");
        }

        #region Create db and tables
        public void createNewDatabase(string UID, string Pass, string ServerName, string newDBName)
        {
            
            using (new WaitCursor())
            {
                using (SqlConnection con = new SqlConnection("Data Source=" + ServerName + ";Initial Catalog=master;UID=" + UID + ";password=" + Pass + ""))
                {
                    try
                    {
                        con.Open();
                        string commandText = "Create database "+newDBName+"";
                        SqlCommand cmd = new SqlCommand(commandText, con);
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                        addTables(UID, Pass, ServerName, newDBName);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
        }
        public void addTables(string UID, string Pass, string ServerName, string newDBName)
        {
            using (SqlConnection con = new SqlConnection("Data Source=" + ServerName + ";Initial Catalog="+newDBName+";UID=" + UID + ";password=" + Pass + ""))
            {
                try
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    con.Open();
                    string commandtext;
                    using (Stream stream = assembly.GetManifestResourceStream("KINPO_Product_Service.Resources.createTablesQuery.sql"))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        commandtext = reader.ReadToEnd();
                    }
                    SqlCommand cmd = new SqlCommand(commandtext, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Database with tables created.");
                    refreshComboBoxDB();
                    btn_cancelNewDB.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        #endregion

       
    }
}
