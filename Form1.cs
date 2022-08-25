using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Newtonsoft.Json;

namespace fatal_predict
{
    public partial class Form1 : Form
    {
        private const string APP_NAME = "ПРЕДСКАЗАНИЯ";
        private readonly string PREDICTION_CONFIG_FILE = $"{Environment.CurrentDirectory}\\predictionConfig.json";
        private string[] _predictions;
        private Random _random = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private async void butpredict_Click(object sender, EventArgs e)
        {
            butpredict.Enabled = false;
           await Task.Run(() =>
           {
               
               for (int i = 0; i <= 100; i++)
               {
                   this.Invoke(new Action(() =>
                   {
                       if(i==progressBar1.Maximum)
                       {
                           progressBar1.Maximum = i + 1;
                           progressBar1.Value = i + 1;
                           progressBar1.Maximum = i;
                       }
                       else
                       {
                           progressBar1.Value = i + 1;
                       }
                       progressBar1.Value = i;
                       
                   }
                   
                   ));
                   Text = $"{i}%";
                   Thread.Sleep(20);
               }
           });
            var index= _random.Next(_predictions.Length);
            var prediction = _predictions[index];
            MessageBox.Show($"{prediction}!");
            progressBar1.Value = 0;
            Text = APP_NAME;
            butpredict.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = APP_NAME;
            try 
            {
                var data = File.ReadAllText(PREDICTION_CONFIG_FILE);
                _predictions = JsonConvert.DeserializeObject<string[]>(data);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
               // MessageBox.Show("нет файла");
            }
            finally
            {
                if (_predictions == null)
                    Close();
                else if (_predictions.Length == 0)
                {
                    MessageBox.Show("предсказания закончились");
                    Close();
                }
            }
        }
    }
}
