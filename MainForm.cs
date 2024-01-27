using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace РВП_3
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            bindingSource.DataSource = brands;
            dgvBrands.DataSource = bindingSource;
            dgvModels.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvModels.ReadOnly = true;
        }
        private List<IBrand> brands = new List<IBrand>();
        private void dgvBrands_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == 4)
            {
                IBrand carBrand = brands[e.RowIndex];
                IBrand carBrandNew = null;
                if (carBrand.Type == "truck")
                {
                    carBrandNew = new CBrandTruck(carBrand.Power, carBrand.ModelName, carBrand.BrandName, carBrand.MaxSpeed, carBrand.Type);
                }
                if (carBrand.Type == "car")
                {
                    carBrandNew = new CBrandCar(carBrand.Power, carBrand.ModelName, carBrand.BrandName, carBrand.MaxSpeed, carBrand.Type);
                }
                if (carBrand.Type == "tractor")
                {
                    carBrandNew = new CBrandTractor(carBrand.Power, carBrand.ModelName, carBrand.BrandName, carBrand.MaxSpeed, carBrand.Type);
                }
                brands[e.RowIndex] = carBrandNew;
                dgvBrands.Rows[e.RowIndex].Tag = carBrandNew;
            }
        }

        private void dgvBrands_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var value = dgvBrands.Rows[e.RowIndex].Cells[4].Value;
            if (value != null)
            {
                if (value.ToString() == "car")
                {
                    dgvBrands.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;

                }
                if (value.ToString() == "truck")
                {
                    dgvBrands.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green;
                }
                if (value.ToString() == "tractor")
                {
                    dgvBrands.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Blue;
                }
            }
        }
        private void dgvBrands_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[4].Value = "car";
        }

        private List<IModel> carModels;
        private async Task LoadCarModels(IBrand carModel)
        {
            await Task.Run(() =>
            {
                carModels = CLoader.load(carModel);
            });
        }
        /*private void dgvBrands_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBrands.SelectedRows.Count > 0)
            {
                IBrand carBrand = dgvBrands.SelectedRows[0].Tag as IBrand;
                timer.Start();
                carModels = null;
                Task task = LoadCarModels(carBrand);
                dgvModels.Rows.Clear();
                dgvModels.Columns.Clear();
                dgvModels.Visible = true;
                progressBar.Visible = true;
                progressBar.Value = 0;
            }
            else
            {
                dgvModels.Visible = false;
                progressBar.Visible = false;
            }

        }*/


        private void timer_Tick(object sender, EventArgs e)
        {
            progressBar.Value = CLoader.getProgress();
            if (progressBar.Value == 100)
            {
                while (carModels == null) { }
                createModelTable();
                timer.Stop();
            }

        }
        private void createModelTable()
        {
            dgvModels.AllowUserToAddRows = true;
            DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
            if (carModels[0] is CModelCar)
            {                
                column1.HeaderText = "regNumber";
                column1.ValueType = typeof(int);
                dgvModels.Columns.Add(column1);
                column2.HeaderText = "mediaName";
                column2.ValueType = typeof(string);
                dgvModels.Columns.Add(column2);
                column3.HeaderText = "airbagsNumber";
                column3.ValueType = typeof(int);
                dgvModels.Columns.Add(column3);

                foreach (CModelCar model in carModels.Cast<CModelCar>())
                {
                    DataGridViewRow row = (DataGridViewRow)dgvModels.Rows[0].Clone();
                    row.Cells[0].Value = model.RegNumber;
                    row.Cells[1].Value = model.MediaName;
                    row.Cells[2].Value = model.AirbagsNumber;
                    dgvModels.Rows.Add(row);
                }
            }
            if (carModels[0] is CModelTruck)
            {
                column1.HeaderText = "regNumber";
                column1.ValueType = typeof(int);
                dgvModels.Columns.Add(column1);
                column2.HeaderText = "wheelsNumber";
                column2.ValueType = typeof(int);
                dgvModels.Columns.Add(column2);
                column3.HeaderText = "bodyVolume";
                column3.ValueType = typeof(int);
                dgvModels.Columns.Add(column3);

                foreach (CModelTruck model in carModels.Cast<CModelTruck>())
                {
                    DataGridViewRow row = (DataGridViewRow)dgvModels.Rows[0].Clone();
                    row.Cells[0].Value = model.RegNumber;
                    row.Cells[1].Value = model.WheelsNumber;
                    row.Cells[2].Value = model.BodyVolume;
                    dgvModels.Rows.Add(row);
                }
            }
            if (carModels[0] is CModelTractor)
            {
                column1.HeaderText = "Color";
                column1.ValueType = typeof(string);
                dgvModels.Columns.Add(column1);
                column2.HeaderText = "Weight";
                column2.ValueType = typeof(int);
                dgvModels.Columns.Add(column2);
                column3.HeaderText = "Carrying";
                column3.ValueType = typeof(int);
                dgvModels.Columns.Add(column3);

                foreach (CModelTractor model in carModels.Cast<CModelTractor>())
                {
                    DataGridViewRow row = (DataGridViewRow)dgvModels.Rows[0].Clone();
                    row.Cells[0].Value = model.Color;
                    row.Cells[1].Value = model.Weight;
                    row.Cells[2].Value = model.Carrying;
                    dgvModels.Rows.Add(row);
                }
            }
            dgvModels.AllowUserToAddRows = false;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "XML Files|*.xml";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Stream stream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                    IBrandSerializer.Serialize(stream, brands);
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML Files|*.xml";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Stream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                    List<IBrand> loadedBrands = IBrandSerializer.Deserialize(stream);
                    bindingSource.Clear();
                    for (int i = 0; i < loadedBrands.Count; i++)
                    {
                        bindingSource.Add(loadedBrands[i]);
                        dgvBrands.Rows[i].Tag = loadedBrands[i];                        
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = new CBrandCar();
        }

        private void dgvBrands_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBrands.SelectedRows.Count > 0)
            {
                IBrand carBrand = dgvBrands.SelectedRows[0].Tag as IBrand;
                SynchronizationContext syncContext = SynchronizationContext.Current;
                carModels = null;
                Task task = СonnectServer(syncContext, carBrand);                
                /*dgvModels.Rows.Clear();
                dgvModels.Columns.Clear();
                dgvModels.Visible = true;*/
            }
            else
            {
                dgvModels.Visible = false;
                progressBar.Visible = false;
            }

        }
        
        private async Task СonnectServer(SynchronizationContext syncContext,IBrand brand)
        {
            await Task.Run(() =>
            {
                byte[] bytes = new byte[4096];
                try
                {
                    IPHostEntry host = Dns.GetHostEntry("localhost");
                    IPAddress ipAddress = host.AddressList[0];
                    IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                    Socket sender = new Socket(ipAddress.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);

                    try
                    {
                        sender.Connect(remoteEP);
                        string xmlData = StringXMLData(brand);//написать что надо
                        byte[] msg = Encoding.ASCII.GetBytes(xmlData);
                        int bytesSent = sender.Send(msg);
                        //int bytesRec = sender.Receive(bytes);

                        while (true)
                        {
                            int bytesRec = sender.Receive(bytes);
                            if (bytesRec > 0)
                            {
                                // Конвертация полученных байтов в строку XML
                                xmlData = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                                ReceiveModels(xmlData);
                                Control control = new Control();
                                syncContext.Post(_ =>
                                {
                                    dgvModels.Rows.Clear();
                                    dgvModels.Columns.Clear();
                                    dgvModels.Visible = true;
                                    createModelTable();
                                }, null);
                            }
                            else break;
                        }

                        
                        xmlData = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        ReceiveModels(xmlData);*/

                        sender.Shutdown(SocketShutdown.Both);
                        sender.Close();
                        

                    }
                    catch (ArgumentNullException ane)
                    {
                        MessageBox.Show("ArgumentNullException : " + ane.ToString(), "ArgumentNullException", MessageBoxButtons.OK, MessageBoxIcon.Error);                        
                    }
                    catch (SocketException se)
                    {
                        MessageBox.Show("SocketException : {0}" + se.ToString(), "SocketException",MessageBoxButtons.OK,MessageBoxIcon.Error);                        
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Unexpected exception : {0}" + e.ToString(), "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);                        
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Unexpected exception : {0}" + e.ToString(), "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
            
        }
        private string StringXMLData(IBrand brand)
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (MemoryStream ms = new MemoryStream())
            {
                IBrandSerializer.Serialize(ms, brand);
               
                ms.Position = 0; // переходим в начало потока
                // Загружаем данные из потока в XML документ
                xmlDocument.Load(ms);
            }

            string xmlData;
            using (StringWriter stringWriter = new StringWriter())
            {
                xmlDocument.Save(stringWriter);
                // Получаем строку с XML данными
                xmlData = stringWriter.ToString();
            }
            return xmlData;
        }
        private void ReceiveModels(string xmlData)
        {
            // Создание XML-документа и загрузка данных
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData);
            using (StringReader stringReader = new StringReader(xmlDocument.OuterXml))
            {
                carModels = IModelSerializer.Deserialize(stringReader);
            }
        }
        // Не гласные договоренности

    }
}

