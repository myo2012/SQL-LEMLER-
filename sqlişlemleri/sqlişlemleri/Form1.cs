using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace sqlişlemleri
{
    public partial class Form1 : Form
    {

        DataSet ds = new DataSet(); 
        SqlConnection conn = new SqlConnection("Data Source=CODER\\SQLEXPRESS;Initial Catalog=YOUTUBE;Integrated Security=TRUE");
        SqlDataAdapter da = new SqlDataAdapter();
        BindingSource tablo = new BindingSource(); //3
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*conn.Open();
            MessageBox.Show(conn.State.ToString());
            conn.Close();*/

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            da.InsertCommand = new SqlCommand("INSERT INTO tablo VALUES(@ad,@soyad)", conn);
            da.InsertCommand.Parameters.Add("@ad", SqlDbType.VarChar).Value = textBox1.Text;
            da.InsertCommand.Parameters.Add("@soyad",SqlDbType.VarChar).Value=textBox2.Text;

            conn.Open();

            da.InsertCommand.ExecuteNonQuery();

            conn.Close();

           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            da.SelectCommand = new SqlCommand("SELECT * FROM tablo", conn);
            ds.Clear();

            da.Fill(ds);

            dg.DataSource = ds.Tables[0];

            tablo.DataSource = ds.Tables[0]; //3

            textBox1.DataBindings.Add(new Binding("Text", tablo, "ad"));
            textBox2.DataBindings.Add(new Binding("Text", tablo, "soyad"));
            goster();




        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tablo.MoveNext();
            dgUpdate();
            goster();

        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            tablo.MovePrevious();
            dgUpdate();
            goster();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            tablo.MoveFirst();
            dgUpdate();
            goster();

        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            tablo.MoveLast();
            dgUpdate();
            goster();
        }

        private void dgUpdate()
        {

            dg.ClearSelection();
            dg.Rows[tablo.Position].Selected = true;
            goster();

        }

        private void goster()
        {
            label3.Text = "Kayıtlar  " + (tablo.Position +1 )+ " - " + (tablo.Count);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int x;


            da.UpdateCommand = new SqlCommand("UPDATE tablo SET ad=@ad,soyad=@soyad WHERE ID=@ID", conn);
            da.UpdateCommand.Parameters.Add("@ad", SqlDbType.VarChar).Value = textBox1.Text;
            da.UpdateCommand.Parameters.Add("@soyad", SqlDbType.VarChar).Value = textBox2.Text;
            da.UpdateCommand.Parameters.Add("@ID", SqlDbType.Int).Value = ds.Tables[0].Rows[tablo.Position][0];

            conn.Open();
            x=da.UpdateCommand.ExecuteNonQuery();
            ds.Clear();
            da.Fill(ds);

            conn.Close();

            if (x >= 1)
                MessageBox.Show("Düzenleme  işlemi tamamlanmıştır");

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            DialogResult dr;

            dr = MessageBox.Show("Silmek istediğinizden Eminmisiniz ?\n Veri tabanından kayıt siliyorsunuz", "Karar Anı", MessageBoxButtons.YesNo);

            if (dr == DialogResult.Yes)
            {

                da.DeleteCommand = new SqlCommand("DELETE FROM tablo WHERE ID=@ID", conn);
                da.DeleteCommand.Parameters.Add("@ID", SqlDbType.Int).Value = ds.Tables[0].Rows[tablo.Position][0];

                conn.Open();

                da.DeleteCommand.ExecuteNonQuery();
                ds.Clear();

                da.Fill(ds);

                conn.Close();
            }
            else
            {

                MessageBox.Show("silme işlemi durdururuldu");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            da.DeleteCommand = new SqlCommand("DELETE FROM tablo WHERE ID=@ID", conn);
            da.DeleteCommand.Parameters.Add("@ID", SqlDbType.Int).Value = textBox3.Text;
            conn.Open();
            

            da.DeleteCommand.ExecuteNonQuery();
           
            ds.Clear();

            da.Fill(ds);

            conn.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            da.SelectCommand = new SqlCommand("SELECT * FROM tablo WHERE ad=@ad", conn);
            da.SelectCommand.Parameters.Add("@ad", SqlDbType.VarChar).Value = textBox4.Text;
            conn.Open();
            ds.Clear();
            da.Fill(ds,"tablo");
            dg.DataSource = ds.Tables["tablo"];
            
            conn.Close();
           


        }

        private void button5_Click(object sender, EventArgs e)
        {


            SqlCommand cmd = new SqlCommand("SELECT ad,soyad FROM tablo WHERE ID=@ID", conn);
            cmd.Parameters.Add("@ID",SqlDbType.Int).Value=textBox5.Text;
            conn.Open();
            try
            {
            SqlDataReader dr = cmd.ExecuteReader();
           
                if (dr.Read())
                {
                    textBox6.Text = dr["ad"].ToString();
                    textBox7.Text = dr["soyad"].ToString();
                }
                else
                {
                    MessageBox.Show("Aradığınız Kayıt bulunamadı");
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Bir hata meydana geldi bu yüzden işlem gerçekleşemiyor"+hata);
            }
         
     
            conn.Close();
  








        }

    }
}
