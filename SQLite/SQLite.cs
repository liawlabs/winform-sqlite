using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//https://www.dotnetcurry.com/aspnet/143/convert-data-reader-to-data-table

namespace SQLite
{
    public partial class SQLite : Form
    {
        public SQLite()
        {
            InitializeComponent();
        }

        private void SQLite_Load(object sender, EventArgs e)
        {
            using (var connection = new SQLiteConnection("Data Source=user.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.Parameters.Clear();

                command.CommandText =
                @"  CREATE TABLE IF NOT EXISTS user (
                        id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL
                    );
                ";
                command.ExecuteNonQuery();

                command.CommandText =
                @"
                       SELECT COUNT(*) 
                       FROM user;
                ";
                var num = command.ExecuteScalar();
                //@"SELECT last_insert_rowid()"

                if (Convert.ToInt32(num) == 0)
                {
                    command.CommandText =
                        @"  INSERT INTO user(name)
                            VALUES('Admin'),
                                  ('Alex'),
                                  ('Chein' );
                     ";
                    command.ExecuteNonQuery();
                }

                command.CommandText =
                    @"  SELECT *
                        FROM user;
                ";
                SQLiteDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable("Users");
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Name", typeof(string));
                dt.Load(reader);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                connection.Close();
            }
        }

        private void btnDelDB_Click(object sender, EventArgs e)
        {
            using (var connection = new SQLiteConnection("Data Source=user.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.Parameters.Clear();
                command.CommandText =
                    @"  DROP TABLE IF EXISTS user;
                ";
                command.ExecuteNonQuery();

                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var connection = new SQLiteConnection("Data Source=user.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.Parameters.Clear();
                command.CommandText =
                @"  INSERT INTO user (name)
                    VALUES ($name);

                    SELECT *
                    FROM user;
                ";
                command.Parameters.AddWithValue("$name", txtName.Text);
                //command.ExecuteNonQuery();
                SQLiteDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGridView1.DataSource = dt;
            }
        }
    }
}
