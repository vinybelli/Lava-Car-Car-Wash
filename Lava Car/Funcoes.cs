using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Npgsql;

namespace Lava_Car
{
    public class Funcoes
    {
        public static void GarantirBancoDados()
        {
            const string databaseName = "postgres";
            const string adminConnectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";

            using (var connection = new NpgsqlConnection(adminConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "SELECT 1 FROM pg_database WHERE datname = @dbName", connection))
                {
                    command.Parameters.AddWithValue("@dbName", databaseName);
                    var exists = command.ExecuteScalar();

                    if (exists == null)
                    {
                        using (var createCommand = new NpgsqlCommand(
                            "CREATE DATABASE \"" + databaseName + "\"", connection))
                        {
                            createCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static void GerarPlanilhaExcel(DataGridView DGV, string NomePlanilha)
        {
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = true;

            Excel.Workbook workbook = excelApp.Workbooks.Add(Missing.Value);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Name = NomePlanilha;

            for (int i = 0; i < DGV.ColumnCount; i++)
            {
                worksheet.Cells[1, i + 1] = DGV.Columns[i].HeaderText;
            }

            for (int i = 0; i < DGV.RowCount; i++)
            {
                for (int j = 0; j < DGV.ColumnCount; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = DGV.Rows[i].Cells[j].Value;
                }
            }

            Excel.Range dataRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[DGV.RowCount + 1, DGV.ColumnCount]];
            dataRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            dataRange.Borders.Weight = Excel.XlBorderWeight.xlThin;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Arquivos do Excel (*.xlsx)|*.xlsx";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(saveFileDialog.FileName);
                MessageBox.Show("Dados exportados com sucesso para o Excel!");
            }

            workbook.Close();
            excelApp.Quit();

            ReleaseObject(worksheet);
            ReleaseObject(workbook);
            ReleaseObject(excelApp);
        }

        private static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Ocorreu um erro ao liberar o objeto do Excel: " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
